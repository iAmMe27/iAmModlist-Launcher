using System.Windows;
using iAmModlist_Launcher.Launcher.Customisation;
using iAmModlist_Launcher.Launcher.Logging;

namespace iAmModlist_Launcher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void AppStartup(object sender, EventArgs e)
        {
            LauncherLogger.CreateLogger();
        }
    }
}
