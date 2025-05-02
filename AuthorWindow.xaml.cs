using ModernWpf;
using System.Windows;
using System.Windows.Media;

namespace iAmModlist_Launcher;

/// <summary>
/// Interaction logic for AuthorWindow.xaml
/// </summary>
public partial class AuthorWindow : Window
{
    private string? Theme { get; }
    private string? AccentColour { get; }
        
    public AuthorWindow(string theme, string accentColour)
    {
        Theme = theme;
        AccentColour = accentColour;
        
        InitializeComponent();
        
        SettingsInitializer();
    }

    private void SettingsInitializer()
    {
        ThemeManager.Current.ApplicationTheme = Theme switch
        {
            "Light" => ApplicationTheme.Light,
            "Dark" => ApplicationTheme.Dark,
            _ => ApplicationTheme.Light
        };

        Background = Theme switch
        {
            "Light" => new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF)),
            "Dark" => new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x00, 0x00)),
            _ => new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF))
        };

        ThemeManager.Current.AccentColor = AccentColour switch
        {
            "Purple" => Color.FromArgb(0xFF, 0x8A, 0x2B, 0xE2),
            "Blue" => Color.FromArgb(0xFF, 0x00, 0x00, 0xFF),
            "Green" => Color.FromArgb(0xFF, 0x00, 0xFF, 0x00),
            "Red" => Color.FromArgb(0xFF, 0xFF, 0x00, 0x00),
            "Yellow" => Color.FromArgb(0xFF, 0xFF, 0xFF, 0x00),
            "Orange" => Color.FromArgb(0xFF, 0xFF, 0xA5, 0x00),
            _ => Color.FromArgb(0xFF, 0x8A, 0x2B, 0xE2)
        };
    }
}