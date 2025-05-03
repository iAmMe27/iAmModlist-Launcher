using static iAmModlist_Launcher.Launcher.UI.ThemeHelper;
using System.Windows;
using System.Windows.Media;

namespace iAmModlist_Launcher;

/// <summary>
/// Interaction logic for AuthorWindow.xaml
/// </summary>
public partial class AuthorWindow : Window
{
    private AppTheme Theme { get; }
    private AccentColour AccentColour { get; }
        
    public AuthorWindow(AppTheme theme, AccentColour accentColour)
    {
        Theme = theme;
        AccentColour = accentColour;
        
        InitializeComponent();
        
        SettingsInitializer();
    }

    private void SettingsInitializer()
    {
        SetTheme(Theme, AccentColour);

        Background = Theme switch
        {
            AppTheme.Light => new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF)),
            AppTheme.Dark => new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x00, 0x00)),
            _ => new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF))
        };
    }
}