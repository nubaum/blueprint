# blueprint-electron

Electron host for the `blueprint-frontend` Angular application.

## Project structure

```
blueprint-electron/
├── main/
│   └── index.ts          # Main process entry point (Node / Electron APIs)
├── preload/
│   └── index.ts          # Preload script – secure IPC bridge to renderer
├── shared/
│   └── constants.ts      # Constants shared between main + preload
├── dist-electron/        # Compiled output (git-ignored)
│   ├── main/
│   └── preload/
├── dist-frontend/        # Angular build output (git-ignored, populated by build script)
├── tsconfig.electron.json
└── package.json
```

## Prerequisites

- Node.js 20+
- `blueprint-frontend` sibling folder (Angular app)

## Development

```bash
# Install dependencies
npm install

# Start Angular dev server AND Electron simultaneously
npm run dev
```

The Angular app is served by `ng serve` on `http://localhost:4200`.
Electron loads that URL in a `BrowserWindow`.
Hot-module replacement (HMR) works exactly as it does in the browser.

## Production build

```bash
# 1. Compile TypeScript + build Angular
npm run build

# 2. Package into a distributable
npm run dist          # current platform
npm run dist:win      # Windows NSIS installer
npm run dist:mac      # macOS DMG
npm run dist:linux    # Linux AppImage
```

The packaged output is placed in `release/`.

## IPC communication

The preload script exposes a `window.blueprintApi` object to the Angular renderer:

```typescript
// In any Angular component / service:
const version = await window.blueprintApi.getAppVersion();
const isDesktop = window.blueprintApi.isElectron; // true
```

To consume this cleanly from TypeScript, add the following declaration to
`blueprint-frontend/src/electron.d.ts`:

```typescript
interface Window {
  blueprintApi?: {
    getAppVersion(): Promise<string>;
    isElectron: true;
  };
}
```

## Security model

| Setting | Value | Reason |
|---|---|---|
| `contextIsolation` | `true` | Renderer cannot access Node globals |
| `nodeIntegration` | `false` | No accidental Node exposure in Angular |
| `sandbox` | `false` | Preload still needs `require` |
| `webSecurity` | `true` (prod) / `false` (dev) | Dev relaxes CORS for `ng serve` |

Only APIs explicitly bridged in `preload/index.ts` are reachable from the renderer.
