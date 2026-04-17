import { Component } from '@angular/core';
import { TabStripComponent } from '../../../features/coding/components/tab-strip/tab-strip';
import { EditorHostComponent } from '../../../features/coding/components/tab-strip/editor-host/editor-host';

@Component({
  selector: 'app-workbench',
  standalone: true,
  imports: [TabStripComponent, EditorHostComponent],
  templateUrl: './workbench.html',
  styleUrl: './workbench.scss',
})
export class WorkbenchComponent {}