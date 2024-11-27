using System.Windows;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Helpers;
using PinnyNotes.WpfUi.Models;

namespace PinnyNotes.WpfUi.Views.ContextMenus;

public class NoteTitleBarContextMenu : ContextMenu
{
    public NoteTitleBarContextMenu(ThemeModel noteTheme,
        RoutedEventHandler onSaveClicked, RoutedEventHandler onResetSizeClicked, RoutedEventHandler onChangeThemeClicked, RoutedEventHandler onSettingsClicked)
    {
        PopulateMenu(
            noteTheme,
            onSaveClicked,
            onResetSizeClicked,
            onChangeThemeClicked,
            onSettingsClicked
        );
    }

    public void PopulateMenu(ThemeModel noteTheme,
        RoutedEventHandler onSaveClicked, RoutedEventHandler onResetSizeClicked, RoutedEventHandler onChangeThemeClicked, RoutedEventHandler onSettingsClicked)
    {
        MenuItem saveMenuItem = new()
        {
            Header = "Save"
        };
        saveMenuItem.Click += onSaveClicked;
        Items.Add(saveMenuItem);


        Items.Add(new Separator());


        MenuItem resetSizeMenuItem = new()
        {
            Header = "Reset Size"
        };
        resetSizeMenuItem.Click += onResetSizeClicked;
        Items.Add(resetSizeMenuItem);


        Items.Add(new Separator());


        AddColorMenuItems(noteTheme, onChangeThemeClicked);


        Items.Add(new Separator());


        MenuItem settingsMenuItem = new()
        {
            Header = "Settings"
        };
        settingsMenuItem.Click += onSettingsClicked;
        Items.Add(settingsMenuItem);
    }

    private void AddColorMenuItems(ThemeModel noteTheme, RoutedEventHandler onChangeThemeClicked)
    {
        foreach (ThemeModel theme in ThemeHelper.Themes)
        {
            MenuItem menuItem = new()
            {
                Header = theme.Name,
                Icon = theme.MenuIcon,
                IsEnabled = (theme != noteTheme)
            };
            menuItem.Click += onChangeThemeClicked;
            Items.Add(menuItem);
        }
    }
}
