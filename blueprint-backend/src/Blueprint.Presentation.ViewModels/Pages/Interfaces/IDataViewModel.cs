using Blueprint.Application.Abstractions;

namespace Blueprint.Presentation.ViewModels.Pages.Interfaces;

public interface IDataViewModel
{
    IEnumerable<DataColor> Colors { get; }
}
