﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Media;

using PinnyNotes.WpfUi.Models;

namespace PinnyNotes.WpfUi.Helpers;

public static class ThemeHelper
{
    public static readonly List<ThemeModel> Themes = [
        // Key, Display Name, MenuIcon
        // Light: TitleBar, TitleBarButtons, Background, Text, Border
        // Dark: ...
        new(
            "yellow", "Yellow", "#fef7b1",
            new ThemeColorModel("#fef7b1", "#464646", "#fffcdd", "#000", "#feea00"),
            new ThemeColorModel("#464646", "#feea00", "#323232", "#fff", "#feea00")
        ),
        new(
            "orange", "Orange", "#ffd179",
            new ThemeColorModel("#ffd179", "#464646", "#fee8b9", "#000", "#ffab00"),
            new ThemeColorModel("#464646", "#ffab00", "#323232", "#fff", "#ffab00")
        ),
        new(
            "red", "Red", "#ff7c81",
            new ThemeColorModel("#ff7c81", "#464646", "#ffc4c6", "#000", "#e33036"),
            new ThemeColorModel("#464646", "#e33036", "#323232", "#fff", "#e33036")
        ),
        new(
            "pink", "Pink", "#d986cc",
            new ThemeColorModel("#d986cc", "#464646", "#ebbfe3", "#000", "#a72995"),
            new ThemeColorModel("#464646", "#a72995", "#323232", "#fff", "#a72995")
        ),
        new(
            "purple", "Purple", "#9d9add",
            new ThemeColorModel("#9d9add", "#464646", "#d0cef3", "#000", "#625bb8"),
            new ThemeColorModel("#464646", "#625bb8", "#323232", "#fff", "#625bb8")
        ),
        new(
            "blue", "Blue", "#7ac3e6",
            new ThemeColorModel("#7ac3e6", "#464646", "#b3d9ec", "#000", "#1195dd"),
            new ThemeColorModel("#464646", "#1195dd", "#323232", "#fff", "#1195dd")
        ),
        new(
            "aqua", "Aqua", "#97cfc6",
            new ThemeColorModel("#97cfc6", "#464646", "#c0e2e1", "#000", "#16b098"),
            new ThemeColorModel("#464646", "#16b098", "#323232", "#fff", "#16b098")
        ),
        new(
            "green", "Green", "#c6d67d",
            new ThemeColorModel("#c6d67d", "#464646", "#e3ebc6", "#000", "#aacc04"),
            new ThemeColorModel("#464646", "#aacc04", "#323232", "#fff", "#aacc04")
        )
    ];

    public static Color HexToColor(string hexColorNotation)
    {
        if (string.IsNullOrWhiteSpace(hexColorNotation) || !hexColorNotation.StartsWith("#") ||
            (hexColorNotation.Length != 4 && hexColorNotation.Length != 7))
        {
            throw new ArgumentException("Invalid Hex value. It must be in the format #RGB or #RRGGBB.");
        }

        string redValue;
        string greenValue;
        string blueValue;
        if (hexColorNotation.Length == 4)
        {
            redValue = new string(hexColorNotation[1], 2);
            greenValue = new string(hexColorNotation[2], 2);
            blueValue = new string(hexColorNotation[3], 2);
        }
        else
        {
            redValue = hexColorNotation.Substring(1, 2);
            greenValue = hexColorNotation.Substring(3, 2);
            blueValue = hexColorNotation.Substring(5, 2);
        }

        return Color.FromRgb(
            byte.Parse(redValue, NumberStyles.HexNumber),
            byte.Parse(greenValue, NumberStyles.HexNumber),
            byte.Parse(blueValue, NumberStyles.HexNumber)
        );
    }

    public static string ColorToHex(Color color)
    {
        return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
    }

    public static ThemeModel GetThemeOrDefault(string key)
    {
        return Themes.Find(t => t.Key == key)
            ?? Themes[0];
    }

    public static ThemeModel GetNextTheme(string currentKey, string? parentKey = null)
    {
        int nextIndex = (Themes.FindIndex(t => t.Key == currentKey) + 1) % Themes.Count;

        if (parentKey != null && nextIndex == Themes.FindIndex(t => t.Key == parentKey))
            nextIndex = (nextIndex + 1) % Themes.Count;

        return Themes[nextIndex];
    }

    public static KeyValuePair<string, string>[] GetDefaultColorList()
    {
        List<KeyValuePair<string, string>> defaultColorList = [];

        defaultColorList.Add(new KeyValuePair<string, string>("#!CYCLE", "Cycle colors"));

        foreach (ThemeModel theme in Themes)
            defaultColorList.Add(new KeyValuePair<string, string>(theme.Key, theme.DisplayName));

        return defaultColorList.ToArray();
    }
}
