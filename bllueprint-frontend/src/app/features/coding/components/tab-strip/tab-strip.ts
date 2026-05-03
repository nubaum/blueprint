import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { TabsStore } from '../../../../state/tabs/tabs.store';

@Component({
  selector: 'app-tab-strip',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './tab-strip.html',
  styleUrl: './tab-strip.scss',
})
export class TabStripComponent {
  protected readonly tabsStore = inject(TabsStore);

  protected activate(tabId: string): void {
    this.tabsStore.setActiveTab(tabId);
  }

  protected close(event: MouseEvent, tabId: string): void {
    event.stopPropagation();
    this.tabsStore.closeTab(tabId);
  }
}