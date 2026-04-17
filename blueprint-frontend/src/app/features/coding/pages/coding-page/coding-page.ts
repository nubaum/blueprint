import { Component } from '@angular/core';
import { WorkbenchComponent } from '../../../../core/layout/workbench/workbench';

@Component({
  selector: 'app-coding-page',
  standalone: true,
  imports: [WorkbenchComponent],
  templateUrl: './coding-page.html',
  styleUrl: './coding-page.scss',
})
export class CodingPageComponent {}