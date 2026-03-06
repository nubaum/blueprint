using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace Blueprint.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class AvoidAsyncVoidAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "BEV002";
    private const string _description = "'async void' should be avoided because exceptions cannot be awaited/observed and callers cannot know when work completes. " +
        "Prefer 'Task' (or 'Task<T>') return types. The only common exception is UI/event handlers.";

    private static readonly DiagnosticDescriptor Rule = new(
        id: DiagnosticId,
        title: "Avoid 'async void'",
        messageFormat: "'async void' detected on {0}. Use 'Task' (or 'Task<T>') instead.",
        category: "Design",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: _description);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.RegisterSymbolAction(AnalyzeMethod, SymbolKind.Method);
        context.RegisterOperationAction(AnalyzeAnonymousFunction, OperationKind.AnonymousFunction);
    }

    private static void AnalyzeMethod(SymbolAnalysisContext context)
    {
        var method = (IMethodSymbol)context.Symbol;

        if (method.IsImplicitlyDeclared)
        {
            return;
        }

        if (method.IsAsync && method.ReturnsVoid)
        {
            string targetName =
                method.MethodKind == MethodKind.LocalFunction
                    ? $"local function '{method.Name}'"
                    : $"method '{method.ContainingType?.Name}.{method.Name}'";

            Location location = method.Locations.Length > 0 ? method.Locations[0] : Location.None;
            context.ReportDiagnostic(Diagnostic.Create(Rule, location, targetName));
        }
    }

    private static void AnalyzeAnonymousFunction(OperationAnalysisContext context)
    {
        var anon = (IAnonymousFunctionOperation)context.Operation;

        IMethodSymbol? symbol = anon.Symbol;
        if (symbol is null)
        {
            return;
        }

        if (symbol.IsAsync && symbol.ReturnsVoid)
        {
            Location location = anon.Syntax.GetLocation();
            context.ReportDiagnostic(Diagnostic.Create(Rule, location, "anonymous function/lambda"));
        }
    }
}
