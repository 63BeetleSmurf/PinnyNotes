using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Helpers;
using PinnyNotes.WpfUi.Views.Controls.ContextMenus;

namespace PinnyNotes.WpfUi.Views;

public partial class NoteWindow : Window
{
    public NoteWindow()
    {
        InitializeComponent();

        Loaded += OnWindowLoaded;
        Closing += OnWindowClosing;
        Activated += OnWindowActivated;
        Deactivated += OnWindowDeactivated;
        StateChanged += OnWindowStateChanged;
        MouseEnter += OnWindowMouseEnter;
        MouseLeave += OnWindowMouseLeave;
        MouseDown += OnWindowMouseDown;

        TitleBarGrid.MouseDown += OnTitleBarMouseDown;
        TitleBarGrid.MouseRightButtonUp += OnTitleBarMouseRightButtonUp;
        NewButton.Click += OnNewButtonClick;
        PinButton.Click += OnPinButtonClick;
        CloseButton.Click += OnCloseButtonClick;

        NoteTextBox.TextChanged += OnNoteTextChanged;
    }

    public nint Handle { get; set; }
    public bool IsPinned { get => PinButton.IsChecked ?? false; set => PinButton.IsChecked = value;}

    public bool Notes_HideTitleBar { get; set; }

    public NoteTitleBarContextMenu TitleBarContextMenu { get; set; } = null!;

    public bool Editor_UseMonoFont { get => NoteTextBox.UseMonoFont; set => NoteTextBox.UseMonoFont = value; }
    public string? Editor_MonoFontFamily { get => NoteTextBox.MonoFontFamily; set => NoteTextBox.MonoFontFamily = value; }
    public bool Editor_SpellCheck { get => NoteTextBox.SpellCheck.IsEnabled; set => NoteTextBox.SpellCheck.IsEnabled = value; }
    public bool Editor_AutoIndent { get => NoteTextBox.AutoIndent; set => NoteTextBox.AutoIndent = value; }
    public bool Editor_NewLineAtEnd { get => NoteTextBox.NewLineAtEnd; set => NoteTextBox.NewLineAtEnd = value; }
    public bool Editor_KeepNewLineVisible { get => NoteTextBox.KeepNewLineVisible; set => NoteTextBox.KeepNewLineVisible = value; }
    public bool Editor_TabsToSpaces { get => NoteTextBox.TabSpaces; set => NoteTextBox.TabSpaces = value; }
    public bool Editor_ConvertIndentationOnPaste { get => NoteTextBox.ConvertTabs; set => NoteTextBox.ConvertTabs = value; }
    public int Editor_TabWidth { get => NoteTextBox.TabWidth; set => NoteTextBox.TabWidth = value; }
    public bool Editor_MiddleClickPaste { get => NoteTextBox.MiddleClickPaste; set => NoteTextBox.MiddleClickPaste = value; }
    public bool Editor_TrimPastedText { get => NoteTextBox.TrimPastedText; set => NoteTextBox.TrimPastedText = value; }
    public bool Editor_TrimCopiedText { get => NoteTextBox.TrimCopiedText; set => NoteTextBox.TrimCopiedText = value; }
    public bool Editor_CopyHighlightedText { get => NoteTextBox.AutoCopy; set => NoteTextBox.AutoCopy = value; }

    public string Text { get => NoteTextBox.Text; set => NoteTextBox.Text = value; }
    public int CaretIndex { get => NoteTextBox.CaretIndex; set => NoteTextBox.CaretIndex = value; }
    public int SelectionStart { get => NoteTextBox.SelectionStart; set => NoteTextBox.SelectionStart = value; }
    public int SelectionLength { get => NoteTextBox.SelectionLength; set => NoteTextBox.SelectionLength = value; }

    public ToolStates Tool_Base64State { get => NoteTextBox.Tool_Base64State; set => NoteTextBox.Tool_Base64State = value; }
    public ToolStates Tool_BracketState { get => NoteTextBox.Tool_BracketState; set => NoteTextBox.Tool_BracketState = value; }
    public ToolStates Tool_CaseState { get => NoteTextBox.Tool_CaseState; set => NoteTextBox.Tool_CaseState = value; }
    public ToolStates Tool_DateTimeState { get => NoteTextBox.Tool_DateTimeState; set => NoteTextBox.Tool_DateTimeState = value; }
    public ToolStates Tool_GibberishState { get => NoteTextBox.Tool_GibberishState; set => NoteTextBox.Tool_GibberishState = value; }
    public ToolStates Tool_HashState { get => NoteTextBox.Tool_HashState; set => NoteTextBox.Tool_HashState = value; }
    public ToolStates Tool_HtmlEntityState { get => NoteTextBox.Tool_HtmlEntityState; set => NoteTextBox.Tool_HtmlEntityState = value; }
    public ToolStates Tool_IndentState { get => NoteTextBox.Tool_IndentState; set => NoteTextBox.Tool_IndentState = value; }
    public ToolStates Tool_JoinState { get => NoteTextBox.Tool_JoinState; set => NoteTextBox.Tool_JoinState = value; }
    public ToolStates Tool_JsonState { get => NoteTextBox.Tool_JsonState; set => NoteTextBox.Tool_JsonState = value; }
    public ToolStates Tool_ListState { get => NoteTextBox.Tool_ListState; set => NoteTextBox.Tool_ListState = value; }
    public ToolStates Tool_QuoteState { get => NoteTextBox.Tool_QuoteState; set => NoteTextBox.Tool_QuoteState = value; }
    public ToolStates Tool_RemoveState { get => NoteTextBox.Tool_RemoveState; set => NoteTextBox.Tool_RemoveState = value; }
    public ToolStates Tool_SlashState { get => NoteTextBox.Tool_SlashState; set => NoteTextBox.Tool_SlashState = value; }
    public ToolStates Tool_SortState { get => NoteTextBox.Tool_SortState; set => NoteTextBox.Tool_SortState = value; }
    public ToolStates Tool_SplitState { get => NoteTextBox.Tool_SplitState; set => NoteTextBox.Tool_SplitState = value; }
    public ToolStates Tool_TrimState { get => NoteTextBox.Tool_TrimState; set => NoteTextBox.Tool_TrimState = value; }

    public Brush BorderColorBrush { get => BorderBrush; set => BorderBrush = value; }
    public Brush TitleBarColorBrush { get => TitleBarGrid.Background; set => TitleBarGrid.Background = value; }
    public Brush TitleButtonColorBrush { get => (Brush)Resources["TitleButtonBrush"]; set => Resources["TitleButtonBrush"] = value; }
    public Brush BackgroundColorBrush { get => Background; set => Background = value; }
    public Brush TextColorBrush { get => (Brush)Resources["NoteFontBrush"]; set => Resources["NoteFontBrush"] = value; }

    public event EventHandler? WindowLoaded;
    public event EventHandler? WindowClosing;
    public event EventHandler? WindowActivated;
    public event EventHandler? WindowDeactivated;
    public event EventHandler? WindowStateChanged;
    public event EventHandler? WindowMoved;

    public event EventHandler? TitleBarMouseRightClick;
    public event EventHandler? NewButtonClick;
    public event EventHandler? PinButtonClick;
    public event EventHandler? CloseButtonClick;

    public event EventHandler? NoteTextChanged;

    public void ScrollToEnd() => NoteTextBox.ScrollToEnd();

    private void OnWindowLoaded(object sender, RoutedEventArgs e)
    {
        // Ensure handle is set before invoking event
        Handle = ScreenHelper.GetWindowHandle(this);
        WindowLoaded?.Invoke(sender, e);
    }

    private void OnWindowClosing(object? sender, CancelEventArgs e)
    {
        WindowClosing?.Invoke(sender, e);
    }

    private void OnWindowActivated(object? sender, EventArgs e)
    {
        WindowActivated?.Invoke(sender, e);

        ToggleTitleBar();
    }

    private void OnWindowDeactivated(object? sender, EventArgs e)
    {
        WindowDeactivated?.Invoke(sender, e);

        ToggleTitleBar(true);
    }

    private void OnWindowStateChanged(object? sender, EventArgs e)
    {
        WindowStateChanged?.Invoke(sender, e);
    }

    private void OnWindowMouseEnter(object sender, MouseEventArgs e)
    {
        ToggleTitleBar();
    }

    private void OnWindowMouseLeave(object sender, MouseEventArgs e)
    {
        if (!IsActive)
            ToggleTitleBar(true);
    }

    private void OnWindowMouseDown(object sender, MouseButtonEventArgs e)
    {
        // Check mouse button is pressed as a missed click of a button
        // can cause issues with DragMove().
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            DragMove();
            WindowMoved?.Invoke(sender, e);
        }
    }

    private void OnTitleBarMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount >= 2)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }
    }

    private void OnTitleBarMouseRightButtonUp(object sender, MouseButtonEventArgs e)
    {
        TitleBarMouseRightClick?.Invoke(sender, e);

        TitleBarGrid.ContextMenu = TitleBarContextMenu;
    }

    private void OnNewButtonClick(object sender, RoutedEventArgs e)
    {
        NewButtonClick?.Invoke(sender, e);
    }

    private void OnPinButtonClick(object sender, RoutedEventArgs e)
    {
        PinButtonClick?.Invoke(sender, e);
    }

    private void OnCloseButtonClick(object sender, RoutedEventArgs e)
    {
        CloseButtonClick?.Invoke(sender, e);
    }

    private void OnNoteTextChanged(object sender, TextChangedEventArgs e)
    {
        NoteTextChanged?.Invoke(sender, e);
    }

    private void ToggleTitleBar(bool hide = false)
    {
        if (hide && Notes_HideTitleBar)
            ((Storyboard)FindResource("HideTitleBarAnimation")).Begin();
        else
            ((Storyboard)FindResource("ShowTitleBarAnimation")).Begin();
    }
}
