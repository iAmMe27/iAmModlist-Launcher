using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using iAmModlist_Launcher.Launcher.Json;
using Serilog;

namespace iAmModlist_Launcher.Launcher.Settings
{
    internal static class Settings
    {
        private const string SettingsFileName = "settings.json";

        public static async Task<LauncherSettings?> LoadSettings()
        {
            LauncherSettings? settings;

            try
            {
                Log.Information("Loading settings");
                settings = await JsonInterface.Read<LauncherSettings>(SettingsFileName);
            }
            catch (Exception)
            {
                Log.Error("Settings file not found, generating new");
                settings = await CreateNewSettings();
            }
            
            return settings;
        }

        public static async Task SaveSettings(LauncherSettings settings)
        {
            await JsonInterface.Write(SettingsFileName, settings);
        }
        
        private static async Task<LauncherSettings> CreateNewSettings()
        {
            LauncherSettings settings = new()
            {
                ModListName = "Modlist Name",
                ModListVersion = "0.0.1.0",
                ModListAuthor = "Chris P Bacon",
                ModListPath = "Modlist\\Path",
                HideAuthorSettings = false
            };

            await SaveSettings(settings);
            return settings;
        }
    }

    public class LauncherSettings
    {
        [JsonPropertyName("ModlistName")]
        public string ModListName { get; set; } = string.Empty;
        
        [JsonPropertyName("ModlistVersion")]
        public string ModListVersion { get; set; } = string.Empty;
        
        [JsonPropertyName("ModlistAuthor")]
        public string ModListAuthor { get; set; } = string.Empty;
        
        // TODO: make this a file path type instead of a string
        [JsonPropertyName("ModlistPath")]
        public string ModListPath { get; set; } = string.Empty;
        
        [JsonPropertyName("HideAuthorSettings")]
        public bool HideAuthorSettings { get; set; } = false;
    }
}
