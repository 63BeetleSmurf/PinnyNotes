using System.Windows;
using System.Windows.Controls;

namespace PinnyNotes.WpfUi.Views.Controls.ContextMenus;

public class ManageNotesNoneContextMenu : ContextMenu
{
    public ManageNotesNoneContextMenu(RoutedEventHandler onCreateNoteClicked, RoutedEventHandler onShowAllClicked, RoutedEventHandler onCloseAllClicked)
    {
        PopulateMenu(
            onCreateNoteClicked,
            onShowAllClicked,
            onCloseAllClicked
        );
    }

    public void PopulateMenu(RoutedEventHandler onCreateNoteClicked, RoutedEventHandler onShowAllClicked, RoutedEventHandler onCloseAllClicked)
    {
        MenuItem newNoteMenuItem = new()
        {
            Header = "New Note"
        };
        newNoteMenuItem.Click += onCreateNoteClicked;
        Items.Add(newNoteMenuItem);


        Items.Add(new Separator());


        MenuItem showAllMenuItem = new()
        {
            Header = "Show All"
        };
        showAllMenuItem.Click += onShowAllClicked;
        Items.Add(showAllMenuItem);


        MenuItem closeAllMenuItem = new()
        {
            Header = "Close All"
        };
        closeAllMenuItem.Click += onCloseAllClicked;
        Items.Add(closeAllMenuItem);
    }
}
