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
      bypassCSP: true,
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

  protocol.handle(APP_SCHEME, (request) => {
    const { pathname } = new URL(request.url);

    // Prevent path traversal attacks
    const relative = path.normalize(pathname).replace(/^(\.\.(\/|\\|$))+/, '');
    const filePath = path.join(browserDir, relative);

    // Ensure the resolved path is still inside browserDir
    if (!filePath.startsWith(browserDir)) {
      return new Response('Forbidden', { status: 403 });
    }

    // Serve the file if it exists
    if (fs.existsSync(filePath) && fs.statSync(filePath).isFile()) {
      return net.fetch(url.pathToFileURL(filePath).toString());
    }

    // Only fall back to index.html for navigation requests (HTML pages),
    // not for assets (JS, CSS, fonts, images, workers).
    // This supports Angular's client-side routing without swallowing
    // real 404s for missing assets.
    const ext = path.extname(filePath).toLowerCase();
    const isAsset = ['.js', '.css', '.ttf', '.woff', '.woff2', '.png',
                     '.jpg', '.svg', '.ico', '.json', '.map'].includes(ext);

    if (isAsset) {
      return new Response(`Not found: ${pathname}`, { status: 404 });
    }

    // Navigation request with no extension (Angular route) → serve index.html
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
      webSecurity: false,
    },
  });

  if (isDev) {
    mainWindow.loadURL(DEV_SERVER_URL);
    mainWindow.webContents.openDevTools();
  } else {
    mainWindow.loadURL(`${APP_SCHEME}://app/index.html`);
  }

  mainWindow.once('ready-to-show', () => mainWindow!.show());
  mainWindow.on('closed', () => { mainWindow = null; });

  mainWindow.webContents.on('did-fail-load', (_e, code, _desc, validatedURL) => {
    if (!isDev && validatedURL.startsWith(`${APP_SCHEME}://`)) {
      setTimeout(() => mainWindow?.loadURL(`${APP_SCHEME}://app/index.html`), 500);
    }
  });

  mainWindow.webContents.setWindowOpenHandler(({ url: u }) => {
    if (u.startsWith('http')) shell.openExternal(u);
    return { action: 'deny' };
  });
}

app.whenReady().then(() => {
  if (!isDev) registerProtocolHandler();
  createWindow();
  app.on('activate', () => {
    if (BrowserWindow.getAllWindows().length === 0) createWindow();
  });
});

app.on('window-all-closed', () => {
  if (process.platform !== 'darwin') app.quit();
});

ipcMain.handle(IPC_CHANNELS.GET_APP_VERSION, () => app.getVersion());
