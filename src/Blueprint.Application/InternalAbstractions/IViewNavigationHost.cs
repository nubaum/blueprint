namespace Blueprint.Application.InternalAbstractions;

internal interface IViewNavigationHost
{
    void NavigateToHome();

    void NavigateToData();

    void NavigateToCode();

    void NavigateToSettings();
}
