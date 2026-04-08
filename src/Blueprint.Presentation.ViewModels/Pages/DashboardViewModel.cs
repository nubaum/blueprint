using System.Windows.Input;
using Blueprint.Abstractions.Messages.Workspace;
using Blueprint.Application.Core;
using Blueprint.Presentation.ViewModels.Pages.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Blueprint.Presentation.ViewModels.Pages;

internal class DashboardViewModel : BindableObject, IDashboardViewModel
{
    private readonly IMediator _mediator;

    public DashboardViewModel(
        IMediator mediator,
        ILogger<DashboardViewModel> logger)
    {
        _mediator = mediator;
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
        await _mediator.Send(new OpenProjectRequest());
    }
}
