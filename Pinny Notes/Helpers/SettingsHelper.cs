using System.Collections.Generic;

using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Models;

namespace PinnyNotes.WpfUi.Helpers;

public static class SettingsHelper
{
    public static readonly IEnumerable<ComboBoxItemModel<StartupPositions>> StartupPositionsList = [
        new(StartupPositions.TopLeft, "Top Left"),
        new(StartupPositions.TopCenter, "Top Center"),
        new(StartupPositions.TopRight, "Top Right"),
        new(StartupPositions.MiddleLeft, "Middle Left"),
        new(StartupPositions.MiddleCenter, "Middle Center"),
        new(StartupPositions.MiddleRight, "Middle Right"),
        new(StartupPositions.BottomLeft, "Bottom Left"),
        new(StartupPositions.BottomCenter, "Bottom Center"),
        new(StartupPositions.BottomRight, "Bottom Right")
    ];

    public static readonly IEnumerable<ComboBoxItemModel<MinimizeModes>> MinimizeModeList = [
        new(MinimizeModes.Allow, "Yes"),
        new(MinimizeModes.Prevent, "No"),
        new(MinimizeModes.PreventIfPinned, "When not pinned")
    ];

    public static readonly IEnumerable<ComboBoxItemModel<string>> DefaultThemeColorList = ThemeHelper.GetDefaultThemeColorList();

    public static readonly IEnumerable<ComboBoxItemModel<ColorModes>> ColorModeList = [
        new(ColorModes.Light, "Light"),
        new(ColorModes.Dark, "Dark"),
        new(ColorModes.System, "System Default")
    ];

    public static readonly IEnumerable<ComboBoxItemModel<TransparencyModes>> TransparencyModeList = [
        new(TransparencyModes.Disabled, "Disabled"),
        new(TransparencyModes.Enabled, "Enabled"),
        new(TransparencyModes.OnlyWhenPinned, "Only when pinned")
    ];

    public static readonly IEnumerable<ComboBoxItemModel<ToolStates>> ToolStateList = [
        new(ToolStates.Enabled, "Enabled"),
        new(ToolStates.Disabled, "Disabled"),
        new(ToolStates.Favorite, "Favorite")
    ];
    public static IEnumerable<ComboBoxItemModel<ToolStates>> GetToolStateList() => ToolStateList;
}
