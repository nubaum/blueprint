import * as monaco from 'monaco-editor';

/**
 * Monaco worker setup.
 *
 * We use a blob: URL wrapper so the worker works regardless of whether
 * the underlying file is ESM or IIFE — the blob just imports it as a
 * module script, which works for both formats in modern browsers/Electron.
 */
(self as Window & typeof globalThis & { MonacoEnvironment?: unknown }).MonacoEnvironment = {
  getWorker(_moduleId: string, label: string): Worker {
    const base = `${location.origin}/assets/monaco`;

    const url = (() => {
      switch (label) {
        case 'typescript':
        case 'javascript':
          return `${base}/ts.worker.js`;
        case 'html':
          return `${base}/html.worker.js`;
        case 'css':
        case 'scss':
          return `${base}/css.worker.js`;
        case 'json':
          return `${base}/json.worker.js`;
        default:
          return `${base}/editor.worker.js`;
      }
    })();

    // Wrap in a blob: URL so the worker can be loaded as a classic script.
    // This avoids cross-origin and ESM/IIFE format issues entirely.
    const blob = new Blob([`importScripts('${url}');`], { type: 'application/javascript' });
    return new Worker(URL.createObjectURL(blob));
  },
};

monaco.languages.typescript.typescriptDefaults.setCompilerOptions({
  target: monaco.languages.typescript.ScriptTarget.ESNext,
  module: monaco.languages.typescript.ModuleKind.ESNext,
  allowNonTsExtensions: true,
  strict: true,
});

monaco.languages.typescript.typescriptDefaults.setDiagnosticsOptions({
  noSemanticValidation: false,
  noSyntaxValidation: false,
});
