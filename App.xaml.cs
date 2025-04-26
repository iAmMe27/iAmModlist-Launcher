using System.Windows;
using iAmModlist_Launcher.Launcher.Logging;
using iAmModlist_Launcher.Launcher.Settings;
using Serilog;
using Serilog.Core;

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
            Log.Information("### iAmModlist Launcher ###");
            
            var settings = Settings.LoadSettings();
        }
    }
}
