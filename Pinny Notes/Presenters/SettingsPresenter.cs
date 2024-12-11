using System;
using System.Windows;

using PinnyNotes.WpfUi.Helpers;
using PinnyNotes.WpfUi.Models;
using PinnyNotes.WpfUi.Views;

namespace PinnyNotes.WpfUi.Presenters;

public class SettingsPresenter
{
    private readonly ApplicationManager _applicaitonManager;
    private readonly SettingsModel _model;
    private readonly SettingsWindow _view;

    public SettingsPresenter(ApplicationManager applicaitonManager, SettingsModel model, SettingsWindow view)
    {
        _applicaitonManager = applicaitonManager;
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
        _view.PopulateNotes_DefaultColors(SettingsModel.DefaultColorList);
        _view.PopulateNotes_ColorModes(SettingsModel.ColorModeList);
        _view.PopulateNotes_TransparencyModes(SettingsModel.TransparencyModeList);
        _view.PopulateToolStates(SettingsModel.ToolStateList);
    }

    private void LoadSettings()
    {
        _model.LoadSettings();

        _view.Applicaiton_TrayIcon = _model.Applicaiton_TrayIcon;
        _view.Applicaiton_NotesInTaskbar = _model.Applicaiton_NotesInTaskbar;
        _view.Applicaiton_CheckForUpdates = _model.Applicaiton_CheckForUpdates;

        _view.Notes_StartupPosition = _model.Notes_StartupPosition;
        _view.Notes_MinimizeMode = _model.Notes_MinimizeMode;
        _view.Notes_HideTitleBar = _model.Notes_HideTitleBar;
        _view.Notes_DefaultColor = _model.Notes_DefaultColor;
        _view.Notes_ColorMode = _model.Notes_ColorMode;
        _view.Notes_TransparencyMode = _model.Notes_TransparencyMode;
        _view.Notes_OpaqueWhenFocused = _model.Notes_OpaqueWhenFocused;

        _view.Editor_MonoFont = _model.Editor_MonoFont;
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

        _view.Base64ToolState = _model.Base64ToolState;
        _view.BracketToolState = _model.BracketToolState;
        _view.CaseToolState = _model.CaseToolState;
        _view.DateTimeToolState = _model.DateTimeToolState;
        _view.GibberishToolState = _model.GibberishToolState;
        _view.HashToolState = _model.HashToolState;
        _view.HtmlEntityToolState = _model.HtmlEntityToolState;
        _view.IndentToolState = _model.IndentToolState;
        _view.JoinToolState = _model.JoinToolState;
        _view.JsonToolState = _model.JsonToolState;
        _view.ListToolState = _model.ListToolState;
        _view.QuoteToolState = _model.QuoteToolState;
        _view.RemoveToolState = _model.RemoveToolState;
        _view.SlashToolState = _model.SlashToolState;
        _view.SortToolState = _model.SortToolState;
        _view.SplitToolState = _model.SplitToolState;
        _view.TrimToolState = _model.TrimToolState;
    }

    private void SaveSettings()
    {
        _view.ApplyButton.IsEnabled = false;

        _model.Applicaiton_TrayIcon = _view.Applicaiton_TrayIcon;
        _model.Applicaiton_NotesInTaskbar = _view.Applicaiton_NotesInTaskbar;
        _model.Applicaiton_CheckForUpdates = _view.Applicaiton_CheckForUpdates;

        _model.Notes_StartupPosition = _view.Notes_StartupPosition;
        _model.Notes_MinimizeMode = _view.Notes_MinimizeMode;
        _model.Notes_HideTitleBar = _view.Notes_HideTitleBar;
        _model.Notes_DefaultColor = _view.Notes_DefaultColor;
        _model.Notes_ColorMode = _view.Notes_ColorMode;
        _model.Notes_TransparencyMode = _view.Notes_TransparencyMode;
        _model.Notes_OpaqueWhenFocused = _view.Notes_OpaqueWhenFocused;

        _model.Editor_MonoFont = _view.Editor_MonoFont;
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

        _model.Base64ToolState = _view.Base64ToolState;
        _model.BracketToolState = _view.BracketToolState;
        _model.CaseToolState = _view.CaseToolState;
        _model.DateTimeToolState = _view.DateTimeToolState;
        _model.GibberishToolState = _view.GibberishToolState;
        _model.HashToolState = _view.HashToolState;
        _model.HtmlEntityToolState = _view.HtmlEntityToolState;
        _model.IndentToolState = _view.IndentToolState;
        _model.JoinToolState = _view.JoinToolState;
        _model.JsonToolState = _view.JsonToolState;
        _model.ListToolState = _view.ListToolState;
        _model.QuoteToolState = _view.QuoteToolState;
        _model.RemoveToolState = _view.RemoveToolState;
        _model.SlashToolState = _view.SlashToolState;
        _model.SortToolState = _view.SortToolState;
        _model.SplitToolState = _view.SplitToolState;
        _model.TrimToolState = _view.TrimToolState;

        _model.SaveSettings();

        SettingsSaved?.Invoke(this, EventArgs.Empty);
    }

    private void HideWindow()
    {
        _view.Owner = null;
        _view.Hide();
    }

    private void AddChangeHandlers()
    {
        _view.Applicaiton_TrayIconCheckBox.Click += OnSettingsChanged;
        _view.Applicaiton_NotesInTaskbarCheckBox.Click += OnSettingsChanged;
        _view.Applicaiton_CheckForUpdatesCheckBox.Click += OnSettingsChanged;

        _view.Notes_StartupPositionComboBox.SelectionChanged += OnSettingsChanged;
        _view.Notes_MinimizeModeComboBox.SelectionChanged += OnSettingsChanged;
        _view.Notes_HideTitleBarCheckBox.Click += OnSettingsChanged;
        _view.Notes_DefaultColorComboBox.SelectionChanged += OnSettingsChanged;
        _view.Notes_ColorModeComboBox.SelectionChanged += OnSettingsChanged;
        _view.Notes_TransparencyModeComboBox.SelectionChanged += OnSettingsChanged;
        _view.Notes_OpaqueWhenFocusedCheckBox.Click += OnSettingsChanged;

        _view.Editor_MonoFontCheckBox.Click += OnSettingsChanged;
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

        _view.Base64ToolStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.BracketToolStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.CaseToolStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.DateTimeToolStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.GibberishToolStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.HashToolStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.HtmlEntityToolStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.IndentToolStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.JoinToolStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.JsonToolStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.ListToolStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.QuoteToolStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.RemoveToolStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.SlashToolStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.SortToolStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.SplitToolStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.TrimToolStateComboBox.SelectionChanged += OnSettingsChanged;
    }
}
