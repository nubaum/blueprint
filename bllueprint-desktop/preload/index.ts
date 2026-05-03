import { contextBridge, ipcRenderer } from 'electron';
import { IPC_CHANNELS } from '../shared/constants';

/**
 * The `bllueprintApi` object is exposed on `window.bllueprintApi` inside the
 * Angular renderer. Only the methods listed here are accessible — the full
 * Electron / Node API is never exposed.
 */
const bllueprintApi = {
  /** Returns the Electron app version string (e.g. "0.0.1"). */
  getAppVersion: (): Promise<string> =>
    ipcRenderer.invoke(IPC_CHANNELS.GET_APP_VERSION),

  /** True when running inside Electron (useful for conditional UI). */
  isElectron: true as const,
};

contextBridge.exposeInMainWorld('bllueprintApi', bllueprintApi);

// ─── Type augmentation (consumed by Angular) ──────────────────────────────
//
// This declaration is for the preload type-check only.
// The Angular project should reference `../bllueprint-electron/preload/index.d.ts`
// or copy the declaration below into a `src/electron.d.ts` file.

export type BllueprintApi = typeof bllueprintApi;

declare global {
  interface Window {
    bllueprintApi: BllueprintApi;
  }
}
