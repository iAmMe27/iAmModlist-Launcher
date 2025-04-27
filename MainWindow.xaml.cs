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

namespace iAmModlist_Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string? ModlistName { get; }
        public string? ModlistVersion { get; }
        public string? ModlistPath { get; }

        public MainWindow()
        {
            var settings = Settings.LoadSettings();

            ModlistName = settings?.ModListName + " Launcher";
            ModlistVersion = settings?.ModListVersion;
            ModlistPath = settings?.ModListPath;
            
            InitializeComponent();
        }
    }
}