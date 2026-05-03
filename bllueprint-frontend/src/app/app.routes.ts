import { Routes } from '@angular/router';
import { ShellComponent } from './core/layout/shell/shell';
import { CodingPageComponent } from './features/coding/pages/coding-page/coding-page';

export const routes: Routes = [
  {
    path: '',
    component: ShellComponent,
    children: [
      {
        path: '',
        redirectTo: 'coding',
        pathMatch: 'full',
      },
      {
        path: 'coding',
        component: CodingPageComponent,
      },
    ],
  },
];