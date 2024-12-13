using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

using PinnyNotes.WpfUi.Helpers;
using PinnyNotes.WpfUi.Views.ContextMenus;
using PinnyNotes.WpfUi.Tools;
using System.Collections.Generic;

namespace PinnyNotes.WpfUi.Views;

public partial class NoteWindow : Window
{
    public NoteWindow()
    {
        InitializeComponent();
    }

    public nint Handle { get; set; }
    public bool PinButtonState
    {
        get => PinButton.IsChecked ?? false;
        set => PinButton.IsChecked = value;
    }
    public bool HideTitleBar { get; set; }

    public NoteTitleBarContextMenu TitleBarContextMenu { get; set; } = null!;

    public bool UseMonoFont
    {
        get => NoteTextBox.UseMonoFont;
        set => NoteTextBox.UseMonoFont = value;
    }
    public string? MonoFontFamily
    {
        get => NoteTextBox.MonoFontFamily;
        set => NoteTextBox.MonoFontFamily = value;
    }
    public bool SpellCheck{
        get => NoteTextBox.SpellCheck.IsEnabled;
        set => NoteTextBox.SpellCheck.IsEnabled = value;
    }
    public bool AutoIndent{
        get => NoteTextBox.AutoIndent;
        set => NoteTextBox.AutoIndent = value;
    }
    public bool NewLineAtEnd{
        get => NoteTextBox.NewLineAtEnd;
        set => NoteTextBox.NewLineAtEnd = value;
    }
    public bool KeepNewLineVisible{
        get => NoteTextBox.KeepNewLineVisible;
        set => NoteTextBox.KeepNewLineVisible = value;
    }
    public bool TabSpaces{
        get => NoteTextBox.TabSpaces;
        set => NoteTextBox.TabSpaces = value;
    }
    public bool ConvertTabs{
        get => NoteTextBox.ConvertTabs;
        set => NoteTextBox.ConvertTabs = value;
    }
    public int TabWidth{
        get => NoteTextBox.TabWidth;
        set => NoteTextBox.TabWidth = value;
    }
    public bool MiddleClickPaste{
        get => NoteTextBox.MiddleClickPaste;
        set => NoteTextBox.MiddleClickPaste = value;
    }
    public bool TrimPastedText{
        get => NoteTextBox.TrimPastedText;
        set => NoteTextBox.TrimPastedText = value;
    }
    public bool TrimCopiedText{
        get => NoteTextBox.TrimCopiedText;
        set => NoteTextBox.TrimCopiedText = value;
    }
    public bool AutoCopy{
        get => NoteTextBox.AutoCopy;
        set => NoteTextBox.AutoCopy = value;
    }
    public string Text
    {
        get => NoteTextBox.Text;
        set => NoteTextBox.Text = value;
    }
    public int CaretIndex {
        get => NoteTextBox.CaretIndex;
        set => NoteTextBox.CaretIndex = value;
    }
    public int SelectionStart
    {
        get => NoteTextBox.SelectionStart;
        set => NoteTextBox.SelectionStart = value;
    }
    public int SelectionLength
    {
        get => NoteTextBox.SelectionLength;
        set => NoteTextBox.SelectionLength = value;
    }

    public List<ITool> Tools
    {
        get => NoteTextBox.Tools;
    }

    public Brush BorderColorBrush {
        get => BorderBrush;
        set => BorderBrush = value;
    }
    public Brush TitleBarColorBrush {
        get => TitleBarGrid.Background;
        set => TitleBarGrid.Background = value;
    }
    public Brush TitleButtonColorBrush {
        get => (Brush)Resources["TitleButtonBrush"];
        set => Resources["TitleButtonBrush"] = value;
    }
    public Brush BackgroundColorBrush {
        get => Background;
        set => Background = value;
    }
    public Brush TextColorBrush {
        get => (Brush)Resources["NoteFontBrush"];
        set => Resources["NoteFontBrush"] = value;
    }

    public event EventHandler? WindowLoaded;
    public event EventHandler? WindowMoved;
    public event EventHandler? WindowActivated;
    public event EventHandler? WindowDeactivated;
    public event EventHandler? WindowStateChanged;

    public event EventHandler? NewNoteClicked;
    public event EventHandler? PinClicked;
    public event EventHandler? CloseNoteClicked;
    public event EventHandler? TitleBarRightClicked;

    public event EventHandler? TextChanged;

    public void ScrollToEnd() => NoteTextBox.ScrollToEnd();

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        // Ensure handle is set before invoking event
        Handle = ScreenHelper.GetWindowHandle(this);
        WindowLoaded?.Invoke(sender, e);
    }

    private void NoteWindow_MouseDown(object sender, MouseButtonEventArgs e)
    {
        // Check mouse button is pressed as a missed click of a button
        // can cause issues with DragMove().
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            DragMove();
            WindowMoved?.Invoke(sender, e);
        }
    }

    private void NoteWindow_StateChanged(object sender, EventArgs e) => WindowStateChanged?.Invoke(sender, e);

    private void Window_MouseEnter(object sender, MouseEventArgs e)
    {
        ToggleTitleBar();
    }

    private void Window_MouseLeave(object sender, MouseEventArgs e)
    {
        if (!IsActive)
            ToggleTitleBar(true);
    }

    private void Window_Activated(object sender, EventArgs e)
    {
        WindowActivated?.Invoke(sender, e);

        ToggleTitleBar();
    }

    private void Window_Deactivated(object sender, EventArgs e)
    {
        WindowDeactivated?.Invoke(sender, e);

        ToggleTitleBar(true);
    }

    // Will probably make a custom window as dark mode does not work in messagebox.
    // Will actully not need this once database is added
    private void Window_Closing(object sender, CancelEventArgs e)
    {
        //if (!_viewModel.IsSaved && NoteTextBox.Text != "")
        //{
        //    MessageBoxResult messageBoxResult = MessageBox.Show(
        //        this,
        //        "Do you want to save this note?",
        //        "Pinny Notes",
        //        MessageBoxButton.YesNoCancel,
        //        MessageBoxImage.Question
        //    );
        //    // If the user presses cancel on the message box or 
        //    // save dialog, do not close.
        //    if (
        //        (messageBoxResult == MessageBoxResult.Yes && SaveNote() == MessageBoxResult.Cancel)
        //        || messageBoxResult == MessageBoxResult.Cancel
        //    )
        //        e.Cancel = true;
        //}
    }

    // This will become Export note once database added
    private MessageBoxResult SaveNote()
    {
        SaveFileDialog saveFileDialog = new()
        {
            Filter = "Text Documents (*.txt)|*.txt|All Files|*"
        };
        if (saveFileDialog.ShowDialog(this) == true)
        {
            File.WriteAllText(saveFileDialog.FileName, NoteTextBox.Text);
            //_viewModel.IsSaved = true; // IsSaved will move to mean i the database
            return MessageBoxResult.OK;
        }
        return MessageBoxResult.Cancel;
    }

    private void ToggleTitleBar(bool hidden = false)
    {
        if (hidden && HideTitleBar)
            ((Storyboard)FindResource("HideTitleBarAnimation")).Begin();
        else
            ((Storyboard)FindResource("ShowTitleBarAnimation")).Begin();
    }

    private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount >= 2)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }
    }

    private void TitleBar_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
    {
        TitleBarRightClicked?.Invoke(sender, e);

        TitleBarGrid.ContextMenu = TitleBarContextMenu;
    }

    private void NewButton_Click(object sender, RoutedEventArgs e) => NewNoteClicked?.Invoke(sender, e);
    private void PinButton_Click(object sender, RoutedEventArgs e) => PinClicked?.Invoke(sender, e);
    private void CloseButton_Click(object sender, RoutedEventArgs e) => CloseNoteClicked?.Invoke(sender, e);

    private void NoteTextBox_TextChanged(object sender, TextChangedEventArgs e) => TextChanged?.Invoke(sender, e);
}
