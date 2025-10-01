using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using PinnyNotes.Core.Enums;
using PinnyNotes.WpfUi.Helpers;
using PinnyNotes.WpfUi.Messages;
using PinnyNotes.WpfUi.Services;
using PinnyNotes.WpfUi.ViewModels;
using PinnyNotes.WpfUi.Themes;

namespace PinnyNotes.WpfUi.Views;

public partial class NoteWindow : Window
{
    private readonly SettingsService _settingsService;
    private readonly MessengerService _messengerService;

    private readonly NoteViewModel _viewModel;

    #region NoteWindow

    public NoteWindow(SettingsService settingsService, MessengerService messengerService, NoteViewModel viewModel)
    {
        _settingsService = settingsService;
        _messengerService = messengerService;
        _messengerService.Subscribe<WindowActionMessage>(OnWindowActionMessage);
        _viewModel = viewModel;

        DataContext = _viewModel;

        InitializeComponent();

        PopulateTitleBarContextMenu();
    }

    private void PopulateTitleBarContextMenu()
    {
        int insertIndex = TitleBarContextMenu.Items.IndexOf(ThemeMenuSeparator);
        foreach (Theme theme in _viewModel.AvailableThemes)
        {
            TitleBarContextMenu.Items.Insert(
                insertIndex,
                new MenuItem()
                {
                    Header = theme.Name,
                    Command = _viewModel.ChangeThemeColorCommand,
                    CommandParameter = theme.ThemeColor,
                    Icon = new Rectangle()
                    {
                        Width = 16,
                        Height = 16,
                        Fill = theme.MenuIcon.Brush
                    }
                }
            );
            insertIndex++;
        }
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is NoteViewModel viewModel)
            viewModel.OnWindowLoaded(
                ScreenHelper.GetWindowHandle(this)
            );
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

            Rect screenBounds = ScreenHelper.GetCurrentScreenBounds(_viewModel.WindowHandle);
            _viewModel.GravityX = (Left - screenBounds.X < screenBounds.Width / 2) ? 1 : -1;
            _viewModel.GravityY = (Top - screenBounds.Y < screenBounds.Height / 2) ? 1 : -1;
        }
    }

    private void NoteWindow_StateChanged(object sender, EventArgs e)
    {
        MinimizeModes minimizeMode = _settingsService.NoteSettings.MinimizeMode;

        if (WindowState == WindowState.Minimized
            && (
                minimizeMode == MinimizeModes.Prevent 
                || (minimizeMode == MinimizeModes.PreventIfPinned && _viewModel.IsPinned)
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
        _viewModel.IsFocused = true;
        _viewModel.UpdateOpacity();
        _viewModel.UpdateAlwaysOnTop();
        ShowTitleBar();
    }

    private void Window_Deactivated(object sender, EventArgs e)
    {
        _viewModel.IsFocused = false;
        _viewModel.UpdateOpacity();
        _viewModel.UpdateAlwaysOnTop();
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

    private void OnWindowActionMessage(WindowActionMessage message)
    {
        if (message.Action == WindowActions.Activate)
        {
            WindowState = WindowState.Normal;
            Activate();
        }
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
        _messengerService.Publish(new CreateNewNoteMessage(_viewModel));
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void HideTitleBar()
    {
        if (_settingsService.NoteSettings.HideTitleBar)
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

    private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
    {
        SaveNote();
    }

    private void ResetMenuItem_Click(object sender, RoutedEventArgs e)
    {
        Width = _settingsService.NoteSettings.DefaultWidth;
        Height = _settingsService.NoteSettings.DefaultHeight;
    }

    private void SettingsMenuItem_Click(object sender, RoutedEventArgs e)
    {
        _messengerService.Publish(new OpenSettingsWindowMessage(this));
    }

    #endregion

    #region TextBox

    private void NoteTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        _viewModel.IsSaved = false;
    }

    #endregion
}
