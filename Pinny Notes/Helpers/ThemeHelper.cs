using System.Collections.Generic;
using System.Windows.Media;

using PinnyNotes.WpfUi.Enums;

namespace PinnyNotes.WpfUi.Helpers;

public static class ThemeHelper
{
    public static readonly Dictionary<ThemeColors, Color[]> Colors = new() {
        {
            ThemeColors.Yellow,
            new[] {
                Color.FromRgb(254, 247, 177),   // #fef7b1
                Color.FromRgb(255, 252, 221),   // #fffcdd
                Color.FromRgb(254, 234, 0),     // #feea00
            }
        },
        {
            ThemeColors.Orange,
            new[] {
                Color.FromRgb(255, 209, 121),   // #ffd179
                Color.FromRgb(254, 232, 185),   // #fee8b9
                Color.FromRgb(255, 171, 0),     // #ffab00
            }
        },
        {
            ThemeColors.Red,
            new[] {
                Color.FromRgb(255, 124, 129),   // #ff7c81
                Color.FromRgb(255, 196, 198),   // #ffc4c6
                Color.FromRgb(227, 48, 54),     // #e33036
            }
        },
        {
            ThemeColors.Pink,
            new[] {
                Color.FromRgb(217, 134, 204),   // #d986cc
                Color.FromRgb(235, 191, 227),   // #ebbfe3
                Color.FromRgb(167, 41, 149),    // #a72995
            }
        },
        {
            ThemeColors.Purple,
            new[] {
                Color.FromRgb(157, 154, 221),   // #9d9add
                Color.FromRgb(208, 206, 243),   // #d0cef3
                Color.FromRgb(98, 91, 184),     // #625bb8
            }
        },
        {
            ThemeColors.Blue,
            new[] {
                Color.FromRgb(122, 195, 230),   // #7ac3e6
                Color.FromRgb(179, 217, 236),   // #b3d9ec
                Color.FromRgb(17, 149, 221),    // #1195dd
            }
        },
        {
            ThemeColors.Aqua,
            new[] {
                Color.FromRgb(151, 207, 198),   // #97cfc6
                Color.FromRgb(192, 226, 225),   // #c0e2e1
                Color.FromRgb(22, 176, 152),    // #16b098
           }
        },
        {
            ThemeColors.Green,
            new[] {
                Color.FromRgb(198, 214, 125),   // #c6d67d
                Color.FromRgb(227, 235, 198),   // #e3ebc6
                Color.FromRgb(170, 204, 4),     // #aacc04
            }
        }
    };
}
