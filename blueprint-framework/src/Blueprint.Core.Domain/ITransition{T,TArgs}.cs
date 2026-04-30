using System.Runtime.CompilerServices;

namespace Blueprint.Core.Domain;

public interface ITransition<in T, in TArgs>
{
    void Invoke(T target, TArgs args, [CallerMemberName] string callerName = "");
}
