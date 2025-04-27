using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using iAmModlist_Launcher.Launcher.Settings;
using iAmModlist_Launcher.Launcher.Customisation;
using ModernWpf;
using System.ComponentModel;

namespace iAmModlist_Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        // iAmMe: this might look messy but the combo of public and private strings allows the XAML binding to be bound to data that can be setup before InitializeComponent() is called and updated on change

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

        private string _modlistName = string.Empty;
        private string _modlistVersion = string.Empty;
        private string _modlistAuthor = string.Empty;
        private string _modlistPath = string.Empty;

        // Visual settings
        public string? Theme
        {
            get => _theme;
            set
            {
                _theme = value ?? string.Empty;
                OnPropertyChange(nameof(Theme));
            }
        }

        public string? AccentColour
        {
            get => _accentColour;
            set
            {
                _accentColour = value ?? string.Empty;
                OnPropertyChange(nameof(AccentColour));
            }
        }

        public string? BackgroundImage
        {
            get => _backgroundImage;
            set
            {
                _backgroundImage = value ?? string.Empty;
                OnPropertyChange(nameof(BackgroundImage));
            }
        }

        private string _theme = string.Empty;
        private string _accentColour = string.Empty;
        private string _backgroundImage = string.Empty;

        public MainWindow()
        {
            InitializeComponent();

            _ = SettingsInitializer();
        }

        private async Task SettingsInitializer()
        {
            var settings = await Settings.LoadSettings();
            
            ModlistName = settings?.ModListName + " Launcher";
            ModlistVersion = "v" + settings?.ModListVersion;
            ModlistAuthor = "By " + settings?.ModListAuthor;
            ModlistPath = settings?.ModListPath;

            var customisationSettings = await Customisation.LoadCustomisationSettings(); 
            
            Theme = customisationSettings?.Theme;
            AccentColour = customisationSettings?.AccentColour;
            BackgroundImage = customisationSettings?.BackgroundImage;

            ThemeManager.Current.ApplicationTheme = Theme switch
            {
                "Light" => ApplicationTheme.Light,
                "Dark" => ApplicationTheme.Dark,
                _ => ApplicationTheme.Light
            };

            ThemeManager.Current.AccentColor = AccentColour switch
            {
                "Purple" => Color.FromArgb(0xFF, 0x8A, 0x2B, 0xE2),
                "Blue" => Color.FromArgb(0xFF, 0x00, 0x00, 0xFF),
                "Green" => Color.FromArgb(0xFF, 0x00, 0xFF, 0x00),
                _ => Color.FromArgb(0xFF, 0x8A, 0x2B, 0xE2)
            };
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChange(string name) =>
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

