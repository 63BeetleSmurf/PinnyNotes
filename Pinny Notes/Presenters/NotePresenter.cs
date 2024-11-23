using System;
using System.Windows.Media;

using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Helpers;
using PinnyNotes.WpfUi.Models;
using PinnyNotes.WpfUi.Properties;
using PinnyNotes.WpfUi.Views;

namespace PinnyNotes.WpfUi.Presenters;

public class NotePresenter
{
    public const double DefaultWidth = 300.0;
    public const double DefaultHeight = 300.0;

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

    private void UpdateBrushes(ThemeColors themeColor)
    {
        ColorModes colorMode = (ColorModes)Settings.Default.ColorMode;

        // 0 = Title, 1 = Background, 2 = Border
        _view.BorderColorBrush = new SolidColorBrush(ThemeHelper.Colors[themeColor][2]);

        if (colorMode == ColorModes.Dark || (colorMode == ColorModes.System && SystemThemeHelper.IsDarkMode()))
        {
            _view.TitleBarColorBrush = new SolidColorBrush(Color.FromRgb(70, 70, 70));    // #464646
            _view.TitleButtonColorBrush = new SolidColorBrush(ThemeHelper.Colors[themeColor][2]);
            _view.BackgroundColorBrush = new SolidColorBrush(Color.FromRgb(50, 50, 50));  // #323232
            _view.TextColorBrush = new SolidColorBrush(Colors.White);
        }
        else
        {
            _view.TitleBarColorBrush = new SolidColorBrush(ThemeHelper.Colors[themeColor][0]);
            _view.TitleButtonColorBrush = new SolidColorBrush(Color.FromRgb(70, 70, 70));    // #464646
            _view.BackgroundColorBrush = new SolidColorBrush(ThemeHelper.Colors[themeColor][1]);
            _view.TextColorBrush = new SolidColorBrush(Colors.Black);
        }
    }
}
