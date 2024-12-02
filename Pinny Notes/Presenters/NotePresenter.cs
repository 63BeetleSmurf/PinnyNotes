using System;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Helpers;
using PinnyNotes.WpfUi.Models;
using PinnyNotes.WpfUi.Properties;
using PinnyNotes.WpfUi.Views;

namespace PinnyNotes.WpfUi.Presenters;

public class NotePresenter
{
    private readonly NoteModel _model;
    private readonly NoteWindow _view;

    public NotePresenter(NoteModel model, NoteWindow view)
    {
        _view = view;
        _model = model;

        _view.Width = _model.Width;
        _view.Height = _model.Height;
        _view.Left = _model.X;
        _view.Top = _model.Y;

        _view.WindowLoaded += OnWindowLoaded;
        _view.WindowMoved += OnWindowMoved;
        _view.WindowActivated += OnWindowActivated;

        _view.TitleBarRightClicked += OnTitleBarRightClicked;

        _view.TextChanged += OnTextChanged;

        ApplyTheme();
        UpdateWindowOpacity();
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
        _view.Topmost = _model.IsPinned;
        UpdateWindowOpacity();
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
        _model.SetDefaultSize();
        _view.Width = _model.Width;
        _view.Height = _model.Height;
    }

    private void OnChangeThemeMenuItemClicked(object? sender, EventArgs e)
    {
        if (sender is MenuItem menuItem)
        {
            ThemeModel? theme = ThemeHelper.Themes.Find(t => t.Name == (string)menuItem.Header);
            if (theme == null)
                return;

            _model.Theme = theme;
            ApplyTheme();
        }
    }

    private void OnSettingsMenuItemClicked(object? sender, EventArgs e)
    {
        
    }

    private void OnTextChanged(object? sender, EventArgs e)
    {
        _model.Text = _view.Text;
    }

    private void UpdateWindowOpacity()
    {
        bool transparentNotes = Settings.Default.TransparentNotes;
        bool opaqueWhenFocused = Settings.Default.OpaqueWhenFocused;
        bool onlyTransparentWhenPinned = Settings.Default.OnlyTransparentWhenPinned;

        if (_view.IsFocused)
            _view.Opacity = (transparentNotes && !opaqueWhenFocused && !onlyTransparentWhenPinned) ? _model.TransparentOpacity : _model.OpaqueOpacity;
        else if (_model.IsPinned)
            _view.Opacity = transparentNotes ? _model.TransparentOpacity : _model.OpaqueOpacity;
        else
            _view.Opacity = (transparentNotes && !onlyTransparentWhenPinned) ? _model.TransparentOpacity : _model.OpaqueOpacity;
    }

    private void ApplyTheme()
    {
        ThemeColorModel themeColor;
        ColorModes colorMode = (ColorModes)Settings.Default.ColorMode;

        if (colorMode == ColorModes.Dark || (colorMode == ColorModes.System && SystemThemeHelper.IsDarkMode()))
            themeColor = _model.Theme.DarkColor;
        else
            themeColor = _model.Theme.LightColor;

        _view.TitleBarColorBrush = themeColor.TitleBarBrush;
        _view.TitleButtonColorBrush = themeColor.TitleBarButtonsBrush;
        _view.BackgroundColorBrush = themeColor.BackgroundColorBrush;
        _view.TextColorBrush = themeColor.TextColorBrush;
        _view.BorderColorBrush = themeColor.BorderColorBrush;
    }
}
