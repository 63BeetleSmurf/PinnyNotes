using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using PinnyNotes.WpfUi.Commands;

using PinnyNotes.WpfUi.Controls.ContextMenus;
using PinnyNotes.WpfUi.Enums;

namespace PinnyNotes.WpfUi.Controls;

public partial class NoteTextBoxControl : TextBox
{

    public RelayCommand CopyCommand;
    public RelayCommand CutCommand;
    public RelayCommand PasteCommand;
    public RelayCommand ClearCommand;

    private NoteTextBoxContextMenu _contextMenu;

    public NoteTextBoxControl() : base()
    {
        AcceptsReturn = true;
        AcceptsTab = true;
        AllowDrop = true;
        VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
        HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;

        TextChanged += OnTextChanged;
        SelectionChanged += OnSelectionChanged;
        DragOver += OnDragOver;
        Drop += OnDrop;
        MouseDoubleClick += OnMouseDoubleClick;
        MouseDown += OnMouseDown;
        MouseUp += OnMouseUp;
        PreviewKeyDown += OnPreviewKeyDown;
        ContextMenuOpening += OnContextMenuOpening;

        CopyCommand = new(Copy);
        CutCommand = new(Cut);
        PasteCommand = new(Paste);
        ClearCommand = new(Clear);

        CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, OnCopyExecuted));
        CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, OnCutExecuted));
        CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, OnPasteExecuted));

        InputBindings.Add(new InputBinding(CopyCommand, new KeyGesture(Key.C, ModifierKeys.Control)));
        InputBindings.Add(new InputBinding(CutCommand, new KeyGesture(Key.X, ModifierKeys.Control)));
        InputBindings.Add(new InputBinding(PasteCommand, new KeyGesture(Key.V, ModifierKeys.Control)));

        _contextMenu = new NoteTextBoxContextMenu(this);
        ContextMenu = _contextMenu;
    }

    public static readonly DependencyProperty AutoCopyProperty = DependencyProperty.Register(nameof(AutoCopy), typeof(bool), typeof(NoteTextBoxControl));
    public bool AutoCopy
    {
        get => (bool)GetValue(AutoCopyProperty);
        set => SetValue(AutoCopyProperty, value);
    }
    public static readonly DependencyProperty AutoIndentProperty = DependencyProperty.Register(nameof(AutoIndent), typeof(bool), typeof(NoteTextBoxControl));
    public bool AutoIndent
    {
        get => (bool)GetValue(AutoIndentProperty);
        set => SetValue(AutoIndentProperty, value);
    }

    public static readonly DependencyProperty ConvertIndentationProperty = DependencyProperty.Register(nameof(ConvertIndentation), typeof(bool), typeof(NoteTextBoxControl));
    public bool ConvertIndentation
    {
        get => (bool)GetValue(ConvertIndentationProperty);
        set => SetValue(ConvertIndentationProperty, value);
    }

    public static readonly DependencyProperty CopyFallbackActionProperty = DependencyProperty.Register(nameof(CopyFallbackAction), typeof(CopyFallbackActions), typeof(NoteTextBoxControl));
    public CopyFallbackActions CopyFallbackAction
    {
        get => (CopyFallbackActions)GetValue(CopyFallbackActionProperty);
        set => SetValue(CopyFallbackActionProperty, value);
    }

    public static readonly DependencyProperty KeepNewLineAtEndVisibleProperty = DependencyProperty.Register(nameof(KeepNewLineAtEndVisible), typeof(bool), typeof(NoteTextBoxControl));
    public bool KeepNewLineAtEndVisible
    {
        get => (bool)GetValue(KeepNewLineAtEndVisibleProperty);
        set => SetValue(KeepNewLineAtEndVisibleProperty, value);
    }

    public static readonly DependencyProperty MiddleClickPasteProperty = DependencyProperty.Register(nameof(MiddleClickPaste), typeof(bool), typeof(NoteTextBoxControl));
    public bool MiddleClickPaste
    {
        get => (bool)GetValue(MiddleClickPasteProperty);
        set => SetValue(MiddleClickPasteProperty, value);
    }

    public static readonly DependencyProperty NewLineAtEndProperty = DependencyProperty.Register(nameof(NewLineAtEnd), typeof(bool), typeof(NoteTextBoxControl));
    public bool NewLineAtEnd
    {
        get => (bool)GetValue(NewLineAtEndProperty);
        set => SetValue(NewLineAtEndProperty, value);
    }

    public static readonly DependencyProperty TabSpacesProperty = DependencyProperty.Register(nameof(TabSpaces), typeof(bool), typeof(NoteTextBoxControl));
    public bool TabSpaces
    {
        get => (bool)GetValue(TabSpacesProperty);
        set => SetValue(TabSpacesProperty, value);
    }

    public static readonly DependencyProperty TabWidthProperty = DependencyProperty.Register(nameof(TabWidth), typeof(int), typeof(NoteTextBoxControl));
    public int TabWidth
    {
        get => (int)GetValue(TabWidthProperty);
        set => SetValue(TabWidthProperty, value);
    }

    public static readonly DependencyProperty TrimCopiedTextProperty = DependencyProperty.Register(nameof(TrimCopiedText), typeof(bool), typeof(NoteTextBoxControl));
    public bool TrimCopiedText
    {
        get => (bool)GetValue(TrimCopiedTextProperty);
        set => SetValue(TrimCopiedTextProperty, value);
    }

    public static readonly DependencyProperty TrimPastedTextProperty = DependencyProperty.Register(nameof(TrimPastedText), typeof(bool), typeof(NoteTextBoxControl));
    public bool TrimPastedText
    {
        get => (bool)GetValue(TrimPastedTextProperty);
        set => SetValue(TrimPastedTextProperty, value);
    }

    public new int LineCount()
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

    public int WordCount()
    {
        string text = (SelectionLength > 0) ? SelectedText : Text;
        if (text.Length == 0)
            return 0;
        return text.Split((char[])[' ', '\t', '\r', '\n'], StringSplitOptions.RemoveEmptyEntries).Length;
    }

    public int CharCount()
    {
        string text = (SelectionLength > 0) ? SelectedText : Text;
        if (text.Length == 0)
            return 0;
        return text.Length - text.Count(c => c == '\n' || c == '\r'); // Substract new lines from count.
    }

    private void OnCopyExecuted(object sender, ExecutedRoutedEventArgs e)
        => Copy();
    private void OnCutExecuted(object sender, ExecutedRoutedEventArgs e)
        => Cut();
    private void OnPasteExecuted(object sender, ExecutedRoutedEventArgs e)
        => Paste();

    private new void Copy()
    {
        string copiedText;

        if (SelectionLength == 0)
        {
            switch (CopyFallbackAction)
            {
                case CopyFallbackActions.CopyLine:
                    copiedText = GetLineText(GetLineIndexFromCharacterIndex(CaretIndex));
                    break;
                case CopyFallbackActions.CopyNote:
                    copiedText = Text;
                    break;
                default:
                    return;
            }
        }
        else
        {
            copiedText = SelectedText;
        }

        if (TrimCopiedText)
            copiedText = copiedText.Trim();
        Clipboard.SetDataObject(copiedText);
    }

    private new void Cut()
    {
        Copy();
        SelectedText = string.Empty;
    }

    private new void Paste()
    {
        // Do nothing if clipboard does not contain text.
        if (!Clipboard.ContainsText())
            return;

        string? clipboardString = null;
        try
        {
            // Get text from clipboard and trim if specified
            clipboardString = Clipboard.GetText();
            if (TrimPastedText)
                clipboardString = clipboardString.Trim();
        }
        catch
        {
            return;
        }

        if (string.IsNullOrEmpty(clipboardString))
            return;

        // Replace tabs/spaces if specified
        if (ConvertIndentation)
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
        if (!hasSelectedText && KeepNewLineAtEndVisible && caretAtEnd)
            ScrollToEnd();
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (!NewLineAtEnd || Text.Length == 0 || Text.EndsWith(Environment.NewLine))
            return;

        bool caretAtEnd = (CaretIndex == Text.Length);
        // Preserving selection when adding new line
        int selectionStart = SelectionStart;
        int selectionLength = SelectionLength;

        AppendText(Environment.NewLine);

        SelectionStart = selectionStart;
        SelectionLength = selectionLength;

        if (KeepNewLineAtEndVisible && caretAtEnd)
            ScrollToEnd();
    }

    private void OnSelectionChanged(object sender, RoutedEventArgs e)
    {
        if (AutoCopy)
            Copy();
    }


    private void OnDragOver(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            e.Effects = DragDropEffects.Copy;
            e.Handled = true;
        }
    }

    private void OnDrop(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            Text = File.ReadAllText(
                ((string[])e.Data.GetData(DataFormats.FileDrop))[0]
            );
        }
    }

    private void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (SelectionLength > 0
            && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
        )
            Copy();
    }

    private void OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        // Triple click to select line, quadruple click to select entire wrapped line
        if (e.ClickCount < 3 || e.ClickCount > 4)
            return;

        TextBox textBox = (TextBox)sender;

        int lineIndex = textBox.GetLineIndexFromCharacterIndex(textBox.CaretIndex);
        int lineLength = textBox.GetLineLength(lineIndex);

        // If there was no new line and is not the last row, line must be wrapped.
        if (e.ClickCount == 4 && lineIndex < textBox.LineCount - 1 && !textBox.GetLineText(lineIndex).EndsWith(Environment.NewLine))
        {
            // Expand length until new line or last line found
            int nextLineIndex = lineIndex;
            do
            {
                nextLineIndex++;
                lineLength += textBox.GetLineLength(nextLineIndex);
            } while (nextLineIndex < textBox.LineCount - 1 && !textBox.GetLineText(nextLineIndex).EndsWith(Environment.NewLine));
        }

        textBox.SelectionStart = textBox.GetCharacterIndexFromLineIndex(lineIndex);
        textBox.SelectionLength = lineLength;

        // Don't select new line char(s)
        if (textBox.SelectedText.EndsWith(Environment.NewLine))
            textBox.SelectionLength -= Environment.NewLine.Length;

        OnMouseDoubleClick(sender, e);
    }

    private void OnMouseUp(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Middle && MiddleClickPaste)
            Paste();
    }

    private void OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Tab)
        {
            e.Handled = HandledTabPressed();
        }
        else if (e.Key == Key.Return && AutoIndent)
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

            e.Handled = true;
        }
    }

    private void OnContextMenuOpening(object sender, ContextMenuEventArgs e)
    {
        _contextMenu.Update();
    }

    private bool HandledTabPressed()
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
            int lineCount = LineCount();
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
}
