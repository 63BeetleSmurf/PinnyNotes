using PinnyNotes.Core.Enums;
using PinnyNotes.WpfUi.Base;

namespace PinnyNotes.WpfUi.Models;

public class ToolSettingsModel : NotifyPropertyChangedBase
{
    public ToolStates Base64ToolState { get => _base64ToolState; set => SetProperty(ref _base64ToolState, value); }
    private ToolStates _base64ToolState;
    public ToolStates BracketToolState { get => _bracketToolState; set => SetProperty(ref _bracketToolState, value); }
    private ToolStates _bracketToolState;
    public ToolStates CaseToolState { get => _caseToolState; set => SetProperty(ref _caseToolState, value); }
    private ToolStates _caseToolState;
    public ToolStates ColorToolState { get => _colorToolState; set => SetProperty(ref _colorToolState, value); }
    private ToolStates _colorToolState;
    public ToolStates DateTimeToolState { get => _dateTimeToolState; set => SetProperty(ref _dateTimeToolState, value); }
    private ToolStates _dateTimeToolState;
    public ToolStates GibberishToolState { get => _gibberishToolState; set => SetProperty(ref _gibberishToolState, value); }
    private ToolStates _gibberishToolState;
    public ToolStates HashToolState { get => _hashToolState; set => SetProperty(ref _hashToolState, value); }
    private ToolStates _hashToolState;
    public ToolStates HtmlEntityToolState { get => _htmlEntityToolState; set => SetProperty(ref _htmlEntityToolState, value); }
    private ToolStates _htmlEntityToolState;
    public ToolStates IndentToolState { get => _indentToolState; set => SetProperty(ref _indentToolState, value); }
    private ToolStates _indentToolState;
    public ToolStates JoinToolState { get => _joinToolState; set => SetProperty(ref _joinToolState, value); }
    private ToolStates _joinToolState;
    public ToolStates JsonToolState { get => _jsonToolState; set => SetProperty(ref _jsonToolState, value); }
    private ToolStates _jsonToolState;
    public ToolStates ListToolState { get => _listToolState; set => SetProperty(ref _listToolState, value); }
    private ToolStates _listToolState;
    public ToolStates QuoteToolState { get => _quoteToolState; set => SetProperty(ref _quoteToolState, value); }
    private ToolStates _quoteToolState;
    public ToolStates RemoveToolState { get => _removeToolState; set => SetProperty(ref _removeToolState, value); }
    private ToolStates _removeToolState;
    public ToolStates SlashToolState { get => _slashToolState; set => SetProperty(ref _slashToolState, value); }
    private ToolStates _slashToolState;
    public ToolStates SortToolState { get => _sortToolState; set => SetProperty(ref _sortToolState, value); }
    private ToolStates _sortToolState;
    public ToolStates SplitToolState { get => _splitToolState; set => SetProperty(ref _splitToolState, value); }
    private ToolStates _splitToolState;
    public ToolStates TrimToolState { get => _trimToolState; set => SetProperty(ref _trimToolState, value); }
    private ToolStates _trimToolState;
    public ToolStates UrlToolState { get => _urlToolState; set => SetProperty(ref _urlToolState, value); }
    private ToolStates _urlToolState;
}
