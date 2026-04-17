import {
  app,
  BrowserWindow,
  ipcMain,
  shell,
  protocol,
  net,
} from 'electron';
import * as path from 'path';
import * as fs from 'fs';
import * as url from 'url';
import { DEV_SERVER_URL, APP_SCHEME, IPC_CHANNELS } from '../shared/constants';

const isDev = !app.isPackaged;

protocol.registerSchemesAsPrivileged([
  {
    scheme: APP_SCHEME,
    privileges: {
      standard: true,
      secure: true,
      supportFetchAPI: true,
      corsEnabled: true,
      stream: true,
    },
  },
]);

function getBrowserDir(): string {
  if (isDev) {
    return path.join(__dirname, '..', '..', 'dist-frontend', 'browser');
  }

  return path.join(process.resourcesPath, 'dist-frontend', 'browser');
}

function registerProtocolHandler(): void {
  const browserDir = getBrowserDir();
  const normalizedBrowserDir = path.normalize(browserDir + path.sep);

  protocol.handle(APP_SCHEME, async (request) => {
    const requestUrl = new URL(request.url);
    const pathname = decodeURIComponent(requestUrl.pathname);
    const relativePath = pathname.replace(/^\/+/, '') || 'index.html';
    const resolvedPath = path.normalize(path.join(browserDir, relativePath));

    if (
      resolvedPath !== path.normalize(path.join(browserDir, 'index.html')) &&
      !resolvedPath.startsWith(normalizedBrowserDir)
    ) {
      return new Response('Forbidden', { status: 403 });
    }

    const fileExists =
      fs.existsSync(resolvedPath) && fs.statSync(resolvedPath).isFile();

    if (fileExists) {
      return net.fetch(url.pathToFileURL(resolvedPath).toString());
    }

    const looksLikeAssetRequest =
      relativePath.includes('.') ||
      relativePath.startsWith('assets/') ||
      relativePath.startsWith('media/') ||
      relativePath.startsWith('favicon');

    if (looksLikeAssetRequest) {
      return new Response('Not Found', { status: 404 });
    }

    const indexPath = path.join(browserDir, 'index.html');
    return net.fetch(url.pathToFileURL(indexPath).toString());
  });
}

let mainWindow: BrowserWindow | null = null;

function createWindow(): void {
  mainWindow = new BrowserWindow({
    width: 1400,
    height: 900,
    minWidth: 800,
    minHeight: 600,
    show: false,
    backgroundColor: '#1e1e1e',
    titleBarStyle: process.platform === 'darwin' ? 'hiddenInset' : 'default',
    webPreferences: {
      preload: path.join(__dirname, '../preload/index.js'),
      contextIsolation: true,
      nodeIntegration: false,
      sandbox: false,
      webSecurity: true,
    },
  });

  if (isDev) {
    mainWindow.loadURL(DEV_SERVER_URL);
    mainWindow.webContents.openDevTools();
  } else {
    mainWindow.loadURL(`${APP_SCHEME}://app/index.html`);
  }

  mainWindow.once('ready-to-show', () => {
    mainWindow!.show();
  });

  mainWindow.on('closed', () => {
    mainWindow = null;
  });

  mainWindow.webContents.on(
    'did-fail-load',
    (_event, errorCode, _errorDesc, validatedURL) => {
      if (!isDev && validatedURL.startsWith(`${APP_SCHEME}://`)) {
        console.error(`Failed to load ${validatedURL} (${errorCode}), retrying...`);
        setTimeout(() => {
          mainWindow?.loadURL(`${APP_SCHEME}://app/index.html`);
        }, 500);
      }
    }
  );

  mainWindow.webContents.setWindowOpenHandler(({ url: targetUrl }) => {
    if (targetUrl.startsWith('http')) {
      shell.openExternal(targetUrl);
    }
    return { action: 'deny' };
  });

  mainWindow.webContents.openDevTools();
}

app.whenReady().then(() => {
  if (!isDev) {
    registerProtocolHandler();
  }

  createWindow();

  app.on('activate', () => {
    if (BrowserWindow.getAllWindows().length === 0) {
      createWindow();
    }
  });
});

app.on('window-all-closed', () => {
  if (process.platform !== 'darwin') {
    app.quit();
  }
});

ipcMain.handle(IPC_CHANNELS.GET_APP_VERSION, () => app.getVersion());
