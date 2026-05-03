#!/usr/bin/env node
/**
 * Bundles Monaco workers into self-contained IIFE scripts.
 * Output goes to public/assets/monaco/ so ng serve picks them up automatically.
 * Run before `ng serve` or `ng build`.
 */
const { execSync } = require('child_process');
const path = require('path');
const fs = require('fs');

const root = path.resolve(__dirname, '..');
const outDir = path.join(root, 'public', 'assets', 'monaco');

fs.mkdirSync(outDir, { recursive: true });

const esbuild = path.join(root, 'node_modules', '.bin', 'esbuild');

const workers = [
  { name: 'editor.worker.js', entry: 'monaco-editor/esm/vs/editor/editor.worker' },
  { name: 'ts.worker.js',     entry: 'monaco-editor/esm/vs/language/typescript/ts.worker' },
  { name: 'html.worker.js',   entry: 'monaco-editor/esm/vs/language/html/html.worker' },
  { name: 'css.worker.js',    entry: 'monaco-editor/esm/vs/language/css/css.worker' },
  { name: 'json.worker.js',   entry: 'monaco-editor/esm/vs/language/json/json.worker' },
];

for (const worker of workers) {
  const out = path.join(outDir, worker.name);
  if (fs.existsSync(out)) {
    console.log(`  ✓ ${worker.name} (cached)`);
    continue;
  }
  console.log(`  Bundling ${worker.name}...`);
  execSync(
    `"${esbuild}" "${worker.entry}" --bundle --outfile="${out}" --format=iife --platform=browser --minify`,
    { cwd: root, stdio: 'inherit' }
  );
}

console.log(`Workers ready → ${outDir}`);
