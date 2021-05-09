using System.Windows.Media;
using WordNet.Util;
using Xunit;

namespace WordNet.Test
{
    public class ColorExtensionsTests
    {
        [Theory]
        [InlineData(0, 0, 0, 0, 0, 0)]
        [InlineData(255, 255, 255, 0, 0, 1)]
        [InlineData(255, 0, 0, 0, 1, 0.5)]
        [InlineData(0, 255, 0, 120/360d, 1, 0.5)]
        [InlineData(0, 0, 255, 240/360d, 1, 0.5)]
        [InlineData(102, 255, 51, 105/360d, 1, 0.6)]
        [InlineData(122, 122, 82, 60/360d, 0.2, 0.4)]
        [InlineData(172, 159, 198, 260/360d, 0.25, 0.7)]
        public void ColorToHSL(byte r, byte g, byte b, double h, double s, double l)
        {
            var hsl = Color.FromRgb(r, g, b).ToHSL();
            Assert.Equal(h, hsl.H, 2);
            Assert.Equal(s, hsl.S, 2);
            Assert.Equal(l, hsl.L, 2);
        }
    }
}