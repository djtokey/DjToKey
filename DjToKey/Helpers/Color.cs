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

        private static System.Windows.Media.Color FromArgbHex(string argb)
        {
            //#FFB17807;

            byte a = byte.Parse(argb.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
            byte r = byte.Parse(argb.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(argb.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(argb.Substring(7, 2), System.Globalization.NumberStyles.HexNumber);

            return System.Windows.Media.Color.FromArgb(a, r, g, b);
        }

        /// <summary>
        /// Tries to find the closest match between a given color and a
        /// given list of colors
        /// </summary>
        /// <param name="color">A color to match</param>
        /// <param name="colors">A list of colors to match</param>
        /// <returns>The closest color from the list</returns>
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

        /// <summary>
        /// Gets a list of names and values of available accent colors
        /// from MahApps.Metro
        /// </summary>
        /// <returns>A list of available accent colors</returns>
        public static Dictionary<string, System.Windows.Media.Color> AvailableAccents()
        {
            var colors = new Dictionary<string, System.Windows.Media.Color>();
            colors.Add("Amber", FromArgbHex("#FFB17807"));
            colors.Add("Blue", FromArgbHex("#FF086F9E"));
            colors.Add("Brown", FromArgbHex("#FF604220"));
            colors.Add("Cobalt", FromArgbHex("#FF003BB0"));
            colors.Add("Crimson", FromArgbHex("#FF77001B"));
            colors.Add("Cyan", FromArgbHex("#FF1377A7"));
            colors.Add("Emerald", FromArgbHex("#FF006600"));
            colors.Add("Green", FromArgbHex("#FF477D11"));
            colors.Add("Indigo", FromArgbHex("#FF4E00BC"));
            colors.Add("Lime", FromArgbHex("#FF799100"));
            colors.Add("Magenta", FromArgbHex("#FF9F0055"));
            colors.Add("Mauve", FromArgbHex("#FF574766"));
            colors.Add("Olive", FromArgbHex("#FF50634A"));
            colors.Add("Orange", FromArgbHex("#FFB94C00"));
            colors.Add("Pink", FromArgbHex("#FFB45499"));
            colors.Add("Purple", FromArgbHex("#FF5133AB"));
            colors.Add("Red", FromArgbHex("#FFA90E00"));
            colors.Add("Sienna", FromArgbHex("#FF763C21"));
            colors.Add("Steel", FromArgbHex("#FF4A5763"));
            colors.Add("Taupe", FromArgbHex("#FF635939"));
            colors.Add("Teal", FromArgbHex("#FF007E7D"));
            colors.Add("Violet", FromArgbHex("#FF7D00BC"));
            colors.Add("Yellow", FromArgbHex("#FFBBA404"));
            return colors;
        }
    }
}