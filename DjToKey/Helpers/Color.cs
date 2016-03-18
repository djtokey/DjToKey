using System;
using System.Collections.Generic;

namespace Ktos.DjToKey.Helpers
{
    internal class Color
    {
        private struct Hsb
        {
            public double H { get; set; }
            public double S { get; set; }
            public double B { get; set; }
        }

        private static System.Drawing.Color ToDrawingColor(System.Windows.Media.Color mediacolor)
        {
            return System.Drawing.Color.FromArgb(mediacolor.A, mediacolor.R, mediacolor.G, mediacolor.B);
        }

        private static Hsb ColorToHsb(System.Windows.Media.Color color)
        {
            var c = ToDrawingColor(color);
            var hsb = new Hsb { H = c.GetHue(), S = c.GetSaturation(), B = c.GetBrightness() };

            return hsb;
        }

        public static System.Windows.Media.Color FromArgbHex(string argb)
        {
            //#FFB17807;

            byte a = byte.Parse(argb.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
            byte r = byte.Parse(argb.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(argb.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(argb.Substring(7, 2), System.Globalization.NumberStyles.HexNumber);

            return System.Windows.Media.Color.FromArgb(a, r, g, b);
        }

        /// <summary>
        /// matches the colour passed as an argument with the closest
        /// one contained in the list
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Tuple<string, System.Windows.Media.Color> ClosestMatch(System.Windows.Media.Color color, Dictionary<string, System.Windows.Media.Color> colors)
        {
            Hsb hsb = ColorToHsb(color);

            double h = hsb.H;
            double s = hsb.S;
            double b = hsb.B;

            double ndf = 0;
            double distance = 255;

            Tuple<string, System.Windows.Media.Color> temp = null;

            foreach (var c in colors)
            {
                if (color == c.Value)
                {
                    return Tuple.Create(c.Key, c.Value);
                }

                Hsb cur = ColorToHsb(c.Value);

                double hVal = 0.5 * Math.Pow((double)(cur.H - (double)h), 2);
                double sVal = 0.5 * Math.Pow((double)(cur.S * 100) - (double)(s * 100), 2);
                double lVal = Math.Pow((double)(cur.B * 100) - (double)(b * 100), 2);

                ndf = hVal + sVal + lVal;
                ndf = Math.Sqrt(ndf);

                if (ndf < distance)
                {
                    distance = ndf;
                    temp = Tuple.Create(c.Key, c.Value);
                }
            }

            return temp;
        }
    }
}