using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Controls;
using PinnyNotes.WpfUi.Services;
using PinnyNotes.WpfUi.Models;

namespace PinnyNotes.WpfUi.Tools;

public abstract class BaseTool
{
    protected ToolSettingsModel ToolSettings;

    protected NoteTextBoxControl NoteTextBox;

    private MenuItem? _menuItem;
    public MenuItem MenuItem => _menuItem
        ?? throw new Exception("Tools menu has not been initialised.");

    public BaseTool(NoteTextBoxControl noteTextBox)
    {
        NoteTextBox = noteTextBox;

        SettingsService settingsService = App.Services.GetRequiredService<SettingsService>();
        ToolSettings = settingsService.ToolSettings;
    }

    protected void InitializeMenuItem(string header, ToolMenuAction[] menuActions)
    {
        _menuItem = new()
        {
            Header = header,
        };

        foreach (ToolMenuAction menuAction in menuActions)
        {
            if (menuAction.Name == "-" && menuAction.Command == null && menuAction.Action == null)
            {
                _menuItem.Items.Add(new Separator());
                continue;
            }

            MenuItem actionMenuItem = new()
            {
                Header = menuAction.Name
            };
            if (menuAction.Command != null)
                actionMenuItem.Command = menuAction.Command;
            if (menuAction.Action != null)
                actionMenuItem.CommandParameter = menuAction.Action;

            _menuItem.Items.Add(actionMenuItem);
        }
    }

    protected void ApplyFunctionToNoteText(Func<string, Enum, string> function, Enum action)
    {
        if (NoteTextBox.HasSelectedText)
        {
            NoteTextBox.SelectedText = function(NoteTextBox.SelectedText, action);
        }
        else
        {
            string noteText = NoteTextBox.Text;
            // Ignore trailing new line if it was automatically added
            if (NoteTextBox.NewLineAtEnd && NoteTextBox.Text.EndsWith(Environment.NewLine))
                noteText = noteText.Remove(noteText.Length - Environment.NewLine.Length);
            NoteTextBox.SelectAll();
            NoteTextBox.SelectedText = function(noteText, action);
            NoteTextBox.SelectionLength = 0;
            if (NoteTextBox.Text.Length > 0)
                NoteTextBox.CaretIndex = NoteTextBox.Text.Length - 1;
        }
    }

    protected void ApplyFunctionToEachLine(Func<string, int, Enum, string?> function, Enum action)
    {
        string noteText = (NoteTextBox.HasSelectedText) ? NoteTextBox.SelectedText : NoteTextBox.Text;

        string[] lines = noteText.Split(Environment.NewLine);
        // Ignore trailing new line if it was automatically added
        if (NoteTextBox.NewLineAtEnd && lines[^1] == "")
            lines = lines[..^1];

        List<string> newLines = [];
        for (int i = 0; i < lines.Length; i++)
        {
            string? line = function(lines[i], i, action);
            if (line != null)
                newLines.Add(line);
        }

        noteText = string.Join(Environment.NewLine, newLines);

        if (NoteTextBox.HasSelectedText)
            NoteTextBox.SelectedText = noteText;
        else
        {
            NoteTextBox.SelectAll();
            NoteTextBox.SelectedText = noteText;
            NoteTextBox.SelectionLength = 0;
            if (NoteTextBox.Text.Length > 0)
                NoteTextBox.CaretIndex = NoteTextBox.Text.Length - 1;
        }
    }

    protected void InsertIntoNoteText(string text)
    {
        int caretIndex = (NoteTextBox.HasSelectedText) ? NoteTextBox.SelectionStart : NoteTextBox.CaretIndex;
        bool caretAtEnd = (caretIndex == NoteTextBox.Text.Length);

        NoteTextBox.SelectedText = text;

        NoteTextBox.CaretIndex = caretIndex + text.Length;
        if (!NoteTextBox.HasSelectedText && NoteTextBox.KeepNewLineAtEndVisible && caretAtEnd)
            NoteTextBox.ScrollToEnd();
    }
}
