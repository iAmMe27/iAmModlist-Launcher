using System.Windows;
using System.Windows.Media;
using iAmModlist_Launcher.Launcher.Settings;
using iAmModlist_Launcher.Launcher.Customisation;
using iAmModlist_Launcher.Launcher.UI;
using static iAmModlist_Launcher.Launcher.UI.ThemeHelper;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Controls;
using Serilog;

namespace iAmModlist_Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
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

        public bool? HideAuthorSettings
        {
            get => _hideAuthorSettings;
            set
            {
                _hideAuthorSettings = value ?? false;
                OnPropertyChange(nameof(HideAuthorSettings));
            }
        }

        private string _modlistName = string.Empty;
        private string _modlistVersion = string.Empty;
        private string _modlistAuthor = string.Empty;
        private string _modlistPath = string.Empty;
        private bool _hideAuthorSettings;

        // Visual settings
        public AppTheme? LauncherTheme
        {
            get => _launcherTheme;
            set
            {
                _launcherTheme = value ?? AppTheme.Dark;
                OnPropertyChange(nameof(LauncherTheme));
            }
        }

        public AccentColour? AccentColour
        {
            get => _accentColour;
            set
            {
                _accentColour = value ?? ThemeHelper.AccentColour.Purple;
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

        private AppTheme _launcherTheme = AppTheme.Dark;
        private AccentColour _accentColour = ThemeHelper.AccentColour.Purple;
        private string _backgroundImage = string.Empty;

        public MainWindow()
        {
            InitializeComponent();

            _ = SettingsInitializer();
            _ = CustomisationInitializer();
            SizeChanged += MainWindow_SizeChanged;
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            VanityImage.Width = e.NewSize.Width;
            VanityImage.Height = e.NewSize.Height;
        }

        private async Task SettingsInitializer()
        {
            var settings = await Settings.LoadSettings();
            
            ModlistName = settings?.ModListName + " Launcher";
            ModlistVersion = "v" + settings?.ModListVersion;
            ModlistAuthor = "By " + settings?.ModListAuthor;
            ModlistPath = settings?.ModListPath;
            HideAuthorSettings = settings?.HideAuthorSettings;
            
            BtnAuthorSettings.Visibility = HideAuthorSettings switch
            {
                true => Visibility.Hidden,
                _ => Visibility.Visible
            };
        }

        private async Task CustomisationInitializer()
        {
            var customisationSettings = await Customisation.LoadCustomisationSettings();

            if (Enum.TryParse(customisationSettings?.Theme, out AppTheme theme))
            {
                _launcherTheme = theme;
            }
            else
            {
                theme = AppTheme.Light;
            }

            if (Enum.TryParse(customisationSettings?.AccentColour, out AccentColour accentColour))
            {
                _accentColour = accentColour;
            }
            else
            {
                accentColour = ThemeHelper.AccentColour.Purple;
            }
            
            SetTheme(theme, accentColour);
            
            Background = theme switch
            {
                AppTheme.Light => new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF)),
                AppTheme.Dark => new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x00, 0x00)),
                _ => new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF))
            };
            
            BackgroundImage = customisationSettings?.BackgroundImage;

            if (BackgroundImage != null)
            {
                var pathForVanityImage = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, BackgroundImage);
                VanityImage.Source = new BitmapImage(new Uri(pathForVanityImage, UriKind.Absolute));
                VanityImage.Width = 900;
                VanityImage.Height = 450;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChange(string name) =>
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private void ModlistNameText_Loaded(object sender, RoutedEventArgs e)
        {
           Foreground = UpdateTextColour(sender);
        }

        private Brush UpdateTextColour(object sender)
        {
            try
            {
                if (sender is TextBlock tb && VanityImage != null)
                {
                    // Determine where the text appears relative to the Image
                    var location = tb.TransformToVisual(VanityImage).Transform(new Point(0, 0));
                    var rect = new Rect(location, new Size(tb.ActualWidth, tb.ActualHeight));

                    var color = ContrastColourConverter.GetAverageColour(VanityImage, rect);
                    return ContrastColourConverter.GetContrastingTextBrush(color);
                }

                return Brushes.Black;
            }
            catch (Exception e)
            {
                Log.Fatal("Error updating text colour: \n" + e.Message);
                
                return Brushes.Black;
            }
            
        }

        private void BtnAuthorSettings_Click(object sender, RoutedEventArgs e)
        {
            Log.Information("Opening author window");
            var authorWindow = new AuthorWindow(_launcherTheme, _accentColour);
            authorWindow.Closed += AuthorWindow_Closed;
            authorWindow.Show();
        }

        private void AuthorWindow_Closed(object? sender, EventArgs e)
        {
            _ = SettingsInitializer();
        }

        private void BtnSettings_OnClick(object sender, RoutedEventArgs e)
        {
            Log.Information("Opening settings window");
            var settingsWindow = new ModlistSettings(_launcherTheme, _accentColour);
            settingsWindow.Closed += ModlistSettings_Closed;
            settingsWindow.Show();
        }
        
        private static void ModlistSettings_Closed(object? sender, EventArgs e)
        {
            
        }
    }
}

