using System.Windows.Media;
using System.ComponentModel;
using iAmModlist_Launcher.Launcher.Settings;
using static iAmModlist_Launcher.Launcher.UI.ThemeHelper;
using Serilog;

namespace iAmModlist_Launcher;

/// <summary>
/// Interaction logic for AuthorWindow.xaml
/// </summary>
public partial class AuthorWindow : INotifyPropertyChanged
{
    // Function settings
    public string? ModlistName
    {
        get => _modlistName;
        set
        {
            _modlistName = value ?? string.Empty;
            OnPropertyChange(nameof(ModlistName));
        }
    }

    public string? ModlistVersion
    {
        get => _modlistVersion;
        set
        {
            _modlistVersion = value ?? string.Empty;
            OnPropertyChange(nameof(ModlistVersion));
        }
    }

    public string? ModlistAuthor
    {
        get => _modlistAuthor;
        set
        {
            _modlistAuthor = value ?? string.Empty;
            OnPropertyChange(nameof(ModlistAuthor));
        }
    }

    public string? ModlistPath
    {
        get => _modlistPath;
        set
        {
            _modlistPath = value ?? string.Empty;
            OnPropertyChange(nameof(ModlistPath));
        }
    }

    public string? WindowTitle
    {
        get => _windowTitle;
        set
        {
            _windowTitle = value ?? string.Empty;
            OnPropertyChange(nameof(ModlistName));
        }
    }

    private string _modlistName = string.Empty;
    private string _modlistVersion = string.Empty;
    private string _modlistAuthor = string.Empty;
    private string _modlistPath = string.Empty;
    private string _windowTitle = string.Empty;

    // Visual settings
    private AppTheme Theme { get; }
    private AccentColour AccentColour { get; }

    LauncherSettings? settings = new();
        
    public AuthorWindow(AppTheme theme, AccentColour accentColour)
    {
        Theme = theme;
        AccentColour = accentColour;
        
        InitializeComponent();
        
        _ = SettingsInitializer();
    }

    private async Task SettingsInitializer()
    {
        settings = await Settings.LoadSettings();

        ModlistName = settings?.ModListName;
        ModlistVersion = settings?.ModListVersion;
        ModlistAuthor = settings?.ModListAuthor;
        ModlistPath = settings?.ModListPath;

        WindowTitle = ModlistName + " - Settings";

        SetTheme(Theme, AccentColour);

        Background = Theme switch
        {
            AppTheme.Light => new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF)),
            AppTheme.Dark => new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x00, 0x00)),
            _ => new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF))
        };
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChange(string name) =>
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private void BtnClose_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        Log.Information("Author window closed");
        Close();
    }

    private void BtnSave_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        settings.ModListName = txtModlistName.Text;
        settings.ModListVersion = txtModlistVersion.Text;
        settings.ModListAuthor = txtModlistAuthor.Text;
        settings.ModListPath = txtModlistPath.Text;

        _ = Save();
    }

    private async Task Save()
    {
        if (settings is not null)
        {
            Log.Information("Saving settings");
            try
            {
                await Settings.SaveSettings(settings);
            }
            catch (Exception ex)
            {
                Log.Error("Error saving settings: {ex}", ex);
            }
        }
    }
}