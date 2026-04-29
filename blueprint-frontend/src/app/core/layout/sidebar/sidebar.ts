import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { TabsStore } from '../../../state/tabs/tabs.store';

interface SidebarNavItem {
  label: string;
  action: () => void;
}

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.scss',
})
export class SidebarComponent {
  private readonly tabsStore = inject(TabsStore);

  protected readonly navItems: SidebarNavItem[] = [
    {
      label: 'Home',
      action: () => this.openHome(),
    },
  ];

  protected openHome(): void {
    this.tabsStore.openHomeTab();
  }
}