using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Helpers;
using PinnyNotes.WpfUi.Properties;
using PinnyNotes.WpfUi.Views.ContextMenus;

namespace PinnyNotes.WpfUi.Views;

public partial class NoteWindow : Window, INoteView
{
    public NoteWindow()
    {
        InitializeComponent();

        // x
        // y
        // width
        // height
        // opacity
        // showintaskbar
    }

    public nint Handle { get; set; }

    public NoteTitleBarContextMenu TitleBarContextMenu { get; set; } = null!;

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
    public Brush BorderColorBrush { get => BorderBrush; set => BorderBrush = value; }
    public Brush TitleBarColorBrush { get => TitleBarGrid.Background; set => TitleBarGrid.Background = value; }
    public Brush TitleButtonColorBrush { get => (Brush)Resources["TitleButtonBrush"]; set => Resources["TitleButtonBrush"] = value; }
    public Brush BackgroundColorBrush { get => Background; set => Background = value; }
    public Brush TextColorBrush { get => (Brush)Resources["NoteFontBrush"]; set => Resources["NoteFontBrush"] = value; }

    public void ScrollToEnd() => NoteTextBox.ScrollToEnd();

    public event EventHandler? WindowLoaded;
    public event EventHandler? WindowMoved;
    public event EventHandler? WindowActivated;
    public event EventHandler? WindowDeactivated;

    public event EventHandler? NewNoteClicked;
    public event EventHandler? CloseNoteClicked;
    public event EventHandler? TitleBarRightClicked;

    public event EventHandler? TextChanged;

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

    // This is temperamental enough, leave in view for now.
    private void NoteWindow_StateChanged(object sender, EventArgs e)
    {
        MinimizeModes minimizeMode = (MinimizeModes)Settings.Default.MinimizeMode;

        if (WindowState == WindowState.Minimized
            && (
                minimizeMode == MinimizeModes.Prevent 
                || (minimizeMode == MinimizeModes.PreventIfPinned && Topmost)
            )
        )
            WindowState = WindowState.Normal;
    }

    private void Window_MouseEnter(object sender, MouseEventArgs e)
    {
        ShowTitleBar();
    }

    private void Window_MouseLeave(object sender, MouseEventArgs e)
    {
        if (!IsActive)
            HideTitleBar();
    }

    private void Window_Activated(object sender, EventArgs e)
    {
        Topmost = true;

        WindowActivated?.Invoke(sender, e);

        ShowTitleBar();
    }

    private void Window_Deactivated(object sender, EventArgs e)
    {
        WindowDeactivated?.Invoke(sender, e);

        HideTitleBar();
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

    private void HideTitleBar()
    {
        if (Settings.Default.HideTitleBar)
            ((Storyboard)FindResource("HideTitleBarAnimation")).Begin();
    }

    private void ShowTitleBar()
    {
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

    private void CloseButton_Click(object sender, RoutedEventArgs e) => CloseNoteClicked?.Invoke(sender, e);


    private void NoteTextBox_TextChanged(object sender, RoutedEventArgs e) => TextChanged?.Invoke(sender, e);

}
