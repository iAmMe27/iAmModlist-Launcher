using System.ComponentModel;
using System.Windows.Media;
using Serilog;
using iAmModlist_Launcher.Launcher.Settings;
using static iAmModlist_Launcher.Launcher.UI.ThemeHelper;

namespace iAmModlist_Launcher
{
    /// <summary>
    /// Interaction logic for ModlistSettings.xaml
    /// </summary>
    public partial class ModlistSettings : INotifyPropertyChanged
    {
        // Visual settings
        private AppTheme Theme { get; }
        private AccentColour AccentColour { get; }
        
        private LauncherSettings? _settings = new();
        
        public ModlistSettings(AppTheme theme, AccentColour accentColour)
        {
            Theme = theme;
            AccentColour = accentColour;
        
            InitializeComponent();
        
            _ = SettingsInitializer();
        }
        
        private async Task SettingsInitializer()
        {
            _settings = await Settings.LoadSettings();

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
            Log.Information("Modlist settings window closed");
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
}
