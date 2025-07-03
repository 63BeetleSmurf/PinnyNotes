using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Messages;
using PinnyNotes.WpfUi.Properties;
using PinnyNotes.WpfUi.Services;

namespace PinnyNotes.WpfUi.ViewModels;

public class SettingsViewModel : BaseViewModel
{
    private readonly MessengerService _messenger = App.Services.GetRequiredService<MessengerService>();

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
        new(TransparencyModes.WhenPinned, "Only when pinned")
    ];

    private static readonly KeyValuePair<string, string>[] _fontFamilyList
        = new InstalledFontCollection().Families
                                       .Select(f => new KeyValuePair<string, string>(f.Name, f.Name))
                                       .ToArray();

    private static readonly KeyValuePair<CopyFallbackActions, string>[] _copyFallbackActionList = [
        new(CopyFallbackActions.None, "None"),
        new(CopyFallbackActions.CopyLine, "Copy line"),
        new(CopyFallbackActions.CopyNote, "Copy note")
    ];

    private static readonly KeyValuePair<ToolStates, string>[] _toolStateList = [
        new(ToolStates.Disabled, "Disabled"),
        new(ToolStates.Enabled, "Enabled"),
        new(ToolStates.Favourite, "Favourite")
    ];

    public SettingsViewModel()
    {
        _defaultNoteHeight = Settings.Default.DefaultNoteHeight;
        _defaultNoteWidth = Settings.Default.DefaultNoteWidth;
        _startupPosition = (StartupPositions)Settings.Default.StartupPosition;
        _cycleColors = Settings.Default.CycleColors;
        _trimCopiedText = Settings.Default.TrimCopiedText;
        _trimPastedText = Settings.Default.TrimPastedText;
        _middleClickPaste = Settings.Default.MiddleClickPaste;
        _autoCopy = Settings.Default.AutoCopy;
        _copyFallbackAction = (CopyFallbackActions)Settings.Default.CopyFallbackAction;
        _spellChecker = Settings.Default.SpellCheck;
        _newLineAtEnd = Settings.Default.NewLineAtEnd;
        _keepNewLineAtEndVisible = Settings.Default.KeepNewLineAtEndVisible;
        _wrapText = (TextWrapping)Settings.Default.WrapText;
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
        _standardFontFamily = Settings.Default.StandardFontFamily;
        _monoFontFamily = Settings.Default.MonoFontFamily;
        _useMonoFont = Settings.Default.UseMonoFont;
        _hideTitleBar = Settings.Default.HideTitleBar;
        _showTrayIcon = Settings.Default.ShowTrayIcon;
        _showNotesInTaskbar = Settings.Default.ShowNotesInTaskbar;
        _checkForUpdates = Settings.Default.CheckForUpdates;

        #region Tools

        _base64ToolState = (ToolStates)Settings.Default.Base64ToolState;
        _bracketToolState = (ToolStates)Settings.Default.BracketToolState;
        _caseToolState = (ToolStates)Settings.Default.CaseToolState;
        _colorToolState = (ToolStates)Settings.Default.ColorToolState;
        _dateTimeToolState = (ToolStates)Settings.Default.DateTimeToolState;
        _gibberishToolState = (ToolStates)Settings.Default.GibberishToolState;
        _hashToolState = (ToolStates)Settings.Default.HashToolState;
        _htmlEntityToolState = (ToolStates)Settings.Default.HtmlEntityToolState;
        _indentToolState = (ToolStates)Settings.Default.IndentToolState;
        _joinToolState = (ToolStates)Settings.Default.JoinToolState;
        _jsonToolState = (ToolStates)Settings.Default.JsonToolState;
        _listToolState = (ToolStates)Settings.Default.ListToolState;
        _quoteToolState = (ToolStates)Settings.Default.QuoteToolState;
        _removeToolState = (ToolStates)Settings.Default.RemoveToolState;
        _slashToolState = (ToolStates)Settings.Default.SlashToolState;
        _sortToolState = (ToolStates)Settings.Default.SortToolState;
        _splitToolState = (ToolStates)Settings.Default.SplitToolState;
        _trimToolState = (ToolStates)Settings.Default.TrimToolState;

        #endregion
    }

    public KeyValuePair<StartupPositions, string>[] StartupPositionsList => _startupPositionsList;
    public KeyValuePair<MinimizeModes, string>[] MinimizeModeList => _minimizeModeList;
    public KeyValuePair<ColorModes, string>[] ColorModeList => _colorModeList;
    public KeyValuePair<TransparencyModes, string>[] TransparencyModeList => _transparencyModeList;
    public KeyValuePair<string, string>[] FontFamilyList => _fontFamilyList;
    public KeyValuePair<CopyFallbackActions, string>[] CopyFallbackActionList => _copyFallbackActionList;
    public KeyValuePair<ToolStates, string>[] ToolStateList => _toolStateList;

    private bool SetPropertyAndSave<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
    {
        if (propertyName == null || value == null)
            throw new ArgumentNullException();

        if (!SetProperty(ref storage, value, propertyName))
            return false;

        if (value is Enum)
            Settings.Default[propertyName] = Convert.ToInt32(value);
        else
            Settings.Default[propertyName] = value;

        Settings.Default.Save();

        _messenger.Publish(new SettingChangedMessage(propertyName, value));

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

    public CopyFallbackActions CopyFallbackAction { get => _copyFallbackAction; set => SetPropertyAndSave(ref _copyFallbackAction, value); }
    private CopyFallbackActions _copyFallbackAction;

    public bool SpellChecker { get => _spellChecker; set => SetPropertyAndSave(ref _spellChecker, value); }
    private bool _spellChecker;

    public bool NewLineAtEnd { get => _newLineAtEnd; set => SetPropertyAndSave(ref _newLineAtEnd, value); }
    private bool _newLineAtEnd;

    public bool KeepNewLineAtEndVisible { get => _keepNewLineAtEndVisible; set => SetPropertyAndSave(ref _keepNewLineAtEndVisible, value); }
    private bool _keepNewLineAtEndVisible;

    public bool WrapText { get => (_wrapText == TextWrapping.Wrap); set => SetPropertyAndSave(ref _wrapText, (value) ? TextWrapping.Wrap : TextWrapping.NoWrap); }
    private TextWrapping _wrapText;

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

    public string StandardFontFamily { get => _standardFontFamily; set => SetPropertyAndSave(ref _standardFontFamily, value); }
    private string _standardFontFamily;

    public string MonoFontFamily { get => _monoFontFamily; set => SetPropertyAndSave(ref _monoFontFamily, value); }
    private string _monoFontFamily;

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

    public ToolStates Base64ToolState { get => _base64ToolState; set => SetPropertyAndSave(ref _base64ToolState, value); }
    private ToolStates _base64ToolState;

    public ToolStates BracketToolState { get => _bracketToolState; set => SetPropertyAndSave(ref _bracketToolState, value); }
    private ToolStates _bracketToolState;

    public ToolStates CaseToolState { get => _caseToolState; set => SetPropertyAndSave(ref _caseToolState, value); }
    private ToolStates _caseToolState;

    public ToolStates ColorToolState { get => _colorToolState; set => SetPropertyAndSave(ref _colorToolState, value); }
    private ToolStates _colorToolState;

    public ToolStates DateTimeToolState { get => _dateTimeToolState; set => SetPropertyAndSave(ref _dateTimeToolState, value); }
    private ToolStates _dateTimeToolState;

    public ToolStates GibberishToolState { get => _gibberishToolState; set => SetPropertyAndSave(ref _gibberishToolState, value); }
    private ToolStates _gibberishToolState;

    public ToolStates HashToolState { get => _hashToolState; set => SetPropertyAndSave(ref _hashToolState, value); }
    private ToolStates _hashToolState;

    public ToolStates HtmlEntityToolState { get => _htmlEntityToolState; set => SetPropertyAndSave(ref _htmlEntityToolState, value); }
    private ToolStates _htmlEntityToolState;

    public ToolStates IndentToolState { get => _indentToolState; set => SetPropertyAndSave(ref _indentToolState, value); }
    private ToolStates _indentToolState;

    public ToolStates JoinToolState { get => _joinToolState; set => SetPropertyAndSave(ref _joinToolState, value); }
    private ToolStates _joinToolState;

    public ToolStates JsonToolState { get => _jsonToolState; set => SetPropertyAndSave(ref _jsonToolState, value); }
    private ToolStates _jsonToolState;

    public ToolStates ListToolState { get => _listToolState; set => SetPropertyAndSave(ref _listToolState, value); }
    private ToolStates _listToolState;

    public ToolStates QuoteToolState { get => _quoteToolState; set => SetPropertyAndSave(ref _quoteToolState, value); }
    private ToolStates _quoteToolState;

    public ToolStates RemoveToolState { get => _removeToolState; set => SetPropertyAndSave(ref _removeToolState, value); }
    private ToolStates _removeToolState;

    public ToolStates SlashToolState { get => _slashToolState; set => SetPropertyAndSave(ref _slashToolState, value); }
    private ToolStates _slashToolState;

    public ToolStates SortToolState { get => _sortToolState; set => SetPropertyAndSave(ref _sortToolState, value); }
    private ToolStates _sortToolState;

    public ToolStates SplitToolState { get => _splitToolState; set => SetPropertyAndSave(ref _splitToolState, value); }
    private ToolStates _splitToolState;

    public ToolStates TrimToolState { get => _trimToolState; set => SetPropertyAndSave(ref _trimToolState, value); }
    private ToolStates _trimToolState;

    #endregion
}
