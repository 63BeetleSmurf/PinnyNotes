using System.Collections.Generic;
using System.Windows.Media;

using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Models;

namespace PinnyNotes.WpfUi.Helpers;

public  static class ThemeHelper
{
    public static readonly Dictionary<ThemeColors, ThemeModel> Themes = new()
    {
        {
            ThemeColors.Yellow,
            new ()
            {
                Name = "Yellow",
                ThemeColor = ThemeColors.Yellow,
                TitleBarColor = Color.FromRgb(254, 247, 177),   // #fef7b1
                BackgroundColor = Color.FromRgb(255, 252, 221), // #fffcdd
                BorderColor = Color.FromRgb(254, 234, 0),       // #feea00
            }
        },
        {
            ThemeColors.Orange,
            new ()
            {
                Name = "Orange",
                ThemeColor = ThemeColors.Orange,
                TitleBarColor = Color.FromRgb(255, 209, 121),   // #ffd179
                BackgroundColor = Color.FromRgb(254, 232, 185), // #fee8b9
                BorderColor = Color.FromRgb(255, 171, 0),       // #ffab00
            }
        },
        {
            ThemeColors.Red,
            new()
            {
                Name = "Red",
                ThemeColor = ThemeColors.Red,
                TitleBarColor = Color.FromRgb(255, 124, 129),   // #ff7c81
                BackgroundColor = Color.FromRgb(255, 196, 198), // #ffc4c6
                BorderColor = Color.FromRgb(227, 48, 54),       // #e33036
            }
        },
        {
            ThemeColors.Pink,
            new()
            {
                Name = "Pink",
                ThemeColor = ThemeColors.Pink,
                TitleBarColor = Color.FromRgb(217, 134, 204),   // #d986cc
                BackgroundColor = Color.FromRgb(235, 191, 227), // #ebbfe3
                BorderColor = Color.FromRgb(167, 41, 149),      // #a72995
            }
        },
        {
            ThemeColors.Purple,
            new()
            {
                Name = "Purple",
                ThemeColor = ThemeColors.Purple,
                TitleBarColor = Color.FromRgb(157, 154, 221),   // #9d9add
                BackgroundColor = Color.FromRgb(208, 206, 243), // #d0cef3
                BorderColor = Color.FromRgb(98, 91, 184),       // #625bb8
            }
        },
        {
            ThemeColors.Blue,
            new()
            {
                Name = "Blue",
                ThemeColor = ThemeColors.Blue,
                TitleBarColor = Color.FromRgb(122, 195, 230),   // #7ac3e6
                BackgroundColor = Color.FromRgb(179, 217, 236), // #b3d9ec
                BorderColor = Color.FromRgb(17, 149, 221),      // #1195dd
            }
        },
        {
            ThemeColors.Aqua,
            new()
            {
                Name = "Aqua",
                ThemeColor = ThemeColors.Aqua,
                TitleBarColor = Color.FromRgb(151, 207, 198),   // #97cfc6
                BackgroundColor = Color.FromRgb(192, 226, 225), // #c0e2e1
                BorderColor = Color.FromRgb(22, 176, 152),      // #16b098
            }
        },
        {
            ThemeColors.Green,
            new()
            {
                Name = "Green",
                ThemeColor = ThemeColors.Green,
                TitleBarColor = Color.FromRgb(198, 214, 125),   // #c6d67d
                BackgroundColor = Color.FromRgb(227, 235, 198), // #e3ebc6
                BorderColor = Color.FromRgb(170, 204, 4),       // #aacc04
            }
        }
    };
}
