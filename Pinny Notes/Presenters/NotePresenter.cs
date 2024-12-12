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
        _model.LoadSettings();
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
        if (_view.WindowState == WindowState.Minimized
            && (
                _model.Settings.Notes_MinimizeMode == MinimizeModes.Prevent
                || (_model.Settings.Notes_MinimizeMode == MinimizeModes.PreventIfPinned && _view.Topmost)
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
            ThemeModel? theme = ThemeHelper.Themes.Find(t => t.Key == (string)menuItem.Tag);
            if (theme == null)
                return;

            _model.Theme = theme;
            _model.SaveTheme();
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
        _view.Width = _model.Width;
        _view.Height = _model.Height;
        _view.Left = _model.X;
        _view.Top = _model.Y;
        _view.PinButtonState = _model.IsPinned;

        _view.HideTitleBar = _model.Settings.Notes_HideTitleBar;
        _view.ShowInTaskbar = _model.Settings.Application_NotesInTaskbar;

        _view.UseMonoFont = _model.Settings.Editor_UseMonoFont;
        _view.MonoFontFamily = _model.Settings.Editor_MonoFontFamily;
        _view.SpellCheck = _model.Settings.Editor_SpellCheck;
        _view.AutoIndent = _model.Settings.Editor_AutoIndent;
        _view.NewLineAtEnd = _model.Settings.Editor_NewLineAtEnd;
        _view.KeepNewLineVisible = _model.Settings.Editor_KeepNewLineVisible;
        _view.TabSpaces = _model.Settings.Editor_TabsToSpaces;
        _view.ConvertTabs = _model.Settings.Editor_ConvertIndentationOnPaste;
        _view.TabWidth = _model.Settings.Editor_TabWidth;
        _view.MiddleClickPaste = _model.Settings.Editor_MiddleClickPaste;
        _view.TrimPastedText = _model.Settings.Editor_TrimPastedText;
        _view.TrimCopiedText = _model.Settings.Editor_TrimCopiedText;
        _view.AutoCopy = _model.Settings.Editor_CopyHighlightedText;
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
        ThemeColorModel themeColor;

        if (_model.Settings.Notes_ColorMode == ColorModes.Dark || (_model.Settings.Notes_ColorMode == ColorModes.System && SystemThemeHelper.IsDarkMode()))
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
