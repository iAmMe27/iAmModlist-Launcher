
using System.Windows;
using System.Windows.Media;
using iAmModlist_Launcher.Launcher.Settings;
using iAmModlist_Launcher.Launcher.Customisation;
using iAmModlist_Launcher.Launcher.UI;
using ModernWpf;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Controls;

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
            SizeChanged += MainWindow_SizeChanged;
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double newHeight = e.NewSize.Height;
            double newWidth = e.NewSize.Width;

            VanityImage.Width = newWidth;
            VanityImage.Height = newHeight;
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
             
            var pathForVanityImage = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, BackgroundImage);
            VanityImage.Source = new BitmapImage(new Uri(pathForVanityImage, UriKind.Absolute));
            VanityImage.Width = 900;
            VanityImage.Height = 450;

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
                "Red" => Color.FromArgb(0xFF, 0xFF, 0x00, 0x00),
                "Yellow" => Color.FromArgb(0xFF, 0xFF, 0xFF, 0x00),
                "Orange" => Color.FromArgb(0xFF, 0xFF, 0xA5, 0x00),
                _ => Color.FromArgb(0xFF, 0x8A, 0x2B, 0xE2)
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChange(string name) =>
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private void ModlistName_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is TextBlock tb && VanityImage != null)
            {
                // Determine where the text appears relative to the Image
                var location = tb.TransformToVisual(VanityImage).Transform(new Point(0, 0));
                var rect = new Rect(location, new Size(tb.ActualWidth, tb.ActualHeight));

                var color = ContrastColourConverter.GetAverageColour(VanityImage, rect);
                tb.Foreground = ContrastColourConverter.GetContrastingTextBrush(color);
            }
        }
    }
}

