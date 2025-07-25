using PinnyNotes.Core.Enums;

namespace PinnyNotes.DataAccess.Models;

public class SettingsDataModel
{
    public int Id { get; init; }

    public bool ShowTrayIcon { get; set; }
    public bool CheckForUpdates { get; set; }

    public bool ShowNotesInTaskbar { get; set; }
    public double DefaultNoteWidth { get; set; }
    public double DefaultNoteHeight { get; set; }
    public StartupPositions StartupPosition { get; set; }
    public MinimizeModes MinimizeMode { get; set; }
    public bool HideTitleBar { get; set; }
    public bool CycleColors { get; set; }
    public ColorModes ColorMode { get; set; }
    public TransparencyModes TransparencyMode { get; set; }
    public bool OpaqueWhenFocused { get; set; }
    public double OpaqueOpacity { get; set; }
    public double TransparentOpacity { get; set; }

    public bool SpellCheck { get; set; }
    public bool AutoIndent { get; set; }
    public bool NewLineAtEnd { get; set; }
    public bool KeepNewLineVisible { get; set; }
    public bool WrapText { get; set; }
    public required string StandardFontFamily { get; set; }
    public required string MonoFontFamily { get; set; }
    public bool UseMonoFont { get; set; }
    public bool TabUsesSpaces { get; set; }
    public bool ConvertIndentationOnPaste { get; set; }
    public int TabWidth { get; set; }
    public CopyActions CopyAction { get; set; }
    public CopyActions CopyAltAction { get; set; }
    public CopyFallbackActions CopyFallbackAction { get; set; }
    public CopyFallbackActions CopyAltFallbackAction { get; set; }
    public bool CopyTextOnHighlight { get; set; }
    public bool TrimCopiedText { get; set; }
    public bool MiddleClickPaste { get; set; }
    public bool TrimPastedText { get; set; }

    public ToolStates Base64State { get; set; }
    public ToolStates BracketState { get; set; }
    public ToolStates CaseState { get; set; }
    public ToolStates ColorState { get; set; }
    public ToolStates DateTimeState { get; set; }
    public ToolStates GibberishState { get; set; }
    public ToolStates HashState { get; set; }
    public ToolStates HTMLEntityState { get; set; }
    public ToolStates IndentState { get; set; }
    public ToolStates JoinState { get; set; }
    public ToolStates JSONState { get; set; }
    public ToolStates ListState { get; set; }
    public ToolStates QuoteState { get; set; }
    public ToolStates RemoveState { get; set; }
    public ToolStates SlashState { get; set; }
    public ToolStates SortState { get; set; }
    public ToolStates SplitState { get; set; }
    public ToolStates TrimState { get; set; }
}
