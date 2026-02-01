namespace PinnyNotes.WpfUi.Themes;

public class Theme(string name)
{
    public string Name { get; set; } = name;
    public Dictionary<string, ColorScheme> ColorSchemes { get; set; } = [];

    public string GetNextColorScheme(string? currentColorScheme, string? parentColorScheme = null)
    {
        string[] keys = [.. ColorSchemes.Keys];

        int index = keys.IndexOf(currentColorScheme) + 1; // Index will be -1 if null, but +1 fixes anyway.
        string nextColorScheme = (index == keys.Length) ? keys[0] : keys[index];

        if (nextColorScheme == parentColorScheme)
        {
            index++;
            nextColorScheme = (index == keys.Length) ? keys[0] : keys[index];
        }

        return nextColorScheme;
    }
}
