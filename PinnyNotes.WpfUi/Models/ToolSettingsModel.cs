using PinnyNotes.Core.Enums;

namespace PinnyNotes.WpfUi.Models;

public class ToolSettingsModel
{
    public ToolStates Base64ToolState { get; set; }
    public ToolStates BracketToolState { get; set; }
    public ToolStates CaseToolState { get; set; }
    public ToolStates ColorToolState { get; set; }
    public ToolStates DateTimeToolState { get; set; }
    public ToolStates GibberishToolState { get; set; }
    public ToolStates HashToolState { get; set; }
    public ToolStates HtmlEntityToolState { get; set; }
    public ToolStates IndentToolState { get; set; }
    public ToolStates JoinToolState { get; set; }
    public ToolStates JsonToolState { get; set; }
    public ToolStates ListToolState { get; set; }
    public ToolStates QuoteToolState { get; set; }
    public ToolStates RemoveToolState { get; set; }
    public ToolStates SlashToolState { get; set; }
    public ToolStates SortToolState { get; set; }
    public ToolStates SplitToolState { get; set; }
    public ToolStates TrimToolState { get; set; }
}
