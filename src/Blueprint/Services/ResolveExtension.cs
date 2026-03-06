using System.Windows.Markup;

namespace Blueprint.Services;

[MarkupExtensionReturnType(typeof(object))]
public sealed class ResolveExtension : MarkupExtension
{
    public Type? Type { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return Type is null
            ? throw new InvalidOperationException($"{nameof(ResolveExtension)} requires {nameof(Type)}.")
            : App.GetService(Type);
    }
}
