using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using PinnyNotes.WpfUi.Properties;
using PinnyNotes.WpfUi.Tools;

namespace PinnyNotes.WpfUi.Views.Controls;

public class NoteTextBox : TextBox
{
    public IEnumerable<ITool> Tools = [];

    public NoteTextBox()
    {
        AcceptsReturn = true;
        AcceptsTab = true;
        AllowDrop = true;
        TextWrapping = TextWrapping.Wrap;
        VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

        //ContextMenu = new(); // May need this, sure there is a reason it was in before

        Tools = [
            new Base64Tool(this),
            new BracketTool(this),
            new CaseTool(this),
            new DateTimeTool(this),
            new GibberishTool(this),
            new HashTool(this),
            new HtmlEntityTool(this),
            new IndentTool(this),
            new JoinTool(this),
            new JsonTool(this),
            new ListTool(this),
            new QuoteTool(this),
            new RemoveTool(this),
            new SlashTool(this),
            new SortTool(this),
            new SplitTool(this),
            new TrimTool(this)
        ];

        CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, CopyEventHandler));
        CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, CutEventHandler));
        CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, PasteEventHandler));
    }

    private void CopyEventHandler(object sender, ExecutedRoutedEventArgs e) => Copy();
    private void CutEventHandler(object sender, ExecutedRoutedEventArgs e) => Cut();
    private void PasteEventHandler(object sender, ExecutedRoutedEventArgs e) => Paste();

    protected override void OnTextChanged(TextChangedEventArgs e)
    {
        base.OnTextChanged(e);

        if (Settings.Default.NewLineAtEnd && Text != "" && !Text.EndsWith(Environment.NewLine))
            AddNewLineAtEnd();
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

        KeepNewLineAtEndVisible(caretAtEnd);
    }

    private void KeepNewLineAtEndVisible(bool caretAtEnd)
    {
        if (Settings.Default.KeepNewLineAtEndVisible && caretAtEnd)
            ScrollToEnd();
    }

    protected override void OnSelectionChanged(RoutedEventArgs e)
    {
        base.OnSelectionChanged(e);

        if (Settings.Default.AutoCopy && SelectionLength > 0)
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

        if (e.ChangedButton == MouseButton.Middle && Settings.Default.MiddleClickPaste)
            Paste();
    }

    protected override void OnPreviewKeyDown(KeyEventArgs e)
    {
        base.OnPreviewKeyDown(e);

        if (e.Key == Key.Tab)
            e.Handled = HandleTabPressed();
        else if (e.Key == Key.Return && Settings.Default.AutoIndent)
            e.Handled = HandleReturnPressed();
    }

    private bool HandleTabPressed()
    {
        if ((SelectionLength == 0 || !SelectedText.Contains(Environment.NewLine)) && Keyboard.Modifiers != ModifierKeys.Shift && Settings.Default.TabSpaces)
        {
            int spaceCount = Settings.Default.TabWidth;
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
                    while (charIndex >= 0 && Text[charIndex] == ' ' && tabLength < Settings.Default.TabWidth)
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
            string indentation = (Settings.Default.TabSpaces) ? "".PadLeft(Settings.Default.TabWidth, ' ') : "\t";
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
        string preceedingWhitespace = new(line.TakeWhile(char.IsWhiteSpace).ToArray());
        string indent = Environment.NewLine + preceedingWhitespace;

        // Add the indent and restore caret position
        Text = Text.Insert(caretIndex, indent);
        CaretIndex = caretIndex + indent.Length;

        return true;
    }

    protected override void OnContextMenuOpening(ContextMenuEventArgs e)
    {
        ContextMenu = new NoteTextBoxContextMenu(this);

        base.OnContextMenuOpening(e);
    }

    public new void Copy()
    {
        CopyToClipboard();
    }

    public new void Cut()
    {
        if (CopyToClipboard())
            SelectedText = "";
    }

    private bool CopyToClipboard()
    {
        if (SelectionLength == 0)
            return false;

        string copiedText = SelectedText;

        if (Settings.Default.TrimCopiedText)
            copiedText = copiedText.Trim();

        Clipboard.SetDataObject(copiedText);

        return true;
    }

    public new void Paste()
    {
        // Do nothing if clipboard does not contain text.
        if (!Clipboard.ContainsText())
            return;

        // Get text from clipboard and trim if specified
        string clipboardString = Clipboard.GetText();
        if (Settings.Default.TrimPastedText)
            clipboardString = clipboardString.Trim();

        // Replace tabs/spaces if specified
        if (Settings.Default.ConvertIndentation)
        {
            string spaces = "".PadLeft(Settings.Default.TabWidth, ' ');

            if (Settings.Default.TabSpaces)
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
            KeepNewLineAtEndVisible(caretAtEnd);
    }

    public int GetLineCount()
    {
        int count;
        if (SelectionLength > 0)
            count = GetLineIndexFromCharacterIndex(SelectionStart + SelectionLength)
                - GetLineIndexFromCharacterIndex(SelectionStart)
                + 1;
        else
            count = GetLineIndexFromCharacterIndex(Text.Length) + ((Settings.Default.NewLineAtEnd) ? 0 : 1);
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

}
