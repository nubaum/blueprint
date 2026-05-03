/**
 * Type declaration for the Electron IPC bridge exposed by the preload script.
 *
 * Place this file at `src/electron.d.ts` inside `bllueprint-frontend`.
 * TypeScript will pick it up automatically — no changes to tsconfig required.
 *
 * `bllueprintApi` is `undefined` when the Angular app runs in a regular
 * browser (outside Electron), so always guard with an optional check:
 *
 * ```typescript
 * if (window.bllueprintApi?.isElectron) {
 *   const version = await window.bllueprintApi.getAppVersion();
 * }
 * ```
 */
interface Window {
  bllueprintApi?: {
    /** Returns the Electron app version string (e.g. "0.0.1"). */
    getAppVersion(): Promise<string>;

    /** Always `true` when running inside Electron. Use to conditionally
     *  show desktop-only UI (window controls, native menus, etc.). */
    isElectron: true;
  };
}
