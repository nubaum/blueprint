using System.Runtime.CompilerServices;

namespace Blueprint.Core.Domain;

public interface ITransition<in T>
{
    void Invoke(T target, [CallerMemberName] string callerName = "");
}
