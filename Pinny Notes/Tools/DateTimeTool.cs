using System;

using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Views.Controls;

namespace PinnyNotes.WpfUi.Tools;

public partial class DateTimeTool : BaseTool, ITool
{
    public ToolStates State { get; }

    public enum ToolActions
    {
        DateTimeSortableDateTime
    }

    public DateTimeTool(NoteTextBoxControl noteTextBox, ToolStates state) : base(noteTextBox)
    {
        State = state;
        _name = "Date Time";
        _menuActions.Add(new("Sortable Date Time", DateTimeSortableDateTimeMenuAction));
    }

    private void DateTimeSortableDateTimeMenuAction(object sender, EventArgs e) => InsertIntoNoteText(GetSortableDateTime());

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
