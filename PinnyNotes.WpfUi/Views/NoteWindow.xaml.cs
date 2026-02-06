using Microsoft.Win32;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;

using PinnyNotes.Core.Enums;
using PinnyNotes.WpfUi.Helpers;
using PinnyNotes.WpfUi.Messages;
using PinnyNotes.WpfUi.Models;
using PinnyNotes.WpfUi.Services;
using PinnyNotes.WpfUi.Themes;
using PinnyNotes.WpfUi.ViewModels;

namespace PinnyNotes.WpfUi.Views;

public partial class NoteWindow : Window
{
    private readonly NoteSettingsModel _noteSettings;
    private readonly MessengerService _messengerService;

    private readonly NoteViewModel _viewModel;

    #region NoteWindow

    public NoteWindow(SettingsService settingsService, MessengerService messengerService, NoteViewModel viewModel)
    {
        _noteSettings = settingsService.NoteSettings;
        _messengerService = messengerService;
        _messengerService.Subscribe<WindowActionMessage>(OnWindowActionMessage);
        _viewModel = viewModel;

        DataContext = _viewModel;

        InitializeComponent();

        Activated += Window_Activated;
        Closing += Window_Closing;
        Deactivated += Window_Deactivated;
        MouseDown += NoteWindow_MouseDown;
        MouseEnter += Window_MouseEnter;
        MouseLeave += Window_MouseLeave;
        Loaded += Window_Loaded;
        StateChanged += NoteWindow_StateChanged;

        TitleBarGrid.MouseDown += TitleBar_MouseDown;
        NewButton.Click += NewButton_Click;
        CloseButton.Click += CloseButton_Click;

        PopulateTitleBarContextMenu();
    }

    private void PopulateTitleBarContextMenu()
    {
        int insertIndex = TitleBarContextMenu.Items.IndexOf(ThemeMenuSeparator);
        foreach (ColorScheme colorScheme in _viewModel.AvailableThemes[0].ColorSchemes.Values)
        {
            MenuItem menuItem = new()
            {
                Header = colorScheme.Name,
                Command = _viewModel.ChangeThemeColorCommand,
                CommandParameter = colorScheme.Name,
                Icon = colorScheme.Icon
            };

            TitleBarContextMenu.Items.Insert(insertIndex, menuItem);

            insertIndex++;
        }
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        _viewModel.OnWindowLoaded(
            ScreenHelper.GetWindowHandle(this)
        );
    }

    private void NoteWindow_MouseDown(object sender, MouseButtonEventArgs e)
    {
        // Check mouse button is pressed as a missed click of a button
        // can cause issues with DragMove().
        if (e.LeftButton != MouseButtonState.Pressed)
            return;

        DragMove();

        _viewModel.OnWindowMoved(Left, Top);
    }

    private void NoteWindow_StateChanged(object? sender, EventArgs e)
    {
        if (WindowState != WindowState.Minimized)
            return;

        if (_noteSettings.MinimizeMode == MinimizeModes.Prevent || (_noteSettings.MinimizeMode == MinimizeModes.PreventIfPinned && _viewModel.Note.IsPinned))
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

    private void Window_Activated(object? sender, EventArgs e)
    {
        _viewModel.Note.IsFocused = true;
        _viewModel.UpdateOpacity();
        _viewModel.UpdateAlwaysOnTop();
        ShowTitleBar();
    }

    private void Window_Deactivated(object? sender, EventArgs e)
    {
        _viewModel.Note.IsFocused = false;
        _viewModel.UpdateOpacity();
        _viewModel.UpdateAlwaysOnTop();
        HideTitleBar();
    }

    private void Window_Closing(object? sender, CancelEventArgs e)
    {
        if (_viewModel.Note.IsSaved || NoteTextBox.Text == "")
            return;

        MessageBoxResult messageBoxResult = MessageBox.Show(
            this,
            "Do you want to save this note?",
            "Pinny Notes",
            MessageBoxButton.YesNoCancel,
            MessageBoxImage.Question
        );

        if (messageBoxResult != MessageBoxResult.Yes)
        {
            e.Cancel = (messageBoxResult == MessageBoxResult.Cancel);
            return;
        }

        MessageBoxResult saveDialogResult = SaveNote();

        e.Cancel = (saveDialogResult == MessageBoxResult.Cancel);
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

        if (saveFileDialog.ShowDialog(this) == false)
            return MessageBoxResult.Cancel;

        File.WriteAllText(saveFileDialog.FileName, NoteTextBox.Text);

        _viewModel.Note.IsSaved = true;

        return MessageBoxResult.OK;
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
        _messengerService.Publish(
            new OpenNoteWindowMessage(ParentNote: _viewModel.Note)
        );
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void HideTitleBar()
    {
        if (_noteSettings.HideTitleBar)
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
        Width = _noteSettings.DefaultWidth;
        Height = _noteSettings.DefaultHeight;
    }

    private void SettingsMenuItem_Click(object sender, RoutedEventArgs e)
    {
        _messengerService.Publish(new OpenSettingsWindowMessage(this));
    }

    #endregion
}
