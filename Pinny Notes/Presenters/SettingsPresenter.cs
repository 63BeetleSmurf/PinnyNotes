using System;
using System.Windows;

using PinnyNotes.WpfUi.Helpers;
using PinnyNotes.WpfUi.Models;
using PinnyNotes.WpfUi.Views;

namespace PinnyNotes.WpfUi.Presenters;

public class SettingsPresenter
{
    private readonly ApplicationManager _applicationManager;
    private readonly SettingsModel _model;
    private readonly SettingsWindow _view;

    public SettingsPresenter(ApplicationManager applicationManager, SettingsModel model, SettingsWindow view)
    {
        _applicationManager = applicationManager;
        _model = model;
        _view = view;

        _view.WindowClosing += OnWindowClosing;

        _view.OkClicked += OnOkClicked;
        _view.CancelClicked += OnCancelClicked;
        _view.ApplyClicked += OnApplyClicked;

        AddChangeHandlers();
    }

    public event EventHandler? SettingsSaved;

    public void ShowWindow(Window? owner = null)
    {
        _view.Owner = owner;

        PositionWindow();

        PopulateLists();
        LoadSettings();
        _view.ApplyButton.IsEnabled = false;

        if (_view.IsVisible)
            _view.Activate();
        else
            _view.Show();
    }

    private void OnWindowClosing(object? sender, EventArgs e)
    {
        HideWindow();
    }

    private void OnOkClicked(object? sender, EventArgs e)
    {
        SaveSettings();
        HideWindow();
    }

    private void OnCancelClicked(object? sender, EventArgs e)
    {
        HideWindow();
    }

    private void OnApplyClicked(object? sender, EventArgs e)
    {
        SaveSettings();
    }

    private void OnSettingsChanged(object? sender, EventArgs e)
    {
        _view.ApplyButton.IsEnabled = true;
    }

    private void PositionWindow()
    {
        if (_view.Owner == null)
        {
            _view.Left = SystemParameters.PrimaryScreenWidth / 2 - _view.Width / 2;
            _view.Top = SystemParameters.PrimaryScreenHeight / 2 - _view.Height / 2;
        }
        else
        {
            Point position = new(
                (_view.Owner.Left + _view.Owner.Width / 2) - _view.Width / 2,
                (_view.Owner.Top + _view.Owner.Height / 2) - _view.Height / 2
            );
            System.Drawing.Rectangle currentScreenBounds = ScreenHelper.GetCurrentScreenBounds(
                ScreenHelper.GetWindowHandle(_view.Owner)
            );

            if (position.X < currentScreenBounds.Left)
                position.X = currentScreenBounds.Left;
            else if (position.X + _view.Width > currentScreenBounds.Right)
                position.X = currentScreenBounds.Right - _view.Width;

            if (position.Y < currentScreenBounds.Top)
                position.Y = currentScreenBounds.Top;
            else if (position.Y + _view.Height > currentScreenBounds.Bottom)
                position.Y = currentScreenBounds.Bottom - _view.Height;

            _view.Left = position.X;
            _view.Top = position.Y;
        }
    }

    private void PopulateLists()
    {
        _view.PopulateNotes_StartupPositions(SettingsModel.StartupPositionsList);
        _view.PopulateNotes_MinimizeModes(SettingsModel.MinimizeModeList);
        _view.PopulateNotes_DefaultThemeColors(SettingsModel.DefaultColorList);
        _view.PopulateNotes_ColorModes(SettingsModel.ColorModeList);
        _view.PopulateNotes_TransparencyModes(SettingsModel.TransparencyModeList);
        _view.PopulateToolStates(SettingsModel.ToolStateList);
    }

    private void LoadSettings()
    {
        _view.Application_TrayIcon = _model.Application_TrayIcon;
        _view.Application_NotesInTaskbar = _model.Application_NotesInTaskbar;
        _view.Application_CheckForUpdates = _model.Application_CheckForUpdates;

        _view.Notes_DefaultWidth = _model.Notes_DefaultWidth;
        _view.Notes_DefaultHeight = _model.Notes_DefaultHeight;
        _view.Notes_StartupPosition = _model.Notes_StartupPosition;
        _view.Notes_MinimizeMode = _model.Notes_MinimizeMode;
        _view.Notes_HideTitleBar = _model.Notes_HideTitleBar;
        _view.Notes_DefaultThemeColorKey = _model.Notes_DefaultThemeColorKey;
        _view.Notes_ColorMode = _model.Notes_ColorMode;
        _view.Notes_TransparencyMode = _model.Notes_TransparencyMode;
        _view.Notes_OpaqueWhenFocused = _model.Notes_OpaqueWhenFocused;
        _view.Notes_TransparentOpacity = _model.Notes_TransparentOpacity;
        _view.Notes_OpaqueOpacity = _model.Notes_OpaqueOpacity;

        _view.Editor_UseMonoFont = _model.Editor_UseMonoFont;
        _view.Editor_MonoFontFamily = _model.Editor_MonoFontFamily;
        _view.Editor_SpellCheck = _model.Editor_SpellCheck;
        _view.Editor_AutoIndent = _model.Editor_AutoIndent;
        _view.Editor_NewLineAtEnd = _model.Editor_NewLineAtEnd;
        _view.Editor_KeepNewLineVisible = _model.Editor_KeepNewLineVisible;
        _view.Editor_TabsToSpaces = _model.Editor_TabsToSpaces;
        _view.Editor_ConvertIndentationOnPaste = _model.Editor_ConvertIndentationOnPaste;
        _view.Editor_TabWidth = _model.Editor_TabWidth;
        _view.Editor_MiddleClickPaste = _model.Editor_MiddleClickPaste;

        _view.Editor_TrimPastedText = _model.Editor_TrimPastedText;
        _view.Editor_TrimCopiedText = _model.Editor_TrimCopiedText;
        _view.Editor_CopyHighlightedText = _model.Editor_CopyHighlightedText;

        _view.Tool_Base64State = _model.Tool_Base64State;
        _view.Tool_BracketState = _model.Tool_BracketState;
        _view.Tool_CaseState = _model.Tool_CaseState;
        _view.Tool_DateTimeState = _model.Tool_DateTimeState;
        _view.Tool_GibberishState = _model.Tool_GibberishState;
        _view.Tool_HashState = _model.Tool_HashState;
        _view.Tool_HtmlEntityState = _model.Tool_HtmlEntityState;
        _view.Tool_IndentState = _model.Tool_IndentState;
        _view.Tool_JoinState = _model.Tool_JoinState;
        _view.Tool_JsonState = _model.Tool_JsonState;
        _view.Tool_ListState = _model.Tool_ListState;
        _view.Tool_QuoteState = _model.Tool_QuoteState;
        _view.Tool_RemoveState = _model.Tool_RemoveState;
        _view.Tool_SlashState = _model.Tool_SlashState;
        _view.Tool_SortState = _model.Tool_SortState;
        _view.Tool_SplitState = _model.Tool_SplitState;
        _view.Tool_TrimState = _model.Tool_TrimState;
    }

    private void SaveSettings()
    {
        _view.ApplyButton.IsEnabled = false;

        _model.Application_TrayIcon = _view.Application_TrayIcon;
        _model.Application_NotesInTaskbar = _view.Application_NotesInTaskbar;
        _model.Application_CheckForUpdates = _view.Application_CheckForUpdates;

        _model.Notes_DefaultWidth = _view.Notes_DefaultWidth;
        _model.Notes_DefaultHeight = _view.Notes_DefaultHeight;
        _model.Notes_StartupPosition = _view.Notes_StartupPosition;
        _model.Notes_MinimizeMode = _view.Notes_MinimizeMode;
        _model.Notes_HideTitleBar = _view.Notes_HideTitleBar;
        _model.Notes_DefaultThemeColorKey = _view.Notes_DefaultThemeColorKey;
        _model.Notes_ColorMode = _view.Notes_ColorMode;
        _model.Notes_TransparencyMode = _view.Notes_TransparencyMode;
        _model.Notes_OpaqueWhenFocused = _view.Notes_OpaqueWhenFocused;
        _model.Notes_TransparentOpacity = _view.Notes_TransparentOpacity;
        _model.Notes_OpaqueOpacity = _view.Notes_OpaqueOpacity;

        _model.Editor_UseMonoFont = _view.Editor_UseMonoFont;
        _model.Editor_MonoFontFamily = _view.Editor_MonoFontFamily;
        _model.Editor_SpellCheck = _view.Editor_SpellCheck;
        _model.Editor_AutoIndent = _view.Editor_AutoIndent;
        _model.Editor_NewLineAtEnd = _view.Editor_NewLineAtEnd;
        _model.Editor_KeepNewLineVisible = _view.Editor_KeepNewLineVisible;
        _model.Editor_TabsToSpaces = _view.Editor_TabsToSpaces;
        _model.Editor_ConvertIndentationOnPaste = _view.Editor_ConvertIndentationOnPaste;
        _model.Editor_TabWidth = _view.Editor_TabWidth;
        _model.Editor_MiddleClickPaste = _view.Editor_MiddleClickPaste;

        _model.Editor_TrimPastedText = _view.Editor_TrimPastedText;
        _model.Editor_TrimCopiedText = _view.Editor_TrimCopiedText;
        _model.Editor_CopyHighlightedText = _view.Editor_CopyHighlightedText;

        _model.Tool_Base64State = _view.Tool_Base64State;
        _model.Tool_BracketState = _view.Tool_BracketState;
        _model.Tool_CaseState = _view.Tool_CaseState;
        _model.Tool_DateTimeState = _view.Tool_DateTimeState;
        _model.Tool_GibberishState = _view.Tool_GibberishState;
        _model.Tool_HashState = _view.Tool_HashState;
        _model.Tool_HtmlEntityState = _view.Tool_HtmlEntityState;
        _model.Tool_IndentState = _view.Tool_IndentState;
        _model.Tool_JoinState = _view.Tool_JoinState;
        _model.Tool_JsonState = _view.Tool_JsonState;
        _model.Tool_ListState = _view.Tool_ListState;
        _model.Tool_QuoteState = _view.Tool_QuoteState;
        _model.Tool_RemoveState = _view.Tool_RemoveState;
        _model.Tool_SlashState = _view.Tool_SlashState;
        _model.Tool_SortState = _view.Tool_SortState;
        _model.Tool_SplitState = _view.Tool_SplitState;
        _model.Tool_TrimState = _view.Tool_TrimState;

        SettingsSaved?.Invoke(this, EventArgs.Empty);
    }

    private void HideWindow()
    {
        _view.Owner = null;
        _view.Hide();
    }

    private void AddChangeHandlers()
    {
        _view.Application_TrayIconCheckBox.Click += OnSettingsChanged;
        _view.Application_NotesInTaskbarCheckBox.Click += OnSettingsChanged;
        _view.Application_CheckForUpdatesCheckBox.Click += OnSettingsChanged;

        _view.Notes_DefaultWidthTextBox.TextChanged += OnSettingsChanged;
        _view.Notes_DefaultHeightTextBox.TextChanged += OnSettingsChanged;
        _view.Notes_StartupPositionComboBox.SelectionChanged += OnSettingsChanged;
        _view.Notes_MinimizeModeComboBox.SelectionChanged += OnSettingsChanged;
        _view.Notes_HideTitleBarCheckBox.Click += OnSettingsChanged;
        _view.Notes_DefaultThemeColorComboBox.SelectionChanged += OnSettingsChanged;
        _view.Notes_ColorModeComboBox.SelectionChanged += OnSettingsChanged;
        _view.Notes_TransparencyModeComboBox.SelectionChanged += OnSettingsChanged;
        _view.Notes_OpaqueWhenFocusedCheckBox.Click += OnSettingsChanged;
        _view.Notes_TransparentOpacityTextBox.TextChanged += OnSettingsChanged;
        _view.Notes_OpaqueOpacityTextBox.TextChanged += OnSettingsChanged;

        _view.Editor_UseMonoFontCheckBox.Click += OnSettingsChanged;
        _view.Editor_MonoFontFamilyTextBox.TextChanged += OnSettingsChanged;
        _view.Editor_SpellCheckCheckBox.Click += OnSettingsChanged;
        _view.Editor_AutoIndentCheckBox.Click += OnSettingsChanged;
        _view.Editor_NewLineAtEndCheckBox.Click += OnSettingsChanged;
        _view.Editor_KeepNewLineVisibleCheckBox.Click += OnSettingsChanged;
        _view.Editor_TabsToSpacesCheckBox.Click += OnSettingsChanged;
        _view.Editor_ConvertIndentationOnPasteCheckBox.Click += OnSettingsChanged;
        _view.Editor_TabWidthTextBox.TextChanged += OnSettingsChanged;
        _view.Editor_MiddleClickPasteCheckBox.Click += OnSettingsChanged;
        _view.Editor_TrimPastedTextCheckBox.Click += OnSettingsChanged;
        _view.Editor_TrimCopiedTextCheckBox.Click += OnSettingsChanged;
        _view.Editor_CopyHighlightedTextCheckBox.Click += OnSettingsChanged;

        _view.Tool_Base64StateComboBox.SelectionChanged += OnSettingsChanged;
        _view.Tool_BracketStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.Tool_CaseStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.Tool_DateTimeStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.Tool_GibberishStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.Tool_HashStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.Tool_HtmlEntityStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.Tool_IndentStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.Tool_JoinStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.Tool_JsonStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.Tool_ListStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.Tool_QuoteStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.Tool_RemoveStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.Tool_SlashStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.Tool_SortStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.Tool_SplitStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.Tool_TrimStateComboBox.SelectionChanged += OnSettingsChanged;
    }
}
