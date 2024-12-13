﻿using System;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Enums;

namespace PinnyNotes.WpfUi.Tools;

public partial class DateTimeTool : BaseTool, ITool
{
    public ToolStates State => ToolStates.Disabled; // (ToolStates)ToolSettings.Default.DateTimeToolState;

    public enum ToolActions
    {
        DateTimeSortableDateTime
    }

    public DateTimeTool(TextBox noteTextBox) : base(noteTextBox)
    {
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
