using System;
using System.Windows.Media;

namespace WordNet.Util
{
    public static class ColorExtensions
    {
        public static (double H, double S, double L) ToHSL(this Color color)
        {
            double r = color.R / 255d;
            double g = color.G / 255d;
            double b = color.B / 255d;

            double max = Math.Max(r, Math.Max(g, b));
            double min = Math.Min(r, Math.Min(g, b));

            double h = 0;
            double s = 0;
            double l = (max + min) / 2;

            if (max != min)
            {
                double d = max - min;
                s = l > 0.5 ? d / (2 - max - min) : d / (max + min);

                if (r == max)
                {
                    h = (g - b) / d + (g < b ? 6 : 0);
                }
                else if (g == max)
                {
                    h = (b - r) / d + 2;
                }
                else
                {
                    h = (r - g) / d + 4;
                }

                h /= 6;
            }
            return (h, s, l);
        }
    }
}