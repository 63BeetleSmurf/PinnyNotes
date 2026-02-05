using PinnyNotes.Core.Enums;

namespace PinnyNotes.WpfUi.Models;

public class ToolSettingsModel : BaseModel
{
    public ToolStates Base64ToolState { get; set => SetProperty(ref field, value); }
    public ToolStates BracketToolState { get; set => SetProperty(ref field, value); }
    public ToolStates CaseToolState { get; set => SetProperty(ref field, value); }
    public ToolStates ColorToolState { get; set => SetProperty(ref field, value); }
    public ToolStates DateTimeToolState { get; set => SetProperty(ref field, value); }
    public ToolStates GibberishToolState { get; set => SetProperty(ref field, value); }
    public ToolStates GuidToolState { get; set => SetProperty(ref field, value); }
    public ToolStates HashToolState { get; set => SetProperty(ref field, value); }
    public ToolStates HtmlEntityToolState { get; set => SetProperty(ref field, value); }
    public ToolStates IndentToolState { get; set => SetProperty(ref field, value); }
    public ToolStates JoinToolState { get; set => SetProperty(ref field, value); }
    public ToolStates JsonToolState { get; set => SetProperty(ref field, value); }
    public ToolStates ListToolState { get; set => SetProperty(ref field, value); }
    public ToolStates QuoteToolState { get; set => SetProperty(ref field, value); }
    public ToolStates RemoveToolState { get; set => SetProperty(ref field, value); }
    public ToolStates SlashToolState { get; set => SetProperty(ref field, value); }
    public ToolStates SortToolState { get; set => SetProperty(ref field, value); }
    public ToolStates SplitToolState { get; set => SetProperty(ref field, value); }
    public ToolStates TrimToolState { get; set => SetProperty(ref field, value); }
    public ToolStates UrlToolState { get; set => SetProperty(ref field, value); }
}
