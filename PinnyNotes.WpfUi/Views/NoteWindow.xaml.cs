using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

using PinnyNotes.WpfUi.Commands;
using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Helpers;
using PinnyNotes.WpfUi.Properties;
using PinnyNotes.WpfUi.ViewModels;
using PinnyNotes.WpfUi.Services;

namespace PinnyNotes.WpfUi.Views;

public partial class NoteWindow : Window
{
    private readonly MessengerService _messenger;

    private NoteViewModel _viewModel { get; }

    private RelayCommand _saveCommand = null!;

    private RelayCommand _resetSizeCommand = null!;

    #region NoteWindow

    public NoteWindow(MessengerService messenger) : this(messenger, null) { }
    public NoteWindow(MessengerService messenger, NoteViewModel? parentViewModel = null)
    {
        _messenger = messenger;
        DataContext = _viewModel = new NoteViewModel(_messenger, parentViewModel);

        InitializeComponent();

        _saveCommand = new(SaveCommandExecute);
        SaveMenuItem.Command = _saveCommand;
        _resetSizeCommand = new(ResetSizeCommandExecute);
        ResetSizeMenuItem.Command = _resetSizeCommand;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        _viewModel.WindowHandel = ScreenHelper.GetWindowHandle(this);
    }

    private void NoteWindow_MouseDown(object sender, MouseButtonEventArgs e)
    {
        // Check mouse button is pressed as a missed click of a button
        // can cause issues with DragMove().
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            DragMove();

            // Reset gravity depending what position the note was moved to.
            // This does not effect the saved start up setting, only what
            // direction new child notes will go towards.
            _viewModel.X = Left;
            _viewModel.Y = Top;

            Rectangle screenBounds = ScreenHelper.GetCurrentScreenBounds(_viewModel.WindowHandel);
            _viewModel.GravityX = (Left - screenBounds.X < screenBounds.Width / 2) ? 1 : -1;
            _viewModel.GravityY = (Top - screenBounds.Y < screenBounds.Height / 2) ? 1 : -1;
        }
    }

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
        _viewModel.IsFocused = true;
        _viewModel.UpdateOpacity();
        ShowTitleBar();
    }

    private void Window_Deactivated(object sender, EventArgs e)
    {
        Topmost = _viewModel.IsPinned;
        _viewModel.IsFocused = false;
        _viewModel.UpdateOpacity();
        HideTitleBar();
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
        if (!_viewModel.IsSaved && NoteTextBox.Text != "")
        {
            MessageBoxResult messageBoxResult = MessageBox.Show(
                this,
                "Do you want to save this note?",
                "Pinny Notes",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question
            );
            // If the user presses cancel on the message box or 
            // save dialog, do not close.
            if (
                (messageBoxResult == MessageBoxResult.Yes && SaveNote() == MessageBoxResult.Cancel)
                || messageBoxResult == MessageBoxResult.Cancel
            )
                e.Cancel = true;
        }
    }

    #endregion

    #region Commands

    public void SaveCommandExecute()
    {
        SaveNote();
    }

    public void ResetSizeCommandExecute()
    {
        Width = NoteViewModel.DefaultWidth;
        Height = NoteViewModel.DefaultHeight;
    }

    #endregion

    #region MiscFunctions

    private MessageBoxResult SaveNote()
    {
        SaveFileDialog saveFileDialog = new()
        {
            Filter = "Text Documents (*.txt)|*.txt|All Files|*"
        };
        if (saveFileDialog.ShowDialog(this) == true)
        {
            File.WriteAllText(saveFileDialog.FileName, NoteTextBox.Text);
            _viewModel.IsSaved = true;
            return MessageBoxResult.OK;
        }
        return MessageBoxResult.Cancel;
    }

    private void HideTitleBar()
    {
        if (Settings.Default.HideTitleBar)
            BeginStoryboard("HideTitleBarAnimation");
    }

    private void ShowTitleBar()
    {
        BeginStoryboard("ShowTitleBarAnimation");
    }

    private void BeginStoryboard(string resourceKey)
    {
        Storyboard hideTitleBar = (Storyboard)FindResource(resourceKey);
        hideTitleBar.Begin();
    }

    #endregion

    #region TitleBar

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

    private void NewButton_Click(object sender, RoutedEventArgs e)
    {
        new NoteWindow(_messenger, _viewModel).Show();
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void SettingsMenuItem_Click(object sender, RoutedEventArgs e)
    {
        ((App)Application.Current).ShowSettingsWindow(this);
    }

    #endregion

    #region TextBox

    private void NoteTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        _viewModel.IsSaved = false;
    }

    #endregion
}
