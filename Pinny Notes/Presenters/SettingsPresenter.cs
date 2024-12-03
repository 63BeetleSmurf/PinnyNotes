using System;
using System.Windows;

using PinnyNotes.WpfUi.Models;
using PinnyNotes.WpfUi.Views;

namespace PinnyNotes.WpfUi.Presenters;

public class SettingsPresenter
{
    private SettingsModel _model;
    private SettingsWindow _view;

    public SettingsPresenter(SettingsModel model, SettingsWindow view)
    {
        _model = model;
        _view = view;

        _view.OkClicked += OnOkClicked;
        _view.CancelClicked += OnCancelClicked;
        _view.ApplyClicked += OnApplyClicked;
    }

    public void ShowWindow(Window? owner)
    {
        _view.Owner = owner;

        PopulateLists();
        LoadSettings();

        if (_view.IsVisible)
            _view.Activate();
        else
            _view.Show();
    }

    private void PopulateLists()
    {
        _view.PopulateStartupPositions(SettingsModel.StartupPositionsList);
        _view.PopulateMinimizeModes(SettingsModel.MinimizeModeList);
        _view.PopulateColorModes(SettingsModel.ColorModeList);
        _view.PopulateTransparencyModes(SettingsModel.TransparencyModeList);
        _view.PopulateToolStates(SettingsModel.ToolStateList);
    }

    private void LoadSettings()
    {
        _model.LoadSettings();

        _view.StartupPosition = _model.StartupPosition;
        _view.CycleColors = _model.CycleColors;
        _view.TrimCopiedText = _model.TrimCopiedText;
        _view.TrimCopiedText = _model.TrimCopiedText;
        _view.MiddleClickPaste = _model.MiddleClickPaste;
        _view.AutoCopy = _model.AutoCopy;
        _view.SpellChecker = _model.SpellChecker;
        _view.NewLineAtEnd = _model.NewLineAtEnd;
        _view.KeepNewLineAtEndVisible = _model.KeepNewLineAtEndVisible;
        _view.AutoIndent = _model.AutoIndent;
        _view.TabSpaces = _model.TabSpaces;
        _view.TabWidth = _model.TabWidth;
        _view.ConvertIndentation = _model.ConvertIndentation;
        _view.MinimizeMode = _model.MinimizeMode;
        _view.TransparencyMode = _model.TransparencyMode;
        _view.OpaqueWhenFocused = _model.OpaqueWhenFocused;
        _view.ColorMode = _model.ColorMode;
        _view.UseMonoFont = _model.UseMonoFont;
        _view.HideTitleBar = _model.HideTitleBar;
        _view.ShowTrayIcon = _model.ShowTrayIcon;
        _view.ShowNotesInTaskbar = _model.ShowNotesInTaskbar;
        _view.CheckForUpdates = _model.CheckForUpdates;

        #region Tools

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

        #endregion
    }

    private void SaveSettings()
    {
        _model.StartupPosition = _view.StartupPosition;
        _model.CycleColors = _view.CycleColors;
        _model.TrimCopiedText = _view.TrimCopiedText;
        _model.TrimCopiedText = _view.TrimCopiedText;
        _model.MiddleClickPaste = _view.MiddleClickPaste;
        _model.AutoCopy = _view.AutoCopy;
        _model.SpellChecker = _view.SpellChecker;
        _model.NewLineAtEnd = _view.NewLineAtEnd;
        _model.KeepNewLineAtEndVisible = _view.KeepNewLineAtEndVisible;
        _model.AutoIndent = _view.AutoIndent;
        _model.TabSpaces = _view.TabSpaces;
        _model.TabWidth = _view.TabWidth;
        _model.ConvertIndentation = _view.ConvertIndentation;
        _model.MinimizeMode = _view.MinimizeMode;
        _model.TransparencyMode = _view.TransparencyMode;
        _model.OpaqueWhenFocused = _view.OpaqueWhenFocused;
        _model.ColorMode = _view.ColorMode;
        _model.UseMonoFont = _view.UseMonoFont;
        _model.HideTitleBar = _view.HideTitleBar;
        _model.ShowTrayIcon = _view.ShowTrayIcon;
        _model.ShowNotesInTaskbar = _view.ShowNotesInTaskbar;
        _model.CheckForUpdates = _view.CheckForUpdates;

        #region Tools

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

        #endregion

        _model.SaveSettings();
    }

    private void OnOkClicked(object? sender, EventArgs e)
    {
        SaveSettings();
        _view.Hide();
    }

    private void OnCancelClicked(object? sender, EventArgs e)
    {
        _view.Hide();
    }

    private void OnApplyClicked(object? sender, EventArgs e)
    {
        SaveSettings();
    }
}
