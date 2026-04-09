using Blueprint.Presentation.ViewModels.Models;

namespace Blueprint.Application.Abstractions;

public interface IRandomColorGenerator
{
    public List<DataColor> GenerateColors();
}
