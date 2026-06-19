using Microsoft.Win32;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Animation;

using PinnyNotes.Core.Enums;
using PinnyNotes.WpfUi.Controls.TitleBar;
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
    private readonly ThemeService _themeService;

    private readonly NoteViewModel _viewModel;

    #region NoteWindow

    public NoteWindow(SettingsService settingsService, MessengerService messengerService, ThemeService themeService, NoteViewModel viewModel)
    {
        _noteSettings = settingsService.NoteSettings;
        _messengerService = messengerService;
        _messengerService.Subscribe<WindowActionMessage>(OnWindowActionMessage);
        _themeService = themeService;

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

        TitleBarPanel.MouseDown += TitleBar_MouseDown;

        InitializeTitleBar();
    }

    private void InitializeTitleBar()
    {
        GridLength buttonColumnWidth = new(40, GridUnitType.Pixel);
        GridLength spacerColumnWidth = new(1, GridUnitType.Star);

        foreach (NoteTitleBarItem titleBarItem in _viewModel.NoteSettings.TitleBarItems)
        {
            switch (titleBarItem)
            {
                case NoteTitleBarItem.Spacer:
                    TitleBarPanel.Children.Add(new TitleBarSpacer());
                    break;
                case NoteTitleBarItem.NewNoteButton:

                    NewNoteButton newNoteButton = new();
                    newNoteButton.Click += NewNoteButton_Click;
                    newNoteButton.SetBinding(NewNoteButton.IconStrokeProperty, new Binding("Note.TitleGridButtonForeground") { Source = _viewModel });

                    TitleBarPanel.Children.Add(newNoteButton);

                    break;
                case NoteTitleBarItem.PinButton:

                    PinButton pinButton = new()
                    {
                        IsChecked = _viewModel.Note.IsPinned
                    };
                    pinButton.SetBinding(PinButton.IconFillProperty, new Binding("Note.TitleGridButtonForeground") { Source = _viewModel });
                    pinButton.SetBinding(PinButton.IsCheckedProperty, new Binding("Note.IsPinned") { Source = _viewModel, Mode = BindingMode.TwoWay });

                    TitleBarPanel.Children.Add(pinButton);

                    break;
                case NoteTitleBarItem.CloseButton:

                    CloseButton closeButton = new();
                    closeButton.Click += CloseButton_Click;
                    closeButton.SetBinding(CloseButton.IconStrokeProperty, new Binding("Note.TitleGridButtonForeground") { Source = _viewModel });

                    TitleBarPanel.Children.Add(closeButton);

                    break;
            }
        }

        // Context menu
        foreach (ColourScheme colourScheme in _themeService.CurrentTheme.ColourSchemes.Values)
        {
            MenuItem menuItem = new()
            {
                Header = colourScheme.Name,
                Command = _viewModel.ChangeThemeColourCommand,
                CommandParameter = colourScheme.Name,
                Icon = colourScheme.Icon
            };

            NoteColourMenuItem.Items.Add(menuItem);
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

        if (_noteSettings.MinimizeMode == MinimizeMode.Prevent || (_noteSettings.MinimizeMode == MinimizeMode.PreventIfPinned && _viewModel.Note.IsPinned))
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
        ShowTitleBar();
    }

    private async void Window_Deactivated(object? sender, EventArgs e)
    {
        if (!_viewModel.Note.IsOpen)
            return;

        _viewModel.Note.IsFocused = false;
        _viewModel.UpdateOpacity();
        HideTitleBar();

        await _viewModel.SaveNote();
    }

    private async void Window_Closing(object? sender, CancelEventArgs e)
    {
        e.Cancel = await _viewModel.CloseNote();
    }

    private void OnWindowActionMessage(WindowActionMessage message)
    {
        if (message.Action == WindowAction.Activate)
        {
            WindowState = WindowState.Normal;
            Activate();
        }
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

    private void NewNoteButton_Click(object sender, RoutedEventArgs e)
    {
        _messengerService.Publish(
            new OpenNoteWindowMessage(ParentNote: _viewModel.Note)
        );
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        _viewModel.Note.IsOpen = false;
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
        SaveFileDialog saveFileDialog = new()
        {
            Filter = "Text Documents (*.txt)|*.txt|All Files|*"
        };

        if (saveFileDialog.ShowDialog(this) == false)
            return;

        File.WriteAllText(saveFileDialog.FileName, NoteTextBox.Text);
    }

    private void ResetMenuItem_Click(object sender, RoutedEventArgs e)
    {
        Width = _noteSettings.DefaultWidth;
        Height = _noteSettings.DefaultHeight;
    }

    private void ManagementMenuItem_Click(object sender, RoutedEventArgs e)
    {
        _messengerService.Publish(new OpenManagementWindowMessage());
    }

    private void SettingsMenuItem_Click(object sender, RoutedEventArgs e)
    {
        _messengerService.Publish(new OpenSettingsWindowMessage(this));
    }

    #endregion
}
