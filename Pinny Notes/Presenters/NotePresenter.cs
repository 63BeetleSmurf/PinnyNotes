using System;
using System.Windows;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Helpers;
using PinnyNotes.WpfUi.Models;
using PinnyNotes.WpfUi.Views;

namespace PinnyNotes.WpfUi.Presenters;

public class NotePresenter
{
    private readonly ApplicationManager _applicationManager;
    private readonly NoteModel _model;
    private readonly NoteWindow _view;

    public NotePresenter(ApplicationManager applicationManager, NoteModel model, NoteWindow view)
    {
        _applicationManager = applicationManager;

        _view = view;
        _model = model;

        PopulateViewProperties();

        _applicationManager.SettingsChanged += OnSettingsChanged;
        _applicationManager.ActivateNotes += OnActivateNotes;

        _view.WindowLoaded += OnWindowLoaded;
        _view.WindowMoved += OnWindowMoved;
        _view.WindowActivated += OnWindowActivated;
        _view.WindowDeactivated += OnWindowDeactivated;
        _view.WindowStateChanged += OnWindowStateChanged;

        _view.NewNoteClicked += OnNewNoteClicked;
        _view.PinClicked += OnPinClicked;
        _view.CloseNoteClicked += OnCloseNoteClicked;
        _view.TitleBarRightClicked += OnTitleBarRightClicked;

        _view.TextChanged += OnTextChanged;

        ApplyTheme();
        UpdateWindowOpacity();
    }

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
        SettingsModel settings = _model.Settings ?? _applicationManager.ApplicationSettings;

        if (_view.WindowState == WindowState.Minimized
            && (
                settings.Notes_MinimizeMode == MinimizeModes.Prevent
                || (settings.Notes_MinimizeMode == MinimizeModes.PreventIfPinned && _view.Topmost)
            )
        )
            _view.WindowState = WindowState.Normal;
    }

    private void OnNewNoteClicked(object? sender, EventArgs e)
    {
        _applicationManager.CreateNewNote(_model);
    }

    private void OnPinClicked(object? sender, EventArgs e)
    {
        _model.IsPinned = _view.IsPinned;
        _view.Topmost = _model.IsPinned;
        UpdateWindowOpacity();
    }

    private void OnCloseNoteClicked(object? sender, EventArgs e)
    {
        _applicationManager.SaveNote(_model);
        _view.Close();
    }

    private void OnTitleBarRightClicked(object? sender, EventArgs e)
    {
        _view.TitleBarContextMenu = new(
            _model.Theme,
            OnSaveMenuItemClicked,
            OnResetSizeMenuItemClicked,
            OnChangeThemeMenuItemClicked,
            OnSettingsMenuItemClicked
        );
    }

    private void OnSaveMenuItemClicked(object? sender, EventArgs e)
    {
        
    }

    private void OnResetSizeMenuItemClicked(object? sender, EventArgs e)
    {
        SettingsModel settings = _model.Settings ?? _applicationManager.ApplicationSettings;

        _model.Width = settings.Notes_DefaultWidth;
        _model.Height = settings.Notes_DefaultHeight;
        _view.Width = _model.Width;
        _view.Height = _model.Height;
    }

    private void OnChangeThemeMenuItemClicked(object? sender, EventArgs e)
    {
        if (sender is MenuItem menuItem)
        {
            ThemeModel? theme = ThemeHelper.Themes.Find(t => t.Key == (string)menuItem.Tag);
            if (theme == null)
                return;

            _model.Theme = theme;
            ApplyTheme();
        }
    }

    private void OnSettingsMenuItemClicked(object? sender, EventArgs e)
    {
        _applicationManager.ShowSettingsWindow(_view);
    }

    private void OnTextChanged(object? sender, EventArgs e)
    {
        _model.Text = _view.Text;
    }

    private void PopulateViewProperties()
    {
        SettingsModel settings = _model.Settings ?? _applicationManager.ApplicationSettings;

        _view.Width = _model.Width;
        _view.Height = _model.Height;
        _view.Left = _model.X;
        _view.Top = _model.Y;
        _view.ShowInTaskbar = settings.Application_NotesInTaskbar;

        PropertiesHelper.CopyMatchingProperties(settings, _view);

    }

    private void UpdateWindowOpacity()
    {
        SettingsModel settings = _model.Settings ?? _applicationManager.ApplicationSettings;

        bool isTransparent = (
            settings.Notes_TransparencyMode != TransparencyModes.Disabled
            && (settings.Notes_TransparencyMode != TransparencyModes.OnlyWhenPinned || _model.IsPinned)
            && !(settings.Notes_OpaqueWhenFocused && _view.IsActive)
        );

        _view.Opacity = (isTransparent) ? settings.Notes_TransparentOpacity : settings.Notes_OpaqueOpacity;
    }

    private void ApplyTheme()
    {
        SettingsModel settings = _model.Settings ?? _applicationManager.ApplicationSettings;

        ThemeColorsModel themeColor = ThemeHelper.GetThemeColorForMode(_model.Theme, settings.Notes_ColorMode);

        _view.TitleBarColorBrush = themeColor.TitleBarColor.Brush;
        _view.TitleButtonColorBrush = themeColor.TitleBarButtonsColor.Brush;
        _view.BackgroundColorBrush = themeColor.BackgroundColor.Brush;
        _view.TextColorBrush = themeColor.TextColor.Brush;
        _view.BorderColorBrush = themeColor.BorderColor.Brush;
    }
}
