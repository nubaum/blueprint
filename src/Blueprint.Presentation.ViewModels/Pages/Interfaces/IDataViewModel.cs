using Blueprint.Presentation.ViewModels.Models;

namespace Blueprint.Presentation.ViewModels.Pages.Interfaces;

public interface IDataViewModel
{
    IEnumerable<DataColor> Colors { get; }
}
