import { Tab } from '../models/tab';

const WELCOME_CODE = `// Welcome to Blueprint
// Click "Home" in the sidebar to open this tab

import { Component, signal } from '@angular/core';
import { bootstrapApplication } from '@angular/platform-browser';

@Component({
  selector: 'app-root',
  template: \`
    <div class="container">
      <h1>Hello, {{ name() }}!</h1>
      <p>Counter: {{ count() }}</p>
      <button (click)="increment()">Increment</button>
      <button (click)="decrement()">Decrement</button>
    </div>
  \`,
})
export class AppComponent {
  name = signal('Blueprint');
  count = signal(0);

  increment() {
    this.count.update(v => v + 1);
  }

  decrement() {
    this.count.update(v => v - 1);
  }
}

bootstrapApplication(AppComponent);
`;

export const WELCOME_TAB: Tab = {
  id: 'home',
  title: 'main.ts',
  content: WELCOME_CODE,
  language: 'typescript',
  isDirty: false,
};
