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
        _view.PinButtonState = _model.IsPinned;

        _view.HideTitleBar = _model.HideTitleBar;
        _view.ShowInTaskbar = _model.ShowInTaskbar;
        _view.MonoFontFamily = _model.MonoFontFamily;
        _view.UseMonoFont = _model.UseMonoFont;
        _view.SpellCheck = _model.SpellCheck;
        _view.AutoIndent = _model.AutoIndent;
        _view.NewLineAtEnd = _model.NewLineAtEnd;
        _view.KeepNewLineVisible = _model.KeepNewLineVisible;
        _view.TabSpaces = _model.TabSpaces;
        _view.ConvertTabs = _model.ConvertTabs;
        _view.TabWidth = _model.TabWidth;
        _view.MiddleClickPaste = _model.MiddleClickPaste;
        _view.TrimPastedText = _model.TrimPastedText;
        _view.TrimCopiedText = _model.TrimCopiedText;
        _view.AutoCopy = _model.AutoCopy;

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

    public void ShowWindow() => _view.Show();

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
        if (_view.WindowState == WindowState.Minimized
            && (
                _model.MinimizeMode == MinimizeModes.Prevent
                || (_model.MinimizeMode == MinimizeModes.PreventIfPinned && _view.Topmost)
            )
        )
            _view.WindowState = WindowState.Normal;
    }

    private void OnNewNoteClicked(object? sender, EventArgs e)
    {
        ((App)Application.Current).CreateNewNote(_model);
    }

    private void OnPinClicked(object? sender, EventArgs e)
    {
        _model.IsPinned = _view.PinButtonState;
        _view.Topmost = _model.IsPinned;
        UpdateWindowOpacity();
    }

    private void OnCloseNoteClicked(object? sender, EventArgs e)
    {
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
        ((App)Application.Current).ShowSettingsWindow();
    }

    private void OnTextChanged(object? sender, EventArgs e)
    {
        _model.Text = _view.Text;
    }

    private void UpdateWindowOpacity()
    {
        bool isTransparent = (
            _model.TransparencyMode != TransparencyModes.Disabled
            && (_model.TransparencyMode != TransparencyModes.OnlyWhenPinned || _model.IsPinned)
            && !(_model.OpaqueWhenFocused && _view.IsActive)
        );

        _view.Opacity = (isTransparent) ? _model.DefaultTransparentOpacity : _model.DefaultOpaqueOpacity;
    }

    private void ApplyTheme()
    {
        ThemeColorModel themeColor;

        if (_model.ThemeColorMode == ColorModes.Dark || (_model.ThemeColorMode == ColorModes.System && SystemThemeHelper.IsDarkMode()))
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
