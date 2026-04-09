namespace Blueprint.Application.InternalAbstractions;

public interface IViewNavigationHost
{
    void NavigateToHome();

    void NavigateToData();

    void NavigateToCode();

    void NavigateToSettings();
}
