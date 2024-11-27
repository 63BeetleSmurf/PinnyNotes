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
    public const int DefaultWidth = 300;
    public const int DefaultHeight = 300;

    private const double _opaqueOpacity = 1.0;
    private const double _transparentOpacity = 0.8;

    private const string _monoFontFamily = "Consolas";

    private readonly NoteModel _model;
    private readonly INoteView _view;

    public NotePresenter(NoteModel model, INoteView view)
    {
        _view = view;
        _model = model;

        _view.WindowLoaded += OnWindowLoaded;
        _view.WindowMoved += OnWindowMoved;
        _view.WindowActivated += OnWindowActivated;

        _view.TitleBarRightClicked += OnTitleBarRightClicked;

        _view.TextChanged += OnTextChanged;
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
        UpdateWindowOpacity(true);
    }

    private void OnWindowDeactivated(object? sender, EventArgs e)
    {
        _view.Topmost = _model.IsPinned;
        UpdateWindowOpacity(false);
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
        _model.Width = DefaultWidth;
        _view.Width = _model.Width;

        _model.Height = DefaultHeight;
        _view.Height = _model.Height;
    }

    private void OnChangeThemeMenuItemClicked(object? sender, EventArgs e)
    {
        if (sender is MenuItem menuItem)
            ApplyTheme((string)menuItem.Header);
    }

    private void OnSettingsMenuItemClicked(object? sender, EventArgs e)
    {
        
    }

    private void OnTextChanged(object? sender, EventArgs e)
    {
        _model.Text = _view.Text;
    }

    private void UpdateWindowOpacity(bool IsFocused)
    {
        bool transparentNotes = Settings.Default.TransparentNotes;
        bool opaqueWhenFocused = Settings.Default.OpaqueWhenFocused;
        bool onlyTransparentWhenPinned = Settings.Default.OnlyTransparentWhenPinned;

        if (IsFocused)
            _view.Opacity = (transparentNotes && !opaqueWhenFocused && !onlyTransparentWhenPinned) ? _transparentOpacity : _opaqueOpacity;
        else if (_model.IsPinned)
            _view.Opacity = transparentNotes ? _transparentOpacity : _opaqueOpacity;
        else
            _view.Opacity = (transparentNotes && !onlyTransparentWhenPinned) ? _transparentOpacity : _opaqueOpacity;
    }

    private void ApplyTheme(string themeName)
    {
        ThemeModel? theme = ThemeHelper.Themes.Find(t => t.Name == themeName);
        if (theme == null)
            return;

        ThemeColorModel themeColor;
        ColorModes colorMode = (ColorModes)Settings.Default.ColorMode;

        if (colorMode == ColorModes.Dark || (colorMode == ColorModes.System && SystemThemeHelper.IsDarkMode()))
            themeColor = theme.DarkColor;
        else
            themeColor = theme.LightColor;

        _view.TitleBarColorBrush = themeColor.TitleBarBrush;
        _view.TitleButtonColorBrush = themeColor.TitleBarButtonsBrush;
        _view.BackgroundColorBrush = themeColor.BackgroundColorBrush;
        _view.TextColorBrush = themeColor.TextColorBrush;
        _view.BorderColorBrush = themeColor.BorderColorBrush;
    }
}
