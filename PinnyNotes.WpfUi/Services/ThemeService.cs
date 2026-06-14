using PinnyNotes.Core.Enums;
using PinnyNotes.WpfUi.Helpers;
using PinnyNotes.WpfUi.Themes;

namespace PinnyNotes.WpfUi.Services;

public class ThemeService(AppMetadataService appMetadataService, SettingsService settingsService)
{
    private readonly AppMetadataService _appMetadataService = appMetadataService;
    private readonly SettingsService _settingsService = settingsService;

    public Theme CurrentTheme { get; } = new DefaultTheme();

    public string GetNewNoteColourSchemeName(string? parentColourScheme = null)
    {
        // Set this first as cycle colours wont trigger a change if the next colour if the default for ThemeColours
        string currentColourScheme = _appMetadataService.Metadata.ColourScheme ?? CurrentTheme.DefaultColourSchemeName;
        if (_settingsService.NoteSettings.CycleColours)
            currentColourScheme = GetNextColourSchemeName(currentColourScheme, parentColourScheme);

        return currentColourScheme;
    }

    public string GetNextColourSchemeName(string currentColourSchemeName, string? parentColourSchemeName = null)
    {
        string[] keys = [..CurrentTheme.ColourSchemes.Keys];

        int index = keys.IndexOf(currentColourSchemeName) + 1;

        string nextName = (index == keys.Length) ? keys[0] : keys[index];
        if (nextName == parentColourSchemeName)
        {
            index++;
            nextName = (index == keys.Length) ? keys[0] : keys[index];
        }

        return nextName;
    }

    public Palette GetPalette(string colourSchemeName, ColourMode colourMode)
    {
        ColourScheme colourScheme = CurrentTheme.ColourSchemes[colourSchemeName];

        Palette palette;
        if (colourMode == ColourMode.Dark || (colourMode == ColourMode.System && RegistryHelper.IsDarkMode()))
            palette = colourScheme.Dark;
        else
            palette = colourScheme.Light;

        return palette;
    }
}
