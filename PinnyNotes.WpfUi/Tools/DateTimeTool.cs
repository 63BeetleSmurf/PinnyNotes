using System;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Commands;
using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Properties;

namespace PinnyNotes.WpfUi.Tools;

public class DateTimeTool : BaseTool, ITool
{
    public ToolStates State => (ToolStates)Settings.Default.DateTimeToolState;

    public enum ToolActions
    {
        DateTimeSortableDateTime
    }

    public DateTimeTool(TextBox noteTextBox) : base(noteTextBox)
    {
        _name = "Date Time";
        _menuActions.Add(new("Sortable Date Time", new RelayCommand(() => MenuAction(ToolActions.DateTimeSortableDateTime))));
    }

    private void MenuAction(ToolActions action)
    {
        switch (action)
        {
            case ToolActions.DateTimeSortableDateTime:
                InsertIntoNoteText(GetSortableDateTime());
                break;
        }
    }

    private string GetSortableDateTime()
    {
        string selectedText = _noteTextBox.SelectedText;
        return GetDateTime("s", selectedText);
    }

    private string GetDateTime(string format, string? dateString = null)
    {
        if (string.IsNullOrEmpty(dateString))
            return DateTime.UtcNow.ToString(format);

        DateTime parsedDateTime;
        if (DateTime.TryParse(dateString, out parsedDateTime))
            return parsedDateTime.ToString(format);

        return "";
    }
}
