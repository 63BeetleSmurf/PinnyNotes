using Microsoft.Win32;
using System.Reflection;

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

    public static void AddToStartup()
    {
        using RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", writable: true);

        string exePath = Environment.ProcessPath ?? Assembly.GetExecutingAssembly().Location;

        key?.SetValue("PinnyNotes", $"\"{exePath}\"");
    }

    public static void RemoveFromStartup()
    {
        using RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", writable: true);

        if (key?.GetValue("PinnyNotes") != null)
        {
            key.DeleteValue("PinnyNotes");
        }
    }
}
