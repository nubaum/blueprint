import {
  AfterViewInit,
  Component,
  ElementRef,
  OnDestroy,
  ViewChild,
  effect,
  inject,
} from '@angular/core';
import { CommonModule } from '@angular/common';

import * as monaco from 'monaco-editor/esm/vs/editor/editor.api.js';

import { TabsStore } from '../../../../../state/tabs/tabs.store';

@Component({
  selector: 'app-editor-host',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './editor-host.html',
  styleUrl: './editor-host.scss',
})
export class EditorHostComponent implements AfterViewInit, OnDestroy {
  @ViewChild('editorContainer')
  private editorContainer!: ElementRef<HTMLDivElement>;

  private readonly tabsStore = inject(TabsStore);

  private editor: monaco.editor.IStandaloneCodeEditor | null = null;
  private readonly models = new Map<string, monaco.editor.ITextModel>();
  private resizeObserver: ResizeObserver | null = null;
  private suppressContentSync = false;

  constructor() {
    effect(() => {
      const activeTab = this.tabsStore.activeTab();

      if (!this.editor || !activeTab) {
        return;
      }

      this.switchToTab(activeTab.id);
    });
  }

  ngAfterViewInit(): void {
    this.createEditor();
  }

  ngOnDestroy(): void {
    this.resizeObserver?.disconnect();

    this.models.forEach(model => model.dispose());
    this.models.clear();

    this.editor?.dispose();
    this.editor = null;
  }

  protected hasTabs(): boolean {
    return this.tabsStore.tabs().length > 0;
  }

  private createEditor(): void {
    this.editor = monaco.editor.create(this.editorContainer.nativeElement, {
      value: '',
      language: 'typescript',
      theme: 'vs-dark',
      fontSize: 14,
      fontFamily: "'Fira Code', 'Cascadia Code', 'Consolas', 'Courier New', monospace",
      fontLigatures: true,
      lineHeight: 22,
      minimap: { enabled: true },
      scrollBeyondLastLine: false,
      automaticLayout: false,
      tabSize: 2,
      insertSpaces: true,
      renderWhitespace: 'selection',
      bracketPairColorization: { enabled: true },
      guides: { bracketPairs: true },
      padding: { top: 12, bottom: 12 },
      cursorBlinking: 'smooth',
      cursorSmoothCaretAnimation: 'on',
      smoothScrolling: true,
      renderLineHighlight: 'all',
      scrollbar: {
        verticalScrollbarSize: 10,
        horizontalScrollbarSize: 10,
      },
    });

    this.editor.onDidChangeModelContent(() => {
      if (this.suppressContentSync) {
        return;
      }

      const activeTab = this.tabsStore.activeTab();
      if (!activeTab || !this.editor) {
        return;
      }

      const content = this.editor.getValue();

      if (content !== activeTab.content) {
        this.tabsStore.updateTabContent(activeTab.id, content);
      }
    });

    const activeTab = this.tabsStore.activeTab();
    if (activeTab) {
      this.switchToTab(activeTab.id);
    }

    this.resizeObserver = new ResizeObserver(() => {
      this.editor?.layout();
    });

    this.resizeObserver.observe(this.editorContainer.nativeElement);
  }

  private switchToTab(tabId: string): void {
    const tab = this.tabsStore.tabs().find(x => x.id === tabId);
    if (!tab || !this.editor) {
      return;
    }

    let model = this.models.get(tabId);

    if (!model) {
      model = monaco.editor.createModel(
        tab.content,
        tab.language,
        monaco.Uri.parse(`inmemory://model/${tabId}`)
      );

      this.models.set(tabId, model);
    } else if (model.getValue() !== tab.content) {
      this.suppressContentSync = true;
      model.setValue(tab.content);
      this.suppressContentSync = false;
    }

    this.editor.setModel(model);
    this.editor.focus();
  }
}