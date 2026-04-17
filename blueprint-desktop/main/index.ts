import {
  app,
  BrowserWindow,
  ipcMain,
  shell,
  protocol,
  net,
} from 'electron';
import * as path from 'path';
import * as url from 'url';
import { DEV_SERVER_URL, APP_SCHEME, IPC_CHANNELS } from '../shared/constants';

// ─── Environment ────────────────────────────────────────────────────────────

const isDev = !app.isPackaged;

// ─── Custom protocol (production) ───────────────────────────────────────────
//
// In production the Angular build is copied to dist-frontend/.
// We register a custom `blueprint://` protocol so that the
// Angular router's `<base href="/">` works correctly when the
// app is loaded from disk (file:// breaks Angular routing).

function registerCustomProtocol(): void {
  protocol.registerSchemesAsPrivileged([
    {
      scheme: APP_SCHEME,
      privileges: {
        standard: true,
        secure: true,
        supportFetchAPI: true,
        corsEnabled: true,
      },
    },
  ]);
}

// Call before `app.ready` so the scheme is registered in time.
registerCustomProtocol();

// ─── Window ─────────────────────────────────────────────────────────────────

let mainWindow: BrowserWindow | null = null;

function createWindow(): void {
  mainWindow = new BrowserWindow({
    width: 1400,
    height: 900,
    minWidth: 800,
    minHeight: 600,
    show: false, // show after `ready-to-show` to avoid white flash
    backgroundColor: '#1e1e1e',
    titleBarStyle: process.platform === 'darwin' ? 'hiddenInset' : 'default',
    webPreferences: {
      preload: path.join(__dirname, '../preload/index.js'),
      contextIsolation: true,  // security: keep renderer isolated
      nodeIntegration: false,  // security: no Node in renderer
      sandbox: false,          // preload needs Node APIs
      webSecurity: !isDev,     // relax CORS only during development
    },
  });

  // ── Load content ──────────────────────────────────────────────────────────

  if (isDev) {
    // Development: load Angular dev server (hot reload included)
    mainWindow.loadURL(DEV_SERVER_URL);
    mainWindow.webContents.openDevTools();
  } else {
    // Production: serve from custom protocol
    mainWindow.loadURL(`${APP_SCHEME}://app/index.html`);
  }

  // ── Window lifecycle ──────────────────────────────────────────────────────

  mainWindow.once('ready-to-show', () => {
    mainWindow!.show();
  });

  mainWindow.on('closed', () => {
    mainWindow = null;
  });

  // Open external links in the OS browser, not inside Electron.
  mainWindow.webContents.setWindowOpenHandler(({ url: targetUrl }) => {
    if (targetUrl.startsWith('http')) {
      shell.openExternal(targetUrl);
    }
    return { action: 'deny' };
  });
}

// ─── Protocol handler (production) ──────────────────────────────────────────

function handleCustomProtocol(): void {
  protocol.handle(APP_SCHEME, (request) => {
    // Strip the custom scheme + host to get a relative path.
    // `blueprint://app/index.html` → `/index.html`
    const { pathname } = new URL(request.url);
    const distPath = path.join(__dirname, '../../dist-frontend');
    const filePath = path.join(distPath, pathname);

    return net.fetch(url.pathToFileURL(filePath).toString());
  });
}

// ─── App lifecycle ───────────────────────────────────────────────────────────

app.whenReady().then(() => {
  if (!isDev) {
    handleCustomProtocol();
  }

  createWindow();

  // macOS: re-create window when dock icon is clicked and no windows are open.
  app.on('activate', () => {
    if (BrowserWindow.getAllWindows().length === 0) {
      createWindow();
    }
  });
});

app.on('window-all-closed', () => {
  // On macOS it is conventional to keep the app running until Cmd+Q.
  if (process.platform !== 'darwin') {
    app.quit();
  }
});

// ─── IPC handlers ────────────────────────────────────────────────────────────

ipcMain.handle(IPC_CHANNELS.GET_APP_VERSION, () => app.getVersion());
