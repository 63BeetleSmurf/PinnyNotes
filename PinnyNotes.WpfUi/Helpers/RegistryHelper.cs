using Microsoft.Win32;

namespace PinnyNotes.WpfUi.Helpers;

public static class RegistryHelper
{
    public static bool IsDarkMode()
    {
        using RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
        if (key is null)
            return false;

        object? value = key.GetValue("AppsUseLightTheme");

        return value is int i && i == 0;
    }
}
