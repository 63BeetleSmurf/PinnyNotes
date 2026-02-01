namespace PinnyNotes.WpfUi.Themes;

public class DefaultTheme : Theme
{
    private const string DefaultName = "Default";

    public DefaultTheme() : base(DefaultName)
    {
        string yellowName = "Yellow";
        Palette yellowLightPalette = new("#FEEA00", "#FFFCDD", "#FEF7B1", "#464646", "#000000");
        Palette yellowDarkPalette = new("#FEEA00", "#323232", "#464646", "#B4A81C", "#FFFFFF");
        ColorSchemes[yellowName] = new(yellowName, "#FEF7B1", yellowLightPalette, yellowDarkPalette);

        string orangeName = "Orange";
        Palette orangeLightPalette = new("#FFAB00", "#FEE8B9", "#FFD179", "#464646", "#000000");
        Palette orangeDarkPalette = new("#FFAB00", "#323232", "#464646", "#FFAB00", "#FFFFFF");
        ColorSchemes[orangeName] = new(orangeName, "#FFD179", orangeLightPalette, orangeDarkPalette);

        string redName = "Red";
        Palette redLightPalette = new("#E33036", "#FFC4C6", "#FF7C81", "#464646", "#000000");
        Palette redDarkPalette = new("#E33036", "#323232", "#464646", "#E33036", "#FFFFFF");
        ColorSchemes[redName] = new(redName, "#FF7C81", redLightPalette, redDarkPalette);

        string pinkName = "Pink";
        Palette pinkLightPalette = new("#A72995", "#EBBFE3", "#D986CC", "#464646", "#000000");
        Palette pinkDarkPalette = new ("#A72995", "#323232", "#464646", "#A72995", "#FFFFFF");
        ColorSchemes[pinkName] = new(pinkName, "#D986CC", pinkLightPalette, pinkDarkPalette);

        string purpleName = "Purple";
        Palette purpleLightPalette = new("#625BB8", "#D0CEF3", "#9D9ADD", "#464646", "#000000");
        Palette purpleDarkPalette = new ("#625BB8", "#323232", "#464646", "#625BB8", "#FFFFFF");
        ColorSchemes[purpleName] = new(purpleName, "#9D9ADD", purpleLightPalette, purpleDarkPalette);

        string blueName = "Blue";
        Palette blueLightPalette = new("#1195DD", "#B3D9EC", "#7AC3E6", "#464646", "#000000");
        Palette blueDarkPalette = new("#1195DD", "#323232", "#464646", "#1195DD", "#FFFFFF");
        ColorSchemes[blueName] = new(blueName, "#7AC3E6", blueLightPalette, blueDarkPalette);

        string aquaName = "Aqua";
        Palette aquaLightPalette = new("#16B098", "#C0E2E1", "#97CFC6", "#464646", "#000000");
        Palette aquaDarkPalette = new("#16B098", "#323232", "#464646", "#16B098", "#FFFFFF");
        ColorSchemes[aquaName] = new(aquaName, "#97CFC6", aquaLightPalette, aquaDarkPalette);

        string greenName = "Green";
        Palette greenLightPalette = new("#AACC04", "#E3EBC6", "#C6D67D", "#464646", "#000000");
        Palette greenDarkPalette = new("#AACC04", "#323232", "#464646", "#AACC04", "#FFFFFF");
        ColorSchemes[greenName] = new(greenName, "#C6D67D", greenLightPalette, greenDarkPalette);
    }
}
