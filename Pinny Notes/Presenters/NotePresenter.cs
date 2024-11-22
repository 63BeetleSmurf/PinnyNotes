using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

using PinnyNotes.WpfUi.Helpers;
using PinnyNotes.WpfUi.Models;
using PinnyNotes.WpfUi.Properties;
using PinnyNotes.WpfUi.Views;

namespace PinnyNotes.WpfUi.Presenters;

public class NotePresenter
{
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


}
