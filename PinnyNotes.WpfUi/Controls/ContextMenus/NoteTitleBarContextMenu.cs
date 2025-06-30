using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using PinnyNotes.WpfUi.Commands;
using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Helpers;
using PinnyNotes.WpfUi.Models;

namespace PinnyNotes.WpfUi.Controls.ContextMenus;

public class NoteTitleBarContextMenu : ContextMenu
{
    private const string _themeTag = "theme";

    private readonly RelayCommand _saveCommand;
    private readonly RelayCommand _resetSizeCommand;
    private readonly RelayCommand<ThemeColors> _changeThemeColorCommand;
    private readonly RelayCommand _settingsCommand;

    public NoteTitleBarContextMenu(
        RelayCommand saveCommand,
        RelayCommand resetSizeCommand,
        RelayCommand<ThemeColors> changeThemeColorCommand,
        RelayCommand settingsCommand)
    {
        _saveCommand = saveCommand;
        _resetSizeCommand = resetSizeCommand;
        _changeThemeColorCommand = changeThemeColorCommand;
        _settingsCommand = settingsCommand;

        Populate();
    }

    public void Update(ThemeColors currentTheme)
    {
        foreach (MenuItem menuItem in Items)
        {
            if (menuItem.Tag as string == _themeTag)
            {
                if (menuItem.CommandParameter as ThemeColors? == currentTheme)
                    menuItem.IsEnabled = false;
                else
                    menuItem.IsEnabled = true;
            }
        }
    }

    private void Populate()
    {
        Items.Add(
            new MenuItem()
            {
                Header = "Save",
                Command = _saveCommand
            }
        );

        Items.Add(new Separator());

        Items.Add(
            new MenuItem()
            {
                Header = "Reset size",
                Command = _resetSizeCommand
            }
        );

        Items.Add(new Separator());
        foreach (ThemeModel theme in ThemeHelper.Themes.Values)
        {
            Items.Add(
                new MenuItem()
                {
                    Command = _changeThemeColorCommand,
                    CommandParameter = theme.ThemeColor,
                    Header = theme.Name,
                    Icon = new Rectangle()
                    {
                        Fill = new SolidColorBrush(theme.TitleBarColor),
                        Height = 16,
                        Width = 16
                    },
                    Tag = _themeTag
                }
            );
        }

        Items.Add(new Separator());

        Items.Add(
            new MenuItem()
            {
                Header = "Settings",
                Command = _settingsCommand
            }
        );
    }

}
