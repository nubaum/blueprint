using Blueprint.Application.Abstractions;
using Blueprint.Presentation.Adapters;
using Blueprint.Presentation.ViewModels.Pages.Interfaces;

namespace Blueprint.Presentation.ViewModels.Pages;

internal class DataViewModel : BindableObject, IDataViewModel
{
    private readonly IRandomColorGenerator _randomColorGenerator;
    private readonly List<DataColor> _colors = [];

    public DataViewModel(IRandomColorGenerator randomColorGenerator)
    {
        _randomColorGenerator = randomColorGenerator;
        InitializeViewModel();
    }

    public IEnumerable<DataColor> Colors => _colors;

    private void InitializeViewModel()
    {
        _colors.Clear();
        _colors.AddRange(_randomColorGenerator.GenerateColors());
    }
}
