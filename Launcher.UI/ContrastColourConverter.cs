using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Serilog;
using Serilog.Core;

namespace iAmModlist_Launcher.Launcher.UI
{
    internal static class ContrastColourConverter
    {
        public static Color GetAverageColour(UIElement element, Rect region)
        {
            // Do some actual input validation
            if (region.Width <= 0 || region.Height <= 0)
            {
                Log.Error("Contrast colour converter: region width or height was equal to or less than 0");
                return Colors.Black; 
            }
            
            var width = (int)region.Width;
            var height = (int)region.Height;
            
            var rtb = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
            var dv = new DrawingVisual();

            using (var dc = dv.RenderOpen())
            {
                var vb = new VisualBrush(element);
                dc.DrawRectangle(vb, null, new Rect(new Point(), region.Size));
            }

            rtb.Render(dv);

            var pixels = new byte[(width * height * 4)];
            rtb.CopyPixels(pixels, (int)region.Width * 4, 0);

            long r = 0, g = 0, b = 0;
            var pixelCount = (long)(width * height);
            
            // avoid a divide by zero error here
            if (pixelCount == 0)
                return Colors.Black;

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
