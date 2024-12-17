using System;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Helpers;

using PinnyNotes.WpfUi.Views.Controls;

namespace PinnyNotes.WpfUi.Tools;

public static class DateTimeTool
{
    public const string Name = "Date Time";

    public static MenuItem MenuItem
        => ToolHelper.GetToolMenuItem(
            Name,
            [
                new("Sortable Date Time", OnSortableDateTimeMenuItemClick)
            ]
        );

    private static void OnSortableDateTimeMenuItemClick(object sender, EventArgs e)
    {
        NoteTextBoxControl noteTextBox = ToolHelper.GetNoteTextBoxFromSender(sender);
        ToolHelper.InsertIntoNoteText(noteTextBox, GetSortableDateTime(noteTextBox));
    }

    private static string GetSortableDateTime(NoteTextBoxControl noteTextBox)
    {
        string selectedText = noteTextBox.SelectedText;
        return GetDateTime("s", selectedText);
    }

    private static string GetDateTime(string format, string? dateString = null)
    {
        if (string.IsNullOrEmpty(dateString))
            return DateTime.UtcNow.ToString(format);

        DateTime parsedDateTime;
        if (DateTime.TryParse(dateString, out parsedDateTime))
            return parsedDateTime.ToString(format);

        return "";
    }
}
