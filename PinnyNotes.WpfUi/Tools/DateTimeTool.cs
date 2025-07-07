using System;

using PinnyNotes.Core.Enums;
using PinnyNotes.WpfUi.Commands;
using PinnyNotes.WpfUi.Controls;

namespace PinnyNotes.WpfUi.Tools;

public class DateTimeTool : BaseTool, ITool
{
    private enum ToolActions
    {
        DateTimeSortableDateTime
    }

    public ToolStates State => _settings.AppSettings.DateTimeState;

    public DateTimeTool(NoteTextBoxControl noteTextBox) : base(noteTextBox)
    {
        InitializeMenuItem(
            "Date Time",
            [
                new ToolMenuAction("Sortable Date Time", new RelayCommand(() => MenuAction(ToolActions.DateTimeSortableDateTime)))
            ]
        );
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
