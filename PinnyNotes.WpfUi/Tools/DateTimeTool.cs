using PinnyNotes.Core.Enums;
using PinnyNotes.WpfUi.Commands;
using PinnyNotes.WpfUi.Controls;
using System.Globalization;

namespace PinnyNotes.WpfUi.Tools;

public class DateTimeTool : BaseTool, ITool
{
    private enum ToolActions
    {
        DateTimeSortableDateTime,
        DateTimeWeekNumber
    }

    public DateTimeTool(NoteTextBoxControl noteTextBox) : base(noteTextBox)
    {
        InitializeMenuItem(
            "Date Time",
            [
                new ToolMenuAction("Sortable Date Time", new RelayCommand(() => MenuAction(ToolActions.DateTimeSortableDateTime))),
                new ToolMenuAction("Week Number", new RelayCommand(() => MenuAction(ToolActions.DateTimeWeekNumber)))
            ]
        );
    }

    public ToolState State => ToolSettings.DateTimeToolState;

    private void MenuAction(ToolActions action)
    {
        switch (action)
        {
            case ToolActions.DateTimeSortableDateTime:
                InsertIntoNoteText(GetSortableDateTime());
                break;
            case ToolActions.DateTimeWeekNumber:
                InsertIntoNoteText(GetWeekNumber());
                break;
        }
    }

    private string GetSortableDateTime()
    {
        string selectedText = NoteTextBox.SelectedText;
        return GetDateTime(selectedText).ToString("s");
    }

    private string GetWeekNumber()
    {
        string selectedText = NoteTextBox.SelectedText;
        return ISOWeek.GetWeekOfYear(GetDateTime(selectedText)).ToString();
    }

    private static DateTime GetDateTime(string? dateString = null)
    {
        if (!string.IsNullOrEmpty(dateString) && DateTime.TryParse(dateString, out DateTime parsedDateTime))
            return parsedDateTime;

        return DateTime.UtcNow;
    }
}
