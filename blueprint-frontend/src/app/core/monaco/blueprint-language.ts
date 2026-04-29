import * as monaco from 'monaco-editor';

// ---------------------------------------------------------------------------
// Color palette reference
//
//  keyword.structural   #6AB0C8  — enum | value | aggregate | dto | controller | at | uses | process
//  keyword.type         #b84580  — guid | text | date | number | bool
//  keyword.set          #e06c75  — Set | To | ToBluePrintDefault
//  keyword.requires     #e5c07b  — Requires | EqualsTo | DifferentThan | LowerThan | HigherThan
//  keyword.error        #FFB86C  — ErrorMessage
//  keyword.http         #ae6bbc  — get | post | put | delete | patch
//  keyword.modifier     #a6a04d  — key | required | input | output | map | default | now
//  type.name            #A08FD4  — PascalCase user-defined identifiers
//  identifier           #98C9A3  — camelCase identifiers
//  keyword.architecture #db7b98  — saga | cqrs | outputbox
//  string               #CE9178  - camelCase identifiers
//  comment              #6A9955
//  number               #B5CEA8
//  delimiter.curly      #FFB86C
//  delimiter.paren      #88C8A8
// ---------------------------------------------------------------------------

monaco.languages.register({ id: 'blueprint' });

monaco.languages.setLanguageConfiguration('blueprint', {
  brackets: [
    ['{', '}'],
    ['(', ')'],
  ],
  autoClosingPairs: [
    { open: '{', close: '}' },
    { open: '(', close: ')' },
    { open: '"', close: '"' },
  ],
  surroundingPairs: [
    { open: '{', close: '}' },
    { open: '(', close: ')' },
    { open: '"', close: '"' },
  ],
  comments: {
    lineComment: '//',
    blockComment: ['/*', '*/'],
  },
});

monaco.languages.setMonarchTokensProvider('blueprint', {
  defaultToken: 'identifier',
  tokenPostfix: '.blueprint',
  tokenizer: {
    root: [
      // Comments
      [/\/\/.*$/, 'comment'],
      [/\/\*/, 'comment', '@blockComment'],

      // Strings
      [/\$"/, 'string.interpolated', '@interpolatedString'],
      [/"/, 'string', '@string'],

      // Structural keywords
      [/\b(uses|enum|value|aggregate|dto|controller|process|api|handler|command|notification)\b/, 'keyword.structural'],

      // Set / To flow — transition assignment (ToBluePrintDefault must come before To)
      [/\bToBluePrintDefault\b/, 'keyword.set'],
      [/\b(Set|To)\b/, 'keyword.set'],

      // Requires flow — transition guards
      [/\b(Requires|EqualsTo|DifferentThan|LowerThan|HigherThan)\b/, 'keyword.requires'],

      // ErrorMessage
      [/\bErrorMessage\b/, 'keyword.error'],

      // HTTP verbs — controller methods
      [/\b(get|post|put|delete|patch)\b/, 'keyword.http'],

      // Field / endpoint modifiers
      [/\b(key|required|input|output|at|maps|default|now|use|list|of|basepath|on|invoke|fetch|save|emit|notify|with|create)\b/, 'keyword.modifier'],
      
      // Architectural keywords
      [/\b(saga|cqrs|outbox)\b/, 'keyword.architecture'],

      // Primitive types (all lowercase, renamed as per spec)
      [/\b(text|guid|date|number|bool)\b/, 'keyword.type'],

      // Constraints / validators
      [/\b(max|min|notEmpty|isEmail)\b/, 'keyword.constraint'],

      // Literals
      [/\b(true|false)\b/, 'keyword.literal'],

      // Operators
      [/<->/, 'operator.map'],
      [/=>/, 'operator.lambda'],
      [/=/, 'operator.equals'],

      // Delimiters
      [/[{}]/, 'delimiter.curly'],
      [/[()]/, 'delimiter.paren'],
      [/[<>]/, 'delimiter.angle'],
      [/[;]/, 'delimiter.semi'],
      [/,/, 'delimiter.comma'],

      // Identifiers — PascalCase = user-defined type, camelCase = variable/field
      [/[A-Z][a-zA-Z0-9]*/, 'type.name'],
      [/[a-zA-Z_]\w*/, 'identifier'],

      [/\d+\.\d+/, 'number.float'],
      [/\d+/, 'number'],
      [/[ \t\r\n]+/, 'white'],
    ],

    blockComment: [
      [/[^/*]+/, 'comment'],
      [/\*\//, 'comment', '@pop'],
      [/[/*]/, 'comment'],
    ],

    string: [
      [/[^"\\]+/, 'string'],
      [/\\./, 'string.escape'],
      [/"/, 'string', '@pop'],
    ],

    interpolatedString: [
      [/\{/, 'string.interpolated.delimiter', '@interpolatedExpr'],
      [/[^"{]+/, 'string.interpolated'],
      [/"/, 'string.interpolated', '@pop'],
    ],

    interpolatedExpr: [
      [/[a-zA-Z_]\w*/, 'identifier.interpolated'],
      [/\}/, 'string.interpolated.delimiter', '@pop'],
    ],
  },
});

monaco.editor.defineTheme('blueprint-dark', {
  base: 'vs-dark',
  inherit: true,
  rules: [
    { token: '', foreground: 'D4D4D4', background: '1C1B1F' },

    // Structure
    { token: 'keyword.structural', foreground: '6AB0C8', fontStyle: 'bold' },

    // Primitive types
    { token: 'keyword.type', foreground: 'b84580' },

    // Transition assignment: Set / To
    { token: 'keyword.set', foreground: 'e06c75', fontStyle: 'bold' },

    // Transition guards: Requires / EqualsTo / DifferentThan etc.
    { token: 'keyword.requires', foreground: 'e5c07b', fontStyle: 'bold' },

    // ErrorMessage
    { token: 'keyword.error', foreground: 'FFB86C', fontStyle: 'italic' },

    // Controller HTTP verbs
    { token: 'keyword.http', foreground: 'ae6bbc', fontStyle: 'bold' },

    // Modifiers
    { token: 'keyword.modifier', foreground: 'a6a04d' },

    // Constraints / validators
    { token: 'keyword.constraint', foreground: 'BD8DE9' },

    // Keyword architecture
    { token: 'keyword.architecture', foreground: 'db7b98' },

    // Literals
    { token: 'keyword.literal', foreground: '569CD6' },

    // User-defined types (PascalCase)
    { token: 'type.name', foreground: 'A08FD4' },

    // Variables / fields (camelCase)
    { token: 'identifier', foreground: '98C9A3' },
    { token: 'identifier.interpolated', foreground: '6FCEBC', fontStyle: 'italic' },

    // Strings
    { token: 'string', foreground: 'CE9178', fontStyle: 'italic'  },
    { token: 'string.interpolated', foreground: 'CE9178', fontStyle: 'italic' },
    { token: 'string.interpolated.delimiter', foreground: 'BD8DE9', fontStyle: 'bold' },
    { token: 'string.escape', foreground: 'D7BA7D' },

    // Comments
    { token: 'comment', foreground: '6A9955', fontStyle: 'italic' },

    // Numbers
    { token: 'number', foreground: 'B5CEA8' },
    { token: 'number.float', foreground: 'B5CEA8' },

    // Operators
    { token: 'operator.map', foreground: 'E9C639', fontStyle: 'bold' },
    { token: 'operator.lambda', foreground: 'E9C639', fontStyle: 'bold' },
    { token: 'operator.equals', foreground: 'E9C639' },

    // Delimiters
    { token: 'delimiter.curly', foreground: 'FFB86C' },
    { token: 'delimiter.paren', foreground: '88C8A8' },
    { token: 'delimiter.angle', foreground: 'ABB2BF' },
    { token: 'delimiter.semi', foreground: '808080' },
    { token: 'delimiter.comma', foreground: '808080' },
  ],
  colors: {
    'editor.background': '#1C1B1F',
    'editor.foreground': '#D4D4D4',
    'editorLineNumber.foreground': '#4A4A5A',
    'editorLineNumber.activeForeground': '#6AB0C8',
    'editor.selectionBackground': '#264F78',
    'editor.lineHighlightBackground': '#2A2A3E',
    'editorCursor.foreground': '#A08FD4',
    'editorBracketHighlight.foreground1': '#FFB86C',
    'editorBracketHighlight.foreground2': '#BD8DE9',
    'editorBracketHighlight.foreground3': '#4EC9B0',
    'editorBracketHighlight.foreground4': '#E9C639',
    'editorBracketHighlight.foreground5': '#6AB0C8',
    'editorBracketHighlight.foreground6': '#CE9178',
    'editorBracketHighlight.unexpectedBracket.foreground': '#E24B4A',
  },
});
