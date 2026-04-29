#!/usr/bin/env node
/**
 * Builds blueprint-frontend into blueprint-desktop/dist-frontend.
 * Monaco workers are bundled first so the packaged app ships the same assets
 * used during local development.
 */
const { execSync } = require('child_process');
const path = require('path');
const fs = require('fs');

const electronRoot = path.resolve(__dirname, '..');
const frontendRoot = path.resolve(electronRoot, '..', 'blueprint-frontend');
const outputPath = path.join(electronRoot, 'dist-frontend');
const browserDir = path.join(outputPath, 'browser');

if (fs.existsSync(outputPath)) {
  fs.rmSync(outputPath, { recursive: true, force: true });
  console.log(`Cleaned ${outputPath}`);
}

console.log(`\nBuilding Angular frontend...`);
console.log(`  Source : ${frontendRoot}`);
console.log(`  Output : ${outputPath}\n`);

execSync('node scripts/bundle-workers.js', {
  cwd: frontendRoot,
  stdio: 'inherit',
});

execSync(`npx ng build --output-path "${outputPath}"`, {
  cwd: frontendRoot,
  stdio: 'inherit',
});

if (!fs.existsSync(browserDir)) {
  console.error(`\nERROR: Expected ${browserDir} after build.`);
  process.exit(1);
}

console.log(`\nAngular build complete → ${browserDir}`);
