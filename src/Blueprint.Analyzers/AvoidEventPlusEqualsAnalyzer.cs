using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace Blueprint.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class AvoidEventPlusEqualsAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "BEV001";

    private static readonly DiagnosticDescriptor Rule = new(
        id: DiagnosticId,
        title: "Avoid subscribing to events with '+='",
        messageFormat: "Event subscription detected ('+=') on '{0}'. Prefer WeakEventManager instead of direct event subscription.",
        category: "Design",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.RegisterOperationAction(AnalyzeEventAssignment, OperationKind.EventAssignment);
    }

    private static void AnalyzeEventAssignment(OperationAnalysisContext context)
    {
        if (context.Operation is not IEventAssignmentOperation op || !op.Adds)
        {
            return;
        }

        if (op.EventReference is not IEventReferenceOperation eventRef || eventRef.Event is null)
        {
            return;
        }

        context.ReportDiagnostic(Diagnostic.Create(
            Rule,
            op.Syntax.GetLocation(),
            eventRef.Event.Name));
    }
}
