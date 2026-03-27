using System.Windows.Input;
using Blueprint.Abstractions.Messages.Workspace;
using Blueprint.Presentation.ViewModels.Core;
using Blueprint.Presentation.ViewModels.Pages.Interfaces;
using MediatR;
using Microsoft.Win32;

namespace Blueprint.Presentation.ViewModels.Pages;

internal class DashboardViewModel : BindableObject, IDashboardViewModel
{
    private readonly IMediator _mediator;
    private int _counter;

    public DashboardViewModel(IUiCoreServices uiCoreServices, IMediator mediator)
        : base(uiCoreServices)
    {
        _mediator = mediator;
        CounterIncrementCommand = new DelegateCommand(OnCounterIncrement);
        OpenProjectCommand = new AsyncCommand(OpenProjectAsync);
    }

    public ICommand OpenProjectCommand { get; }

    public ICommand CounterIncrementCommand { get; }

    public int Counter
    {
        get => _counter;
        set => SetField(ref _counter, value);
    }

    private void OnCounterIncrement()
    {
        Counter++;
    }

    private async Task OpenProjectAsync()
    {
        var dialog = new OpenFolderDialog
        {
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        };
        if(dialog.ShowDialog() ?? false)
        {
            await _mediator.Send(new OpenProjectRequest { ProjectFilePath = dialog.FolderName });
        }
    }
}
