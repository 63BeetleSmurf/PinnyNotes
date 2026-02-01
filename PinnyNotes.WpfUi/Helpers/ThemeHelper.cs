using PinnyNotes.Core.Enums;
using PinnyNotes.WpfUi.Themes;

namespace PinnyNotes.WpfUi.Helpers;

public  static class ThemeHelper
{
    // TitleBarHex, TitleButtonHex, BackgroundHex, BorderHex, TextHex
    public static readonly Dictionary<ThemeColors, Theme> Themes = new()
    {
        {
            ThemeColors.Yellow,
            new Theme("Yellow", ThemeColors.Yellow, "#FEF7B1")
            {
                NoteLightPalette = new NotePalette("#FEF7B1", "#464646", "#FFFCDD", "#FEEA00", "#000000"),
                NoteDarkPalette = new NotePalette("#464646", "#FEEA00", "#323232", "#FEEA00", "#FFFFFF")
            }
        },
        {
            ThemeColors.Orange,
            new Theme("Orange", ThemeColors.Orange, "#FFD179")
            {
                NoteLightPalette = new NotePalette("#FFD179", "#464646", "#FEE8B9", "#FFAB00", "#000000"),
                NoteDarkPalette = new NotePalette("#464646", "#FFAB00", "#323232", "#FFAB00", "#FFFFFF")
            }
        },
        {
            ThemeColors.Red,
            new Theme("Red", ThemeColors.Red, "#FF7C81")
            {
                NoteLightPalette = new NotePalette("#FF7C81", "#464646", "#FFC4C6", "#E33036", "#000000"),
                NoteDarkPalette = new NotePalette("#464646", "#E33036", "#323232", "#E33036", "#FFFFFF")
            }
        },
        {
            ThemeColors.Pink,
            new Theme("Pink", ThemeColors.Pink, "#D986CC")
            {
                NoteLightPalette = new NotePalette("#D986CC", "#464646", "#EBBFE3", "#A72995", "#000000"),
                NoteDarkPalette = new NotePalette("#464646", "#A72995", "#323232", "#A72995", "#FFFFFF")
            }
        },
        {
            ThemeColors.Purple,
            new Theme("Purple", ThemeColors.Purple, "#9D9ADD")
            {
                NoteLightPalette = new NotePalette("#9D9ADD", "#464646", "#D0CEF3", "#625BB8", "#000000"),
                NoteDarkPalette = new NotePalette("#464646", "#625BB8", "#323232", "#625BB8", "#FFFFFF")
            }
        },
        {
            ThemeColors.Blue,
            new Theme("Blue", ThemeColors.Blue, "#7AC3E6")
            {
                NoteLightPalette = new NotePalette("#7AC3E6", "#464646", "#B3D9EC", "#1195DD", "#000000"),
                NoteDarkPalette = new NotePalette("#464646", "#1195DD", "#323232", "#1195DD", "#FFFFFF")
            }
        },
        {
            ThemeColors.Aqua,
            new Theme("Aqua", ThemeColors.Aqua, "#97CFC6")
            {
                NoteLightPalette = new NotePalette("#97CFC6", "#464646", "#C0E2E1", "#16B098", "#000000"),
                NoteDarkPalette = new NotePalette("#464646", "#16B098", "#323232", "#16B098", "#FFFFFF")
            }
        },
        {
            ThemeColors.Green,
            new Theme("Green", ThemeColors.Green, "#C6D67D")
            {
                NoteLightPalette = new NotePalette("#C6D67D", "#464646", "#E3EBC6", "#AACC04", "#000000"),
                NoteDarkPalette = new NotePalette("#464646", "#AACC04", "#323232", "#AACC04", "#FFFFFF")
            }
        }
    };
}
