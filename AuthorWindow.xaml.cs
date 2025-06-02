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

    public bool? HideAuthorSettings
    {
        get => _hideAuthorSettings;
        set
        {
            _hideAuthorSettings = value ?? false;
            OnPropertyChange(nameof(HideAuthorSettings));
        }
    }

    public string? WindowTitle
    {
        get => _windowTitle;
        private set
        {
            _windowTitle = value ?? string.Empty;
            OnPropertyChange(nameof(ModlistName));
        }
    }

    private string _modlistName = string.Empty;
    private string _modlistVersion = string.Empty;
    private string _modlistAuthor = string.Empty;
    private string _modlistPath = string.Empty;
    private bool _hideAuthorSettings;
    private string _windowTitle = string.Empty;

    // Visual settings
    private AppTheme Theme { get; }
    private AccentColour AccentColour { get; }

    private LauncherSettings? _settings = new();
        
    public AuthorWindow(AppTheme theme, AccentColour accentColour)
    {
        Theme = theme;
        AccentColour = accentColour;
        
        InitializeComponent();
        
        _ = SettingsInitializer();
    }

    private async Task SettingsInitializer()
    {
        _settings = await Settings.LoadSettings();

        ModlistName = _settings?.ModListName;
        ModlistVersion = _settings?.ModListVersion;
        ModlistAuthor = _settings?.ModListAuthor;
        ModlistPath = _settings?.ModListPath;

        HideAuthorSettings = _settings?.HideAuthorSettings;

        ChkHideAuthorSettings.IsChecked = HideAuthorSettings;

        WindowTitle = ModlistName + " - Author Settings";

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
        if (_settings != null)
        {
            _settings.ModListName = TxtModlistName.Text;
            _settings.ModListVersion = TxtModlistVersion.Text;
            _settings.ModListAuthor = TxtModlistAuthor.Text;
            _settings.ModListPath = TxtModlistPath.Text;

            _settings.HideAuthorSettings = ChkHideAuthorSettings.IsChecked switch
            {
                true => true,
                _ => false
            };
        }

        _ = Save();
    }

    private async Task Save()
    {
        if (_settings is not null)
        {
            Log.Information("Saving settings");
            try
            {
                await Settings.SaveSettings(_settings);
            }
            catch (Exception ex)
            {
                Log.Error("Error saving settings: {ex}", ex);
            }
        }
    }
}