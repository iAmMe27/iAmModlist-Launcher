using System.Text.Json.Serialization;
using iAmModlist_Launcher.Launcher.Json;
using Serilog;

namespace iAmModlist_Launcher.Launcher.Customisation;

internal static class Customisation
{
    private const string SettingsFileName = "customisation.json";

    public static async Task<CustomisationSettings?> LoadCustomisationSettings()
    {
        CustomisationSettings? customisationSettings;
        
        try
        {
            Log.Information("Loading customisation settings");
            customisationSettings = await JsonInterface.Read<CustomisationSettings>(SettingsFileName);
        }
        catch (Exception)
        {
            Log.Error("Customisation settings file not found, generating new");
            customisationSettings = await CreateNewSettings();
        }
            
        return customisationSettings;
    }

    public static async Task SaveCustomisationSettings(CustomisationSettings settings)
    {
        await JsonInterface.Write(SettingsFileName, settings);
    }
    
    private static async Task<CustomisationSettings> CreateNewSettings()
    {
        CustomisationSettings settings = new()
        {
            Theme = "Light",
            AccentColour = "Purple",
            BackgroundImage = string.Empty
        };

        await SaveCustomisationSettings(settings);
        return settings;
    }
}

public class CustomisationSettings
{
    [JsonPropertyName("Theme")]
    public string Theme { get; set; } = string.Empty;
    
    [JsonPropertyName("AccentColour")]
    public string AccentColour { get; set; } = string.Empty;
    
    [JsonPropertyName("BackgroundImage")]
    public string BackgroundImage { get; set; } = string.Empty;
}