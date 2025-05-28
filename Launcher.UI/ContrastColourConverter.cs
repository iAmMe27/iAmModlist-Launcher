using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace iAmModlist_Launcher.Launcher.UI
{
    internal static class ContrastColourConverter
    {
        public static Color GetAverageColour(UIElement element, Rect region)
        {
            var rtb = new RenderTargetBitmap((int)region.Width, (int)region.Height, 96, 96, PixelFormats.Pbgra32);
            var dv = new DrawingVisual();

            using (var dc = dv.RenderOpen())
            {
                var vb = new VisualBrush(element);
                dc.DrawRectangle(vb, null, new Rect(new Point(), region.Size));
            }

            rtb.Render(dv);

            var pixels = new byte[(int)(region.Width * region.Height * 4)];
            rtb.CopyPixels(pixels, (int)region.Width * 4, 0);

            long r = 0, g = 0, b = 0;
            var pixelCount = (int)(region.Width * region.Height);

            for (var i = 0; i < pixels.Length; i += 4)
            {
                b += pixels[i];
                g += pixels[i + 1];
                r += pixels[i + 2];
            }

            return Color.FromRgb((byte)(r / pixelCount), (byte)(g / pixelCount), (byte)(b / pixelCount));
        }

        public static Brush GetContrastingTextBrush(Color backgroundColor)
        {
            var brightness = 0.299 * backgroundColor.R + 0.587 * backgroundColor.G + 0.114 * backgroundColor.B;
            return brightness > 128 ? Brushes.Black : Brushes.White;
        }
    }
}
