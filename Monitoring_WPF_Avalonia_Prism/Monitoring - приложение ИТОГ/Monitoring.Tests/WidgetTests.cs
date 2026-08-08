using Monitoring.Models.Widgets;
using Xunit;

namespace Monitoring.Tests
{
    public class WidgetTests
    {
        // Тест 1: ProgressWidget обрезает значение в диапазон 0..100
        [Theory]
        [InlineData(150, 100)]   // выше максимума
        [InlineData(-20, 0)]     // ниже нуля
        [InlineData(55, 55)]     // в диапазоне
        public void ProgressWidget_обрезает_значение_в_диапазон_0_100(int input, int expected)
        {
            var widget = new ProgressWidgetVM(new FakePollingService(), m => 0, m => "");
            widget.Progress = input;
            Assert.Equal(expected, widget.Progress);
        }

        // Тест 2: ProgressWidget возвращает цвет по порогу
        [Theory]
        [InlineData(10, "Red")]
        [InlineData(20, "Red")]
        [InlineData(35, "Orange")]
        [InlineData(50, "Yellow")]
        [InlineData(75, "LightGreen")]
        [InlineData(95, "Green")]
        public void ProgressWidget_возвращает_цвет_по_порогу(int progress, string expectedColor)
        {
            var widget = new ProgressWidgetVM(new FakePollingService(), m => 0, m => "");
            widget.Progress = progress;
            Assert.Equal(expectedColor, widget.Color);
        }

        // Тест 3: GaugeWidget считает размеры зон из порогов (и обнуляет отрицательные)
        [Theory]
        [InlineData(6000, 4500, 5500, 4500, 1000, 500)]   // обычный случай
        [InlineData(100, 50, 200, 50, 150, 0)]            // RedFrom > Max → RedSize = 0
        public void GaugeWidget_считает_размеры_зон(double max, double yellowFrom, double redFrom,
                                                    double expectedGreen, double expectedYellow, double expectedRed)
        {
            var gauge = new GaugeWidgetVM(new FakePollingService(), m => 0d, m => "")
            {
                Max = max,
                YellowFrom = yellowFrom,
                RedFrom = redFrom
            };

            Assert.Equal(expectedGreen, gauge.GreenSize);
            Assert.Equal(expectedYellow, gauge.YellowSize);
            Assert.Equal(expectedRed, gauge.RedSize);
        }
    }
}
