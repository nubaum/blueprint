#!/usr/bin/env node
/**
 * 1. Runs `ng build` for blueprint-frontend
 * 2. Bundles Monaco workers into self-contained scripts using esbuild
 *    (esbuild ships with @angular/build so no extra install needed)
 *
 * Monaco's worker files in node_modules are ESM entry points that import
 * many other modules. They cannot be served as-is — they must be bundled
 * into single self-contained scripts first.
 */

const { execSync } = require('child_process');
const path = require('path');
const fs = require('fs');

const electronRoot = path.resolve(__dirname, '..');
const frontendRoot = path.resolve(electronRoot, '..', 'blueprint-frontend');
const outputPath   = path.join(electronRoot, 'dist-frontend');
const browserDir   = path.join(outputPath, 'browser');
const monacoOutDir = path.join(browserDir, 'assets', 'monaco');

// ── Step 1: Clean ────────────────────────────────────────────────────────────

if (fs.existsSync(outputPath)) {
  fs.rmSync(outputPath, { recursive: true, force: true });
  console.log(`Cleaned ${outputPath}`);
}

// ── Step 2: ng build ─────────────────────────────────────────────────────────

console.log(`\nBuilding Angular frontend...`);
console.log(`  Source : ${frontendRoot}`);
console.log(`  Output : ${outputPath}\n`);

execSync(
  `npx ng build --output-path "${outputPath}"`,
  { cwd: frontendRoot, stdio: 'inherit' }
);

if (!fs.existsSync(browserDir)) {
  console.error(`\nERROR: Expected ${browserDir} after build.`);
  process.exit(1);
}

// ── Step 3: Bundle Monaco workers ────────────────────────────────────────────
//
// esbuild is bundled inside @angular/build. We resolve it from there.

console.log(`\nBundling Monaco workers...`);

fs.mkdirSync(monacoOutDir, { recursive: true });

// Try to find esbuild from @angular/build's dependencies
const esbuildBin = path.join(
  frontendRoot, 'node_modules', '@angular', 'build', 'node_modules', '.bin', 'esbuild'
);
const esbuildBinFallback = path.join(frontendRoot, 'node_modules', '.bin', 'esbuild');

const esbuild = fs.existsSync(esbuildBin) ? esbuildBin : esbuildBinFallback;

const workers = [
  {
    name: 'editor.worker.js',
    entry: 'monaco-editor/esm/vs/editor/editor.worker',
  },
  {
    name: 'ts.worker.js',
    entry: 'monaco-editor/esm/vs/language/typescript/ts.worker',
  },
  {
    name: 'html.worker.js',
    entry: 'monaco-editor/esm/vs/language/html/html.worker',
  },
  {
    name: 'css.worker.js',
    entry: 'monaco-editor/esm/vs/language/css/css.worker',
  },
  {
    name: 'json.worker.js',
    entry: 'monaco-editor/esm/vs/language/json/json.worker',
  },
];

for (const worker of workers) {
  const outFile = path.join(monacoOutDir, worker.name);
  console.log(`  Bundling ${worker.name}...`);

  execSync(
    `"${esbuild}" "${worker.entry}" --bundle --outfile="${outFile}" --format=iife --platform=browser --minify`,
    { cwd: frontendRoot, stdio: 'inherit' }
  );
}

console.log(`\nAngular build complete → ${browserDir}`);
console.log(`Monaco workers bundled → ${monacoOutDir}`);
