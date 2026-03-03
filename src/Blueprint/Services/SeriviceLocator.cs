using System.Windows.Markup;

namespace Blueprint.Services
{
    [MarkupExtensionReturnType(typeof(object))]
    public sealed class ResolveExtension : MarkupExtension
    {
        public Type? Type { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Type is null)
                throw new InvalidOperationException($"{nameof(ResolveExtension)} requires {nameof(Type)}.");

            return App.GetService(Type);
        }
    }

    public sealed class ServiceLocator
    {
        public static T Get<T>() where T : notnull => App.GetService<T>();
    }
}