using System.Windows.Media;
using Blueprint.Models;
using Blueprint.ViewModels.Core;
using Wpf.Ui.Abstractions.Controls;

namespace Blueprint.ViewModels.Pages;

public class DataViewModel : NotifyPropertyChangedBase, INavigationAware
{
    private readonly List<DataColor> _colors = [];
    private bool _isInitialized;

    public IEnumerable<DataColor> Colors => _colors;

    public Task OnNavigatedToAsync()
    {
        if (!_isInitialized)
        {
            InitializeViewModel();
        }

        return Task.CompletedTask;
    }

    public Task OnNavigatedFromAsync() => Task.CompletedTask;

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

        _isInitialized = true;
    }
}
