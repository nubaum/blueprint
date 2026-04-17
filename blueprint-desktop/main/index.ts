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

// ─── Environment ─────────────────────────────────────────────────────────────

const isDev = !app.isPackaged;

// ─── Custom protocol ──────────────────────────────────────────────────────────
//
// IMPORTANT: registerSchemesAsPrivileged must be called before app.whenReady()
// and before any other use of `protocol`. This is an Electron hard requirement.

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

// ─── Resolve the Angular browser output directory ─────────────────────────────

function getBrowserDir(): string {
  if (isDev) {
    return path.join(__dirname, '..', '..', 'dist-frontend', 'browser');
  }
  // In a packaged app, electron-builder places files listed under `files`
  // into <resources>/app.asar — but static assets are NOT inside the asar.
  // We use `extraResources` to place dist-frontend/browser alongside it.
  return path.join(process.resourcesPath, 'dist-frontend', 'browser');
}

// ─── Protocol handler ─────────────────────────────────────────────────────────

function registerProtocolHandler(): void {
  const browserDir = getBrowserDir();

  protocol.handle(APP_SCHEME, (request) => {
    const { pathname } = new URL(request.url);

    // Strip leading slash and normalise
    const relative = pathname.replace(/^\//, '') || 'index.html';
    const filePath = path.join(browserDir, relative);

    // If the file exists, serve it directly
    if (fs.existsSync(filePath) && fs.statSync(filePath).isFile()) {
      return net.fetch(url.pathToFileURL(filePath).toString());
    }

    // Fallback to index.html for Angular client-side routing
    const indexPath = path.join(browserDir, 'index.html');
    return net.fetch(url.pathToFileURL(indexPath).toString());
  });
}

// ─── Window ───────────────────────────────────────────────────────────────────

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

  // If the window fails to load (e.g. protocol not ready), retry once
  mainWindow.webContents.on('did-fail-load', (_event, errorCode, _errorDesc, validatedURL) => {
    if (!isDev && validatedURL.startsWith(`${APP_SCHEME}://`)) {
      console.error(`Failed to load ${validatedURL} (${errorCode}), retrying...`);
      setTimeout(() => {
        mainWindow?.loadURL(`${APP_SCHEME}://app/index.html`);
      }, 500);
    }
  });

  mainWindow.webContents.setWindowOpenHandler(({ url: targetUrl }) => {
    if (targetUrl.startsWith('http')) {
      shell.openExternal(targetUrl);
    }
    return { action: 'deny' };
  });

  //TODO: Remove this once we're confident the protocol handler is reliable. It's
  mainWindow.webContents.openDevTools();
}

// ─── App lifecycle ────────────────────────────────────────────────────────────

app.whenReady().then(() => {
  // Register protocol handler before creating the window
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

// ─── IPC ──────────────────────────────────────────────────────────────────────

ipcMain.handle(IPC_CHANNELS.GET_APP_VERSION, () => app.getVersion());
