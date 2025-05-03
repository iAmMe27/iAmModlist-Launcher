using ModernWpf;
using System.Windows.Media;

namespace iAmModlist_Launcher.Launcher.UI;

public static class ThemeHelper
{
    public enum AppTheme
    {
        Light,
        Dark
    }

    public enum AccentColour
    {
        Purple,
        Blue,
        Green,
        Red,
        Yellow,
        Orange
    }

    public static void SetTheme(AppTheme theme, AccentColour accentColour)
    {
        ThemeManager.Current.ApplicationTheme = theme switch
        {
            AppTheme.Light => ApplicationTheme.Light,
            AppTheme.Dark => ApplicationTheme.Dark,
            _ => ApplicationTheme.Light
        };

        ThemeManager.Current.AccentColor = accentColour switch
        {
            AccentColour.Purple => Color.FromArgb(0xFF, 0x8A, 0x2B, 0xE2),
            AccentColour.Blue => Color.FromArgb(0xFF, 0x00, 0x00, 0xFF),
            AccentColour.Green => Color.FromArgb(0xFF, 0x00, 0xFF, 0x00),
            AccentColour.Red => Color.FromArgb(0xFF, 0xFF, 0x00, 0x00),
            AccentColour.Yellow => Color.FromArgb(0xFF, 0xFF, 0xFF, 0x00),
            AccentColour.Orange => Color.FromArgb(0xFF, 0xFF, 0xA5, 0x00),
            _ => Color.FromArgb(0xFF, 0x8A, 0x2B, 0xE2)
        };
    }

}
