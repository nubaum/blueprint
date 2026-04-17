import * as monaco from 'monaco-editor';

(self as Window & typeof globalThis & { MonacoEnvironment?: unknown }).MonacoEnvironment = {
  getWorkerUrl(_moduleId: string, label: string): string {
    const base = `${location.origin}/assets/monaco`;
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
