using System.Windows.Input;
using Blueprint.Abstractions.Application.Workspace;
using Blueprint.Abstractions.Messages.Workspace;
using Blueprint.Presentation.ViewModels.Core;
using Blueprint.Presentation.ViewModels.Pages.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;

namespace Blueprint.Presentation.ViewModels.Pages;

internal class DashboardViewModel : BindableObject, IDashboardViewModel
{
    private readonly IMediator _mediator;
    private readonly IPathProvider _pathProvider;

    public DashboardViewModel(
        IMediator mediator,
        IPathProvider pathProvider,
        ILogger<DashboardViewModel> logger)
    {
        _mediator = mediator;
        _pathProvider = pathProvider;
        CounterIncrementCommand = new DelegateCommand(OnCounterIncrement);
        OpenProjectCommand = new AsyncCommand(OpenProjectAsync, logger);
    }

    public ICommand OpenProjectCommand { get; }

    public ICommand CounterIncrementCommand { get; }

    public int Counter
    {
        get;
        set => SetField(ref field, value);
    }

    private void OnCounterIncrement()
    {
        Counter += 2;
    }

    private async Task OpenProjectAsync()
    {
        _pathProvider.CreateSourceFolderPath();
        var dialog = new OpenFolderDialog
        {
            InitialDirectory = _pathProvider.SourceFolderPath
        };

        if(dialog.ShowDialog() ?? false)
        {
            await _mediator.Send(new OpenProjectRequest { ProjectFilePath = dialog.FolderName });
        }
    }
}
