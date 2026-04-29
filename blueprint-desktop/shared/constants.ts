/**
 * Shared constants between main process and preload script.
 * Keep this file free of Node/Electron-specific imports so it
 * can safely be required from both contexts.
 */

export const APP_SCHEME = 'blueprint';

/** Dev server URL served by `ng serve` */
export const DEV_SERVER_URL = 'http://localhost:4200';

/** IPC channel names */
export const IPC_CHANNELS = {
  /** Renderer → Main: request the app version */
  GET_APP_VERSION: 'app:get-version',
  /** Main → Renderer: push the app version back */
  APP_VERSION: 'app:version',
} as const;
