using System.Windows.Media;
using Blueprint.Application.Abstractions;

namespace Blueprint.Services;

internal class RandomColorGenerator : IRandomColorGenerator
{
    public List<DataColor> GenerateColors()
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

        return colorCollection;
    }
}
