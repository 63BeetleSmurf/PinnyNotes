using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Properties;
using PinnyNotes.WpfUi.Services;

namespace PinnyNotes.WpfUi.ViewModels;

public class SettingsViewModel : BaseViewModel
{
    private readonly MessengerService _messenger;

    private static readonly KeyValuePair<StartupPositions, string>[] _startupPositionsList = [
        new(StartupPositions.TopLeft, "Top Left"),
        new(StartupPositions.TopCenter, "Top Center"),
        new(StartupPositions.TopRight, "Top Right"),
        new(StartupPositions.MiddleLeft, "Middle Left"),
        new(StartupPositions.MiddleCenter, "Middle Center"),
        new(StartupPositions.MiddleRight, "Middle Right"),
        new(StartupPositions.BottomLeft, "Bottom Left"),
        new(StartupPositions.BottomCenter, "Bottom Center"),
        new(StartupPositions.BottomRight, "Bottom Right")
    ];

    private static readonly KeyValuePair<MinimizeModes, string>[] _minimizeModeList = [
        new(MinimizeModes.Allow, "Yes"),
        new(MinimizeModes.Prevent, "No"),
        new(MinimizeModes.PreventIfPinned, "When not pinned")
    ];

    private static readonly KeyValuePair<ColorModes, string>[] _colorModeList = [
        new(ColorModes.Light, "Light"),
        new(ColorModes.Dark, "Dark"),
        new(ColorModes.System, "System Default")
    ];

    private static readonly KeyValuePair<TransparencyModes, string>[] _transparencyModeList = [
        new(TransparencyModes.Disabled, "Disabled"),
        new(TransparencyModes.Enabled, "Enabled"),
        new(TransparencyModes.WhenPinned, "Only when pinned"),
    ];

    public SettingsViewModel(MessengerService messenger)
    {
        _messenger = messenger;

        _defaultNoteHeight = Settings.Default.DefaultNoteHeight;
        _defaultNoteWidth = Settings.Default.DefaultNoteWidth;
        _startupPosition = (StartupPositions)Settings.Default.StartupPosition;
        _cycleColors = Settings.Default.CycleColors;
        _trimCopiedText = Settings.Default.TrimCopiedText;
        _trimPastedText = Settings.Default.TrimPastedText;
        _middleClickPaste = Settings.Default.MiddleClickPaste;
        _autoCopy = Settings.Default.AutoCopy;
        _spellChecker = Settings.Default.SpellCheck;
        _newLineAtEnd = Settings.Default.NewLineAtEnd;
        _keepNewLineAtEndVisible = Settings.Default.KeepNewLineAtEndVisible;
        _autoIndent = Settings.Default.AutoIndent;
        _tabSpaces = Settings.Default.TabSpaces;
        _tabWidth = Settings.Default.TabWidth;
        _convertIndentation = Settings.Default.ConvertIndentation;
        _minimizeMode = (MinimizeModes)Settings.Default.MinimizeMode;
        _transparencyMode = (TransparencyModes)Settings.Default.TransparencyMode;
        _isTransparencyEnabled = (_transparencyMode != TransparencyModes.Disabled);
        _opaqueWhenFocused = Settings.Default.OpaqueWhenFocused;
        _opaqueOpacity = Settings.Default.OpaqueOpacity;
        _transparentOpacity = Settings.Default.TransparentOpacity;
        _colorMode = (ColorModes)Settings.Default.ColorMode;
        _useMonoFont = Settings.Default.UseMonoFont;
        _hideTitleBar = Settings.Default.HideTitleBar;
        _showTrayIcon = Settings.Default.ShowTrayIcon;
        _showNotesInTaskbar = Settings.Default.ShowNotesInTaskbar;
        _checkForUpdates = Settings.Default.CheckForUpdates;

        #region Tools

        _base64ToolEnabled = ToolSettings.Default.Base64ToolEnabled;
        _base64ToolFavourite = ToolSettings.Default.Base64ToolFavourite;
        _bracketToolEnabled = ToolSettings.Default.BracketToolEnabled;
        _bracketToolFavourite = ToolSettings.Default.BracketToolFavourite;
        _caseToolEnabled = ToolSettings.Default.CaseToolEnabled;
        _caseToolFavourite = ToolSettings.Default.CaseToolFavourite;
        _dateTimeToolEnabled = ToolSettings.Default.DateTimeToolEnabled;
        _dateTimeToolFavourite = ToolSettings.Default.DateTimeToolFavourite;
        _gibberishToolEnabled = ToolSettings.Default.GibberishToolEnabled;
        _gibberishToolFavourite = ToolSettings.Default.GibberishToolFavourite;
        _hashToolEnabled = ToolSettings.Default.HashToolEnabled;
        _hashToolFavourite = ToolSettings.Default.HashToolFavourite;
        _htmlEntityToolEnabled = ToolSettings.Default.HtmlEntityToolEnabled;
        _htmlEntityToolFavourite = ToolSettings.Default.HtmlEntityToolFavourite;
        _indentToolEnabled = ToolSettings.Default.IndentToolEnabled;
        _indentToolFavourite = ToolSettings.Default.IndentToolFavourite;
        _joinToolEnabled = ToolSettings.Default.JoinToolEnabled;
        _joinToolFavourite = ToolSettings.Default.JoinToolFavourite;
        _jsonToolEnabled = ToolSettings.Default.JsonToolEnabled;
        _jsonToolFavourite = ToolSettings.Default.JsonToolFavourite;
        _listToolEnabled = ToolSettings.Default.ListToolEnabled;
        _listToolFavourite = ToolSettings.Default.ListToolFavourite;
        _quoteToolEnabled = ToolSettings.Default.QuoteToolEnabled;
        _quoteToolFavourite = ToolSettings.Default.QuoteToolFavourite;
        _removeToolEnabled = ToolSettings.Default.RemoveToolEnabled;
        _removeToolFavourite = ToolSettings.Default.RemoveToolFavourite;
        _slashToolEnabled = ToolSettings.Default.SlashToolEnabled;
        _slashToolFavourite = ToolSettings.Default.SlashToolFavourite;
        _sortToolEnabled = ToolSettings.Default.SortToolEnabled;
        _sortToolFavourite = ToolSettings.Default.SortToolFavourite;
        _splitToolEnabled = ToolSettings.Default.SplitToolEnabled;
        _splitToolFavourite = ToolSettings.Default.SplitToolFavourite;
        _trimToolEnabled = ToolSettings.Default.TrimToolEnabled;
        _trimToolFavourite = ToolSettings.Default.TrimToolFavourite;

        #endregion
    }

    public KeyValuePair<StartupPositions, string>[] StartupPositionsList => _startupPositionsList;
    public KeyValuePair<MinimizeModes, string>[] MinimizeModeList => _minimizeModeList;
    public KeyValuePair<ColorModes, string>[] ColorModeList => _colorModeList;
    public KeyValuePair<TransparencyModes, string>[] TransparencyModeList => _transparencyModeList;

    private bool SetPropertyAndSave<T>(ref T storage, T value, bool isToolSetting = false, [CallerMemberName] string? propertyName = null)
    {
        if (!SetProperty(ref storage, value, propertyName))
            return false;

        if (isToolSetting)
        {
            if (value is Enum)
                ToolSettings.Default[propertyName] = Convert.ToInt32(value);
            else
                ToolSettings.Default[propertyName] = value;

            ToolSettings.Default.Save();
        }
        else
        {
            if (value is Enum)
                Settings.Default[propertyName] = Convert.ToInt32(value);
            else
                Settings.Default[propertyName] = value;

            Settings.Default.Save();
        }

        _messenger.SendSettingChangedNotification(propertyName!, value!);

        return true;
    }
    public int DefaultNoteHeight { get => _defaultNoteHeight; set => SetPropertyAndSave(ref _defaultNoteHeight, value); }
    private int _defaultNoteHeight;

    public int DefaultNoteWidth { get => _defaultNoteWidth; set => SetPropertyAndSave(ref _defaultNoteWidth, value); }
    private int _defaultNoteWidth;

    public StartupPositions StartupPosition { get => _startupPosition; set => SetPropertyAndSave(ref _startupPosition, value); }
    private StartupPositions _startupPosition;

    public bool CycleColors { get => _cycleColors; set => SetPropertyAndSave(ref _cycleColors, value); }
    private bool _cycleColors;

    public bool TrimCopiedText { get => _trimCopiedText; set => SetPropertyAndSave(ref _trimCopiedText, value); }
    private bool _trimCopiedText;

    public bool TrimPastedText { get => _trimPastedText; set => SetPropertyAndSave(ref _trimPastedText, value); }
    private bool _trimPastedText;

    public bool MiddleClickPaste { get => _middleClickPaste; set => SetPropertyAndSave(ref _middleClickPaste, value); }
    private bool _middleClickPaste;

    public bool AutoCopy { get => _autoCopy; set => SetPropertyAndSave(ref _autoCopy, value); }
    private bool _autoCopy;

    public bool SpellChecker { get => _spellChecker; set => SetPropertyAndSave(ref _spellChecker, value); }
    private bool _spellChecker;

    public bool NewLineAtEnd { get => _newLineAtEnd; set => SetPropertyAndSave(ref _newLineAtEnd, value); }
    private bool _newLineAtEnd;

    public bool KeepNewLineAtEndVisible { get => _keepNewLineAtEndVisible; set => SetPropertyAndSave(ref _keepNewLineAtEndVisible, value); }
    private bool _keepNewLineAtEndVisible;

    public bool AutoIndent { get => _autoIndent; set => SetPropertyAndSave(ref _autoIndent, value); }
    private bool _autoIndent;

    public bool TabSpaces { get => _tabSpaces; set => SetPropertyAndSave(ref _tabSpaces, value); }
    private bool _tabSpaces;

    public int TabWidth { get => _tabWidth; set => SetPropertyAndSave(ref _tabWidth, value); }
    private int _tabWidth;

    public bool ConvertIndentation { get => _convertIndentation; set => SetPropertyAndSave(ref _convertIndentation, value); }
    private bool _convertIndentation;

    public MinimizeModes MinimizeMode { get => _minimizeMode; set => SetPropertyAndSave(ref _minimizeMode, value); }
    private MinimizeModes _minimizeMode;

    public TransparencyModes TransparencyMode
    {
        get => _transparencyMode;
        set
        {
            SetPropertyAndSave(ref _transparencyMode, value);
            IsTransparencyEnabled = (TransparencyMode != TransparencyModes.Disabled);
        }
    }
    private TransparencyModes _transparencyMode;

    public bool IsTransparencyEnabled { get => _isTransparencyEnabled; set => SetProperty(ref _isTransparencyEnabled, value); }
    private bool _isTransparencyEnabled;

    public bool OpaqueWhenFocused { get => _opaqueWhenFocused; set => SetPropertyAndSave(ref _opaqueWhenFocused, value); }
    private bool _opaqueWhenFocused;

    public double OpaqueOpacity { get => _opaqueOpacity; set => SetPropertyAndSave(ref _opaqueOpacity, value); }
    private double _opaqueOpacity;

    public double TransparentOpacity { get => _transparentOpacity; set => SetPropertyAndSave(ref _transparentOpacity, value); }
    private double _transparentOpacity;

    public ColorModes ColorMode { get => _colorMode; set => SetPropertyAndSave(ref _colorMode, value); }
    private ColorModes _colorMode;

    public bool UseMonoFont { get => _useMonoFont; set => SetPropertyAndSave(ref _useMonoFont, value); }
    private bool _useMonoFont;

    public bool HideTitleBar { get => _hideTitleBar; set => SetPropertyAndSave(ref _hideTitleBar, value); }
    private bool _hideTitleBar;

    public bool ShowTrayIcon { get => _showTrayIcon; set => SetPropertyAndSave(ref _showTrayIcon, value); }
    private bool _showTrayIcon;

    public bool ShowNotesInTaskbar { get => _showNotesInTaskbar; set => SetPropertyAndSave(ref _showNotesInTaskbar, value); }
    private bool _showNotesInTaskbar;

    public bool CheckForUpdates { get => _checkForUpdates; set => SetPropertyAndSave(ref _checkForUpdates, value); }
    private bool _checkForUpdates;

    #region Tools

    #region Base64

    public bool Base64ToolEnabled { get => _base64ToolEnabled; set => SetPropertyAndSave(ref _base64ToolEnabled, value, true); }
    private bool _base64ToolEnabled;

    public bool Base64ToolFavourite { get => _base64ToolFavourite; set => SetPropertyAndSave(ref _base64ToolFavourite, value, true); }
    private bool _base64ToolFavourite;

    #endregion

    #region Bracket

    public bool BracketToolEnabled { get => _bracketToolEnabled; set => SetPropertyAndSave(ref _bracketToolEnabled, value, true); }
    private bool _bracketToolEnabled;

    public bool BracketToolFavourite { get => _bracketToolFavourite; set => SetPropertyAndSave(ref _bracketToolFavourite, value, true); }
    private bool _bracketToolFavourite;

    #endregion

    #region Case

    public bool CaseToolEnabled { get => _caseToolEnabled; set => SetPropertyAndSave(ref _caseToolEnabled, value, true); }
    private bool _caseToolEnabled;

    public bool CaseToolFavourite { get => _caseToolFavourite; set => SetPropertyAndSave(ref _caseToolFavourite, value, true); }
    private bool _caseToolFavourite;

    #endregion

    #region DateTime

    public bool DateTimeToolEnabled { get => _dateTimeToolEnabled; set => SetPropertyAndSave(ref _dateTimeToolEnabled, value, true); }
    private bool _dateTimeToolEnabled;

    public bool DateTimeToolFavourite { get => _dateTimeToolFavourite; set => SetPropertyAndSave(ref _dateTimeToolFavourite, value, true); }
    private bool _dateTimeToolFavourite;

    #endregion

    #region Gibberish

    public bool GibberishToolEnabled { get => _gibberishToolEnabled; set => SetPropertyAndSave(ref _gibberishToolEnabled, value, true); }
    private bool _gibberishToolEnabled;

    public bool GibberishToolFavourite { get => _gibberishToolFavourite; set => SetPropertyAndSave(ref _gibberishToolFavourite, value, true); }
    private bool _gibberishToolFavourite;

    #endregion

    #region Hash

    public bool HashToolEnabled { get => _hashToolEnabled; set => SetPropertyAndSave(ref _hashToolEnabled, value, true); }
    private bool _hashToolEnabled;

    public bool HashToolFavourite { get => _hashToolFavourite; set => SetPropertyAndSave(ref _hashToolFavourite, value, true); }
    private bool _hashToolFavourite;

    #endregion

    #region HTMLEntity

    public bool HtmlEntityToolEnabled { get => _htmlEntityToolEnabled; set => SetPropertyAndSave(ref _htmlEntityToolEnabled, value, true); }
    private bool _htmlEntityToolEnabled;

    public bool HtmlEntityToolFavourite { get => _htmlEntityToolFavourite; set => SetPropertyAndSave(ref _htmlEntityToolFavourite, value, true); }
    private bool _htmlEntityToolFavourite;

    #endregion

    #region Indent

    public bool IndentToolEnabled { get => _indentToolEnabled; set => SetPropertyAndSave(ref _indentToolEnabled, value, true); }
    private bool _indentToolEnabled;

    public bool IndentToolFavourite { get => _indentToolFavourite; set => SetPropertyAndSave(ref _indentToolFavourite, value, true); }
    private bool _indentToolFavourite;

    #endregion

    #region Join

    public bool JoinToolEnabled { get => _joinToolEnabled; set => SetPropertyAndSave(ref _joinToolEnabled, value, true); }
    private bool _joinToolEnabled;

    public bool JoinToolFavourite { get => _joinToolFavourite; set => SetPropertyAndSave(ref _joinToolFavourite, value, true); }
    private bool _joinToolFavourite;

    #endregion

    #region JSON

    public bool JsonToolEnabled { get => _jsonToolEnabled; set => SetPropertyAndSave(ref _jsonToolEnabled, value, true); }
    private bool _jsonToolEnabled;

    public bool JsonToolFavourite { get => _jsonToolFavourite; set => SetPropertyAndSave(ref _jsonToolFavourite, value, true); }
    private bool _jsonToolFavourite;

    #endregion

    #region List

    public bool ListToolEnabled { get => _listToolEnabled; set => SetPropertyAndSave(ref _listToolEnabled, value, true); }
    private bool _listToolEnabled;

    public bool ListToolFavourite { get => _listToolFavourite; set => SetPropertyAndSave(ref _listToolFavourite, value, true); }
    private bool _listToolFavourite;

    #endregion

    #region Quote

    public bool QuoteToolEnabled { get => _quoteToolEnabled; set => SetPropertyAndSave(ref _quoteToolEnabled, value, true); }
    private bool _quoteToolEnabled;

    public bool QuoteToolFavourite { get => _quoteToolFavourite; set => SetPropertyAndSave(ref _quoteToolFavourite, value, true); }
    private bool _quoteToolFavourite;

    #endregion

    #region Remove

    public bool RemoveToolEnabled { get => _removeToolEnabled; set => SetPropertyAndSave(ref _removeToolEnabled, value, true); }
    private bool _removeToolEnabled;

    public bool RemoveToolFavourite { get => _removeToolFavourite; set => SetPropertyAndSave(ref _removeToolFavourite, value, true); }
    private bool _removeToolFavourite;

    #endregion

    #region Slash

    public bool SlashToolEnabled { get => _slashToolEnabled; set => SetPropertyAndSave(ref _slashToolEnabled, value, true); }
    private bool _slashToolEnabled;

    public bool SlashToolFavourite { get => _slashToolFavourite; set => SetPropertyAndSave(ref _slashToolFavourite, value, true); }
    private bool _slashToolFavourite;

    #endregion

    #region Sort

    public bool SortToolEnabled { get => _sortToolEnabled; set => SetPropertyAndSave(ref _sortToolEnabled, value, true); }
    private bool _sortToolEnabled;

    public bool SortToolFavourite { get => _sortToolFavourite; set => SetPropertyAndSave(ref _sortToolFavourite, value, true); }
    private bool _sortToolFavourite;

    #endregion

    #region Split

    public bool SplitToolEnabled { get => _splitToolEnabled; set => SetPropertyAndSave(ref _splitToolEnabled, value, true); }
    private bool _splitToolEnabled;

    public bool SplitToolFavourite { get => _splitToolFavourite; set => SetPropertyAndSave(ref _splitToolFavourite, value, true); }
    private bool _splitToolFavourite;

    #endregion

    #region Trim

    public bool TrimToolEnabled { get => _trimToolEnabled; set => SetPropertyAndSave(ref _trimToolEnabled, value, true); }
    private bool _trimToolEnabled;

    public bool TrimToolFavourite { get => _trimToolFavourite; set => SetPropertyAndSave(ref _trimToolFavourite, value, true); }
    private bool _trimToolFavourite;

    #endregion

    #endregion
}
