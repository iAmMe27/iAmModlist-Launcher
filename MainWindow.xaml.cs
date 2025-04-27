using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using iAmModlist_Launcher.Launcher.Settings;
using iAmModlist_Launcher.Launcher.Customisation;

namespace iAmModlist_Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Function settings
        public string? ModlistName { get; set; }
        public string? ModlistVersion { get; set; }
        public string? ModlistAuthor { get; set; }
        public string? ModlistPath { get; set; }

        // Customisation settings
        public string? Theme { get; set; }
        public string? AccentColour { get; set; }
        public string? BackgroundImage { get; set; }

        public MainWindow()
        {
            // Initialise settings
            _ = SettingsInitialiser();

            InitializeComponent();
        }

        public async Task SettingsInitialiser()
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
        }
    }
}

