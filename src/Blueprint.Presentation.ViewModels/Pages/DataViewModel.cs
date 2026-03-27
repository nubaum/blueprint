using System.Windows.Media;
using Blueprint.Presentation.ViewModels.Core;
using Blueprint.Presentation.ViewModels.Models;
using Blueprint.Presentation.ViewModels.Pages.Interfaces;

namespace Blueprint.Presentation.ViewModels.Pages;

internal class DataViewModel : BindableObject, IDataViewModel
{
    private readonly List<DataColor> _colors = [];

    public DataViewModel()
    {
        InitializeViewModel();
    }

    public IEnumerable<DataColor> Colors => _colors;

    private void InitializeViewModel()
    {
        var random = new Random();
        var colorCollection = new List<DataColor>();

        for (int i = 0; i < 8192; i++)
        {
            colorCollection.Add(
                new DataColor
                {
                    Color = new SolidColorBrush(
                        Color.FromArgb(
                            200,
                            (byte)random.Next(0, 250),
                            (byte)random.Next(0, 250),
                            (byte)random.Next(0, 250)))
                });
        }

        _colors.Clear();
        _colors.AddRange(colorCollection);
    }
}
