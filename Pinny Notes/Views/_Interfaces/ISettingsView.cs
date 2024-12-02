using System;
using System.Collections.Generic;

using PinnyNotes.WpfUi.Enums;

namespace PinnyNotes.WpfUi.Views;

public interface ISettingsView
{
    public void Close();

    List<KeyValuePair<int, string>> PopulateColorModes { set; }
    List<KeyValuePair<int, string>> PopulateMinimizeModes { set; }
    List<KeyValuePair<int, string>> PopulateStartupPositions { set; }
    List<KeyValuePair<int, string>> PopulateToolStates { set; }

    bool AutoCopy { get; set; }
    bool AutoIndent { get; set; }
    bool CheckForUpdates { get; set; }
    ColorModes ColorMode { get; set; }
    bool ConvertIndentation { get; set; }
    bool CycleColors { get; set; }
    bool HideTitleBar { get; set; }
    bool KeepNewLineAtEndVisible { get; set; }
    bool MiddleClickPaste { get; set; }
    MinimizeModes MinimizeMode { get; set; }
    bool NewLineAtEnd { get; set; }
    bool OnlyTransparentWhenPinned { get; set; }
    bool OpaqueWhenFocused { get; set; }
    bool ShowNotesInTaskbar { get; set; }
    bool ShowTrayIcon { get; set; }
    bool SpellChecker { get; set; }
    StartupPositions StartupPosition { get; set; }
    bool TabSpaces { get; set; }
    int TabWidth { get; set; }
    bool TransparentNotes { get; set; }
    bool TrimCopiedText { get; set; }
    bool TrimPastedText { get; set; }
    bool UseMonoFont { get; set; }

    ToolStates Base64ToolState { get; set; }
    ToolStates BracketToolState { get; set; }
    ToolStates CaseToolState { get; set; }
    ToolStates DateTimeToolState { get; set; }
    ToolStates GibberishToolState { get; set; }
    ToolStates HashToolState { get; set; }
    ToolStates HtmlEntityToolState { get; set; }
    ToolStates IndentToolState { get; set; }
    ToolStates JoinToolState { get; set; }
    ToolStates JsonToolState { get; set; }
    ToolStates ListToolState { get; set; }
    ToolStates QuoteToolState { get; set; }
    ToolStates RemoveToolState { get; set; }
    ToolStates SlashToolState { get; set; }
    ToolStates SortToolState { get; set; }
    ToolStates SplitToolState { get; set; }
    ToolStates TrimToolState { get; set; }

    event EventHandler SaveClicked;
    event EventHandler CloseClicked;
}
