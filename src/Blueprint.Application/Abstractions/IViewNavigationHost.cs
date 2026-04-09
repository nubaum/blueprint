namespace Blueprint.Application.Abstractions;

public interface IViewNavigationHost
{
    void NavigateToHome();

    void NavigateToData();

    void NavigateToCode();

    void NavigateToSettings();
}
