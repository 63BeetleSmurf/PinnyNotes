using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PinnyNotes.WpfUi.Views.Controls.ContextMenus;

public class ManageNotesSelectionContextMenu : ContextMenu
{
    public ManageNotesSelectionContextMenu(IEnumerable<int> noteIds,
        RoutedEventHandler onOpenClicked, RoutedEventHandler onCloseClicked, RoutedEventHandler onDeleteClicked)
    {
        PopulateMenu(
            noteIds,
            onOpenClicked,
            onCloseClicked,
            onDeleteClicked
        );
    }

    public void PopulateMenu(IEnumerable<int> noteIds,
        RoutedEventHandler onOpenClicked, RoutedEventHandler onCloseClicked, RoutedEventHandler onDeleteClicked)
    {
        MenuItem openMenuItem = new()
        {
            Header = "Open",
            Tag = noteIds
        };
        openMenuItem.Click += onOpenClicked;
        Items.Add(openMenuItem);

        MenuItem closeMenuItem = new()
        {
            Header = "Close",
            Tag = noteIds
        };
        closeMenuItem.Click += onCloseClicked;
        Items.Add(closeMenuItem);


        Items.Add(new Separator());


        MenuItem deleteMenuItem = new()
        {
            Header = "Delete",
            Tag = noteIds
        };
        deleteMenuItem.Click += onDeleteClicked;
        Items.Add(deleteMenuItem);
    }
}
