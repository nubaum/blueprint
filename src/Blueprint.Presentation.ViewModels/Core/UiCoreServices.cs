using Microsoft.Extensions.Logging;
using Wpf.Ui;

namespace Blueprint.Presentation.ViewModels.Core;

internal class UiCoreServices(ILoggerFactory loggerFactory, ISnackbarService snackbarService) : IUiCoreServices
{
    public ILoggerFactory LoggerFactory => loggerFactory;

    public ISnackbarService SnackbarService => snackbarService;
}
