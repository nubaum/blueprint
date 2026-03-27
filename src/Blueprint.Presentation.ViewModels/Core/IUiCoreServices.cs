using Microsoft.Extensions.Logging;
using Wpf.Ui;

namespace Blueprint.Presentation.ViewModels.Core;

public interface IUiCoreServices
{
    ILoggerFactory LoggerFactory { get; }

    ISnackbarService SnackbarService { get; }
}
