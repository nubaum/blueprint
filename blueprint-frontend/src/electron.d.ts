/**
 * Type declaration for the Electron IPC bridge exposed by the preload script.
 *
 * Place this file at `src/electron.d.ts` inside `blueprint-frontend`.
 * TypeScript will pick it up automatically — no changes to tsconfig required.
 *
 * `blueprintApi` is `undefined` when the Angular app runs in a regular
 * browser (outside Electron), so always guard with an optional check:
 *
 * ```typescript
 * if (window.blueprintApi?.isElectron) {
 *   const version = await window.blueprintApi.getAppVersion();
 * }
 * ```
 */
interface Window {
  blueprintApi?: {
    /** Returns the Electron app version string (e.g. "0.0.1"). */
    getAppVersion(): Promise<string>;

    /** Always `true` when running inside Electron. Use to conditionally
     *  show desktop-only UI (window controls, native menus, etc.). */
    isElectron: true;
  };
}
