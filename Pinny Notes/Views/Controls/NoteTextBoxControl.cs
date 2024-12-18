using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Views.ContextMenus;

namespace PinnyNotes.WpfUi.Views.Controls;

public class NoteTextBoxControl : TextBox
{
    public NoteTextBoxControl()
    {
        AcceptsReturn = true;
        AcceptsTab = true;
        AllowDrop = true;
        TextWrapping = TextWrapping.Wrap;
        VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

        // This ensures custom menu is used, without it default context menu sometimes appears.
        ContextMenu = new();

        CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, CopyEventHandler));
        CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, CutEventHandler));
        CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, PasteEventHandler));
    }

    public string? MonoFontFamily { get; set; }
    private bool _useMonoFont;
    public bool UseMonoFont {
        get => _useMonoFont;
        set
        {
            _useMonoFont = value;
            if (_useMonoFont && !string.IsNullOrWhiteSpace(MonoFontFamily))
                FontFamily = new FontFamily(MonoFontFamily);
            else
                FontFamily = new FontFamily();
        }
    }
    public bool AutoIndent { get; set; }
    public bool NewLineAtEnd { get; set; }
    public bool KeepNewLineVisible { get; set; }
    public bool TabSpaces { get; set; }
    public bool ConvertTabs { get; set; }
    public int TabWidth { get; set; }
    public bool MiddleClickPaste { get; set; }
    public bool TrimPastedText { get; set; }
    public bool TrimCopiedText { get; set; }
    public bool AutoCopy { get; set; }

    public ToolStates Tool_Base64State { get; set; }
    public ToolStates Tool_BracketState { get; set; }
    public ToolStates Tool_CaseState { get; set; }
    public ToolStates Tool_DateTimeState { get; set; }
    public ToolStates Tool_GibberishState { get; set; }
    public ToolStates Tool_HashState { get; set; }
    public ToolStates Tool_HtmlEntityState { get; set; }
    public ToolStates Tool_IndentState { get; set; }
    public ToolStates Tool_JoinState { get; set; }
    public ToolStates Tool_JsonState { get; set; }
    public ToolStates Tool_ListState { get; set; }
    public ToolStates Tool_QuoteState { get; set; }
    public ToolStates Tool_RemoveState { get; set; }
    public ToolStates Tool_SlashState { get; set; }
    public ToolStates Tool_SortState { get; set; }
    public ToolStates Tool_SplitState { get; set; }
    public ToolStates Tool_TrimState { get; set; }

    public new void Copy()
    {
        CopyToClipboard();
    }

    public new void Cut()
    {
        if (CopyToClipboard())
            SelectedText = "";
    }

    public new void Paste()
    {
        // Do nothing if clipboard does not contain text.
        if (!Clipboard.ContainsText())
            return;

        // Get text from clipboard and trim if specified
        string clipboardString;
        try
        {
            clipboardString = Clipboard.GetText();
        }
        catch
        {
            // If there are any issues getting text, just ignore it.
            return;
        }
        if (TrimPastedText)
            clipboardString = clipboardString.Trim();

        // Replace tabs/spaces if specified
        if (ConvertTabs)
        {
            string spaces = "".PadLeft(TabWidth, ' ');

            if (TabSpaces)
                clipboardString = clipboardString.Replace("\t", spaces);
            else
                clipboardString = clipboardString.Replace(spaces, "\t");
        }

        bool hasSelectedText = (SelectionLength > 0);
        int caretIndex = (hasSelectedText) ? SelectionStart : CaretIndex;
        bool caretAtEnd = (caretIndex == Text.Length);

        SelectedText = clipboardString;
        CaretIndex = caretIndex + clipboardString.Length;

        if (!hasSelectedText)
            CheckNewLineAtEndVisible(caretAtEnd);
    }

    public int GetLineCount()
    {
        int count;
        if (SelectionLength > 0)
            count = GetLineIndexFromCharacterIndex(SelectionStart + SelectionLength)
                - GetLineIndexFromCharacterIndex(SelectionStart)
                + 1;
        else
            count = GetLineIndexFromCharacterIndex(Text.Length) + ((NewLineAtEnd) ? 0 : 1);
        return count;
    }

    public int GetWordCount()
    {
        string text = (SelectionLength > 0) ? SelectedText : Text;
        if (text.Length == 0)
            return 0;
        return text.Split((char[])[' ', '\t', '\r', '\n'], StringSplitOptions.RemoveEmptyEntries).Length;
    }

    public int GetCharCount()
    {
        string text = (SelectionLength > 0) ? SelectedText : Text;
        if (text.Length == 0)
            return 0;
        return text.Length - text.Count(c => c == '\n' || c == '\r'); // Substract new lines from count.
    }

    private void CopyEventHandler(object sender, ExecutedRoutedEventArgs e)
        => Copy();

    private void CutEventHandler(object sender, ExecutedRoutedEventArgs e)
        => Cut();

    private void PasteEventHandler(object sender, ExecutedRoutedEventArgs e)
        => Paste();

    protected override void OnTextChanged(TextChangedEventArgs e)
    {
        base.OnTextChanged(e);

        if (NewLineAtEnd && Text != "" && !Text.EndsWith(Environment.NewLine))
            AddNewLineAtEnd();
    }

    protected override void OnSelectionChanged(RoutedEventArgs e)
    {
        base.OnSelectionChanged(e);

        if (AutoCopy && SelectionLength > 0)
            Copy();

    }

    protected override void OnDragOver(DragEventArgs e)
    {
        base.OnDrop(e);

        if (e.Data.GetDataPresent(DataFormats.FileDrop))
            e.Effects = DragDropEffects.Copy;
    }

    protected override void OnDrop(DragEventArgs e)
    {
        base.OnDrop(e);

        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            Text = File.ReadAllText(
                ((string[])e.Data.GetData(DataFormats.FileDrop))[0]
            );
        }

    }

    protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
    {
        base.OnMouseDoubleClick(e);

        if (SelectionLength > 0 && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            Copy();
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        base.OnMouseDown(e);

        // Triple click to select line, quadruple click to select entire wrapped line
        if (e.ClickCount < 3 || e.ClickCount > 4)
            return;

        int lineIndex = GetLineIndexFromCharacterIndex(CaretIndex);
        int lineLength = GetLineLength(lineIndex);

        // If there was no new line and is not the last row, line must be wrapped.
        if (e.ClickCount == 4 && lineIndex < LineCount - 1 && !GetLineText(lineIndex).EndsWith(Environment.NewLine))
        {
            // Expand length until new line or last line found
            int nextLineIndex = lineIndex;
            do
            {
                nextLineIndex++;
                lineLength += GetLineLength(nextLineIndex);
            } while (nextLineIndex < LineCount - 1 && !GetLineText(nextLineIndex).EndsWith(Environment.NewLine));
        }

        SelectionStart = GetCharacterIndexFromLineIndex(lineIndex);
        SelectionLength = lineLength;

        // Don't select new line char(s)
        if (SelectedText.EndsWith(Environment.NewLine))
            SelectionLength -= Environment.NewLine.Length;

        OnMouseDoubleClick(e);
    }

    protected override void OnMouseUp(MouseButtonEventArgs e)
    {
        base.OnMouseUp(e);

        if (e.ChangedButton == MouseButton.Middle && MiddleClickPaste)
            Paste();
    }

    protected override void OnPreviewKeyDown(KeyEventArgs e)
    {
        base.OnPreviewKeyDown(e);

        if (e.Key == Key.Tab)
            e.Handled = HandleTabPressed();
        else if (e.Key == Key.Return && AutoIndent)
            e.Handled = HandleReturnPressed();
    }

    protected override void OnContextMenuOpening(ContextMenuEventArgs e)
    {
        base.OnContextMenuOpening(e);

        ContextMenu = new NoteTextBoxContextMenu(this);
    }

    private void AddNewLineAtEnd()
    {
        bool caretAtEnd = (CaretIndex == Text.Length);
        // Preserving selection when adding new line
        int selectionStart = SelectionStart;
        int selectionLength = SelectionLength;

        AppendText(Environment.NewLine);

        SelectionStart = selectionStart;
        SelectionLength = selectionLength;

        CheckNewLineAtEndVisible(caretAtEnd);
    }

    private void CheckNewLineAtEndVisible(bool caretAtEnd)
    {
        if (KeepNewLineVisible && caretAtEnd)
            ScrollToEnd();
    }

    private bool HandleTabPressed()
    {
        if ((SelectionLength == 0 || !SelectedText.Contains(Environment.NewLine)) && Keyboard.Modifiers != ModifierKeys.Shift && TabSpaces)
        {
            int spaceCount = TabWidth;
            int caretIndex = (SelectionLength == 0) ? CaretIndex : SelectionStart;

            int lineStart = GetCharacterIndexFromLineIndex(
                GetLineIndexFromCharacterIndex(caretIndex)
            );
            if (lineStart != caretIndex)
            {
                int lineCaretIndex = caretIndex - lineStart;
                int tabWidth = lineCaretIndex % spaceCount;
                if (tabWidth > 0)
                    spaceCount = spaceCount - tabWidth;
            }
            string spaces = "".PadLeft(spaceCount, ' ');

            if (SelectionLength == 0)
            {
                Text = Text.Insert(caretIndex, spaces);
                CaretIndex = caretIndex + spaceCount;
            }
            else
            {
                SelectedText = spaces;
                SelectionLength = 0;
                CaretIndex = caretIndex + spaceCount;
            }
            return true;
        }
        else if (SelectionLength == 0 && Keyboard.Modifiers == ModifierKeys.Shift)
        {
            int caretIndex = CaretIndex;
            if (caretIndex > 0)
            {
                int tabLength = 0;
                int charIndex = caretIndex - 1;

                if (Text[charIndex] == '\t')
                {
                    tabLength = 1;
                }
                else if (Text[charIndex] == ' ')
                {
                    while (charIndex >= 0 && Text[charIndex] == ' ' && tabLength < TabWidth)
                    {
                        tabLength++;
                        charIndex--;
                    }
                }

                if (tabLength > 0)
                {
                    Text = Text.Remove(caretIndex - tabLength, tabLength);
                    CaretIndex = caretIndex - tabLength;
                }
            }
            return true;
        }
        else if (SelectionLength > 0 && SelectedText.Contains(Environment.NewLine))
        {
            int selectionStart = SelectionStart;
            int selectionEnd = SelectionStart + SelectionLength;

            // Fix the selection so full lines, not wrapped lines, are selected
            // Find the starting line by making sure the previous line ends with a new line
            int startLineIndex = GetLineIndexFromCharacterIndex(selectionStart);
            while (startLineIndex > 0 && !GetLineText(startLineIndex - 1).Contains(Environment.NewLine))
                startLineIndex--;
            selectionStart = GetCharacterIndexFromLineIndex(startLineIndex);

            // Find the end line by making sure it ends with a new line
            int endLineIndex = GetLineIndexFromCharacterIndex(selectionEnd);
            int lineCount = GetLineCount();
            while (endLineIndex < lineCount - 1 && !GetLineText(endLineIndex).Contains(Environment.NewLine))
                endLineIndex++;
            selectionEnd = GetCharacterIndexFromLineIndex(endLineIndex) + GetLineLength(endLineIndex) - Environment.NewLine.Length;

            // Select the full lines so we can now easily get the required selected text
            Select(selectionStart, selectionEnd - selectionStart);

            // Loop though each line adding or removing the indentation where required
            string indentation = (TabSpaces) ? "".PadLeft(TabWidth, ' ') : "\t";
            string[] lines = SelectedText.Split(Environment.NewLine);
            for (int i = 0; i < lines.Length; i++)
            {
                if (Keyboard.Modifiers == ModifierKeys.Shift)
                {
                    if (lines[i].Length > 0)
                    {
                        if (lines[i][0] == '\t')
                            lines[i] = lines[i].Remove(0, 1);
                        else if (lines[i][0] == ' ')
                        {
                            int concurrentSpaces = 0;
                            foreach (char character in lines[i])
                            {
                                if (character != ' ')
                                    break;
                                concurrentSpaces++;
                            }

                            switch (concurrentSpaces)
                            {
                                case >= 4:
                                    lines[i] = lines[i].Remove(0, 4);
                                    break;
                                case 1:
                                    lines[i] = lines[i].Remove(0, concurrentSpaces);
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    lines[i] = $"{indentation}{lines[i]}";
                }
            }

            SelectedText = string.Join(Environment.NewLine, lines);

            return true;
        }

        return false;
    }

    private bool HandleReturnPressed()
    {
        // If there is selected text remove it and set caret to correct position
        if (SelectionLength > 0)
        {
            int selectionStart = SelectionStart;
            Text = Text.Remove(selectionStart, SelectionLength);
            CaretIndex = selectionStart;
        }

        // Store caret position for positioning later
        int caretIndex = CaretIndex;

        // Get the current line of text, trimming any new lines
        string line = GetLineText(GetLineIndexFromCharacterIndex(caretIndex)).TrimEnd(Environment.NewLine.ToCharArray());

        // Get the whitespace from the beginning of the line and create our indent string
        string precedingWhitespace = new(line.TakeWhile(char.IsWhiteSpace).ToArray());
        string indent = Environment.NewLine + precedingWhitespace;

        // Add the indent and restore caret position
        Text = Text.Insert(caretIndex, indent);
        CaretIndex = caretIndex + indent.Length;

        return true;
    }

    private bool CopyToClipboard()
    {
        if (SelectionLength == 0)
            return false;

        string copiedText = SelectedText;

        if (TrimCopiedText)
            copiedText = copiedText.Trim();

        Clipboard.SetDataObject(copiedText);

        return true;
    }
}
