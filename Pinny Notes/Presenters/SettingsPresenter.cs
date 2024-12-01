using System;

using PinnyNotes.WpfUi.Models;
using PinnyNotes.WpfUi.Views;

namespace PinnyNotes.WpfUi.Presenters;

public class SettingsPresenter
{
    private SettingsModel _model;
    private ISettingsView _view;

    public SettingsPresenter(SettingsModel model, ISettingsView view)
    {
        _model = model;
        _view = view;

        _view.SaveClicked += OnSaveClicked;
        _view.CancelClicked += OnCancelClicked;

        LoadSettings();
    }

    private void LoadSettings()
    {
        _model.StartupPosition;
        _model.CycleColors;
        _model.TrimCopiedText;
        _model.TrimPastedText;
        _model.MiddleClickPaste;
        _model.AutoCopy;
        _model.SpellChecker;
        _model.NewLineAtEnd;
        _model.KeepNewLineAtEndVisible;
        _model.AutoIndent;
        _model.TabSpaces;
        _model.TabWidth;
        _model.ConvertIndentation;
        _model.MinimizeMode;
        _model.TransparentNotes;
        _model.OpaqueWhenFocused;
        _model.OnlyTransparentWhenPinned;
        _model.ColorMode;
        _model.UseMonoFont;
        _model.HideTitleBar;
        _model.ShowTrayIcon;
        _model.ShowNotesInTaskbar;
        _model.CheckForUpdates;

        #region Tools

        _model.Base64ToolState;
        _model.BracketToolState;
        _model.CaseToolState;
        _model.DateTimeToolState;
        _model.GibberishToolState;
        _model.HashToolState;
        _model.HtmlEntityToolState;
        _model.IndentToolState;
        _model.JoinToolState;
        _model.JsonToolState;
        _model.ListToolState;
        _model.QuoteToolState;
        _model.RemoveToolState;
        _model.SlashToolState;
        _model.SortToolState;
        _model.SplitToolState;
        _model.TrimToolState;

        #endregion
    }

    private void SaveSettings()
    {
        _model.Property = _view.Property;

        _model.Save();
    }

    private void OnSaveClicked(object sender, EventArgs e)
    {
        SaveSettings();
        _view.Close();
    }

    private void OnCancelClicked(object sender, EventArgs e)
    {
        _view.Close();
    }
}
