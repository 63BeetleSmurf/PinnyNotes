using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Helpers;
using PinnyNotes.WpfUi.Models;
using PinnyNotes.WpfUi.Services;
using PinnyNotes.WpfUi.Views;

namespace PinnyNotes.WpfUi.Presenters;

public class NotePresenter
{
    private readonly NoteService _noteService;
    private readonly NoteModel _model;
    private readonly NoteWindow _view;

    private readonly Timer _typingTimer;

    public NotePresenter(NoteService noteService, NoteModel model, NoteWindow view)
    {
        _noteService = noteService;

        _view = view;
        _model = model;

        _typingTimer = new(2000)
        {
            AutoReset = false
        };

        PopulateViewProperties();

        _noteService.SettingsChanged += OnSettingsChanged;
        _noteService.ActivateNotes += OnActivateNotes;

        _view.Loaded += OnWindowLoaded;
        _view.Closed += OnWindowClosed;
        _view.Activated += OnWindowActivated;
        _view.Deactivated += OnWindowDeactivated;
        _view.StateChanged += OnWindowStateChanged;
        _view.WindowMoved += OnWindowMoved;

        _view.TitleBarGrid.MouseRightButtonUp += OnTitleBarMouseRightClick;
        _view.NewButton.Click += OnNewButtonClick;
        _view.PinButton.Click += OnPinButtonClick;
        _view.CloseButton.Click += OnCloseButtonClick;

        _view.NoteTextBox.TextChanged += OnNoteTextChanged;

        _typingTimer.Elapsed += OnTypingStopped;

        ApplyTheme();
        UpdateWindowOpacity();

        _view.Show();
    }

    public int NoteId => _model.Id;

    public void ShowWindow()
    {
        _view.Show();
        _view.Activate();
    }

    private void OnSettingsChanged(object? sender, EventArgs e)
    {
        PopulateViewProperties();
        UpdateWindowOpacity();
        ApplyTheme();
    }

    private void OnActivateNotes(object? sender, EventArgs e)
    {
        _view.Activate();
    }

    private void OnWindowLoaded(object? sender, EventArgs e)
    {
        _model.WindowHandle = _view.Handle; // May not be needed, see gravity update
    }

    private void OnWindowClosed(object? sender, EventArgs e)
    {
        _typingTimer.Stop();
    }

    private void OnWindowActivated(object? sender, EventArgs e)
    {
        UpdateWindowOpacity();
    }

    private void OnWindowDeactivated(object? sender, EventArgs e)
    {
        UpdateWindowOpacity();
    }

    private void OnWindowStateChanged(object? sender, EventArgs e)
    {
        if (_view.WindowState == WindowState.Minimized
            && (
                _model.Settings.Notes_MinimizeMode == MinimizeModes.Prevent
                || (_model.Settings.Notes_MinimizeMode == MinimizeModes.PreventIfPinned && _view.Topmost)
            )
        )
            _view.WindowState = WindowState.Normal;
    }

    private void OnWindowMoved(object? sender, EventArgs e)
    {
        // Reset gravity depending what position the note was moved to.
        // This does not effect the saved start up setting, only what
        // direction new child notes will go towards.
        _model.X = (int)_view.Left;
        _model.Y = (int)_view.Top;
        _model.UpdateGravity(
            ScreenHelper.GetCurrentScreenBounds(_view.Handle)
        );
    }

    private void OnTitleBarMouseRightClick(object? sender, EventArgs e)
    {
        _view.TitleBarContextMenu = new(
            _model.Theme,
            OnSaveMenuItemClicked,
            OnResetSizeMenuItemClicked,
            OnChangeThemeMenuItemClicked,
            OnSettingsMenuItemClicked
        );
    }

    private void OnResetSizeMenuItemClicked(object? sender, EventArgs e)
    {
        _model.Width = _model.Settings.Notes_DefaultWidth;
        _model.Height = _model.Settings.Notes_DefaultHeight;
        _view.Width = _model.Width;
        _view.Height = _model.Height;
    }

    private void OnChangeThemeMenuItemClicked(object? sender, EventArgs e)
    {
        if (sender is not MenuItem menuItem)
            return;

        ThemeModel? theme = ThemeHelper.Themes.Find(t => t.Key == (string)menuItem.Tag);
        if (theme == null)
            return;

        _model.Theme = theme;
        ApplyTheme();

        _noteService.SaveNote(_model);
    }

    private void OnSettingsMenuItemClicked(object? sender, EventArgs e)
    {
        _noteService.OpenSettingsWindowOnNote(_view);
    }

    private void OnNewButtonClick(object? sender, EventArgs e)
    {
        _noteService.OpenNewNote(_model);
    }

    private void OnPinButtonClick(object? sender, EventArgs e)
    {
        _model.IsPinned = _view.IsPinned;
        _view.Topmost = _model.IsPinned;
        UpdateWindowOpacity();
    }

    private void OnCloseButtonClick(object? sender, EventArgs e)
    {
        _noteService.CloseNote(_model, _view);
    }

    private void OnNoteTextChanged(object? sender, EventArgs e)
    {
        _typingTimer.Stop();
        _typingTimer.Start();

        _model.Text = _view.Text;
    }

    private void OnTypingStopped(object? sender, EventArgs e)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            _noteService.SaveNote(_model);
        });
    }

    private void OnSaveMenuItemClicked(object? sender, EventArgs e)
    {
        
    }

    private void PopulateViewProperties()
    {
        _view.Width = _model.Width;
        _view.Height = _model.Height;
        _view.Left = _model.X;
        _view.Top = _model.Y;
        _view.ShowInTaskbar = _model.Settings.Application_NotesInTaskbar;
        _view.NoteTextBox.Text = _model.Text;

        PropertiesHelper.CopyMatchingProperties(_model.Settings, _view);

    }

    private void UpdateWindowOpacity()
    {
        bool isTransparent = (
            _model.Settings.Notes_TransparencyMode != TransparencyModes.Disabled
            && (_model.Settings.Notes_TransparencyMode != TransparencyModes.OnlyWhenPinned || _model.IsPinned)
            && !(_model.Settings.Notes_OpaqueWhenFocused && _view.IsActive)
        );

        _view.Opacity = (isTransparent) ? _model.Settings.Notes_TransparentOpacity : _model.Settings.Notes_OpaqueOpacity;
    }

    private void ApplyTheme()
    {
        _view.TitleBarColorBrush = _model.ThemeColors.TitleBarColor.Brush;
        _view.TitleButtonColorBrush = _model.ThemeColors.TitleBarButtonsColor.Brush;
        _view.BackgroundColorBrush = _model.ThemeColors.BackgroundColor.Brush;
        _view.TextColorBrush = _model.ThemeColors.TextColor.Brush;
        _view.BorderColorBrush = _model.ThemeColors.BorderColor.Brush;
    }
}
