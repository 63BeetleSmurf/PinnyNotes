using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Collections.Generic;
using System.Windows.Controls;

using Pinny_Notes.Enums;
using Pinny_Notes.Properties;

namespace Pinny_Notes.ViewModels;

public partial class SettingsViewModel : ObservableRecipient
{
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

    public SettingsViewModel()
    {
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
        _transparentNotes = Settings.Default.TransparentNotes;
        _opaqueWhenFocused = Settings.Default.OpaqueWhenFocused;
        _onlyTransparentWhenPinned = Settings.Default.OnlyTransparentWhenPinned;
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

    private void UpdateSetting(string settingName, object oldValue, object newValue)
    {
        Settings.Default[settingName] = newValue;
        Settings.Default.Save();

        Messenger.Send(new PropertyChangedMessage<object>(this, settingName, oldValue, newValue));
    }

    private void UpdateToolSetting(string settingName, object oldValue, object newValue)
    {
        ToolSettings.Default[settingName] = newValue;
        ToolSettings.Default.Save();
    }

    [ObservableProperty]
    private StartupPositions _startupPosition;
    partial void OnStartupPositionChanged(StartupPositions oldValue, StartupPositions newValue) =>
        UpdateSetting(nameof(StartupPosition), (int)oldValue, (int)newValue);

    [ObservableProperty]
    private bool _cycleColors;
    partial void OnCycleColorsChanged(bool oldValue, bool newValue) =>
        UpdateSetting(nameof(CycleColors), oldValue, newValue);

    [ObservableProperty]
    private bool _trimCopiedText;
    partial void OnTrimCopiedTextChanged(bool oldValue, bool newValue) =>
        UpdateSetting(nameof(TrimCopiedText), oldValue, newValue);

    [ObservableProperty]
    private bool _trimPastedText;
    partial void OnTrimPastedTextChanged(bool oldValue, bool newValue) =>
        UpdateSetting(nameof(TrimPastedText), oldValue, newValue);

    [ObservableProperty]
    private bool _middleClickPaste;
    partial void OnMiddleClickPasteChanged(bool oldValue, bool newValue) =>
        UpdateSetting(nameof(MiddleClickPaste), oldValue, newValue);

    [ObservableProperty]
    private bool _autoCopy;
    partial void OnAutoCopyChanged(bool oldValue, bool newValue) =>
        UpdateSetting(nameof(AutoCopy), oldValue, newValue);

    [ObservableProperty]
    private bool _spellChecker;
    partial void OnSpellCheckerChanged(bool oldValue, bool newValue) =>
        UpdateSetting(nameof(SpellCheck), oldValue, newValue);

    [ObservableProperty]
    private bool _newLineAtEnd;
    partial void OnNewLineAtEndChanged(bool oldValue, bool newValue) =>
        UpdateSetting(nameof(NewLineAtEnd), oldValue, newValue);

    [ObservableProperty]
    private bool _keepNewLineAtEndVisible;
    partial void OnKeepNewLineAtEndVisibleChanged(bool oldValue, bool newValue) =>
        UpdateSetting(nameof(KeepNewLineAtEndVisible), oldValue, newValue);

    [ObservableProperty]
    private bool _autoIndent;
    partial void OnAutoIndentChanged(bool oldValue, bool newValue) =>
        UpdateSetting(nameof(AutoIndent), oldValue, newValue);

    [ObservableProperty]
    private bool _tabSpaces;
    partial void OnTabSpacesChanged(bool oldValue, bool newValue) =>
        UpdateSetting(nameof(TabSpaces), oldValue, newValue);

    [ObservableProperty]
    private int _tabWidth;
    partial void OnTabWidthChanged(int oldValue, int newValue) =>
        UpdateSetting(nameof(TabWidth), oldValue, newValue);

    [ObservableProperty]
    private bool _convertIndentation;
    partial void OnConvertIndentationChanged(bool oldValue, bool newValue) =>
        UpdateSetting(nameof(ConvertIndentation), oldValue, newValue);

    [ObservableProperty]
    private MinimizeModes _minimizeMode;
    partial void OnMinimizeModeChanged(MinimizeModes oldValue, MinimizeModes newValue) =>
        UpdateSetting(nameof(MinimizeMode), (int)oldValue, (int)newValue);

    [ObservableProperty]
    private bool _transparentNotes;
    partial void OnTransparentNotesChanged(bool oldValue, bool newValue) =>
        UpdateSetting(nameof(TransparentNotes), oldValue, newValue);

    [ObservableProperty]
    private bool _opaqueWhenFocused;
    partial void OnOpaqueWhenFocusedChanged(bool oldValue, bool newValue) =>
        UpdateSetting(nameof(OpaqueWhenFocused), oldValue, newValue);

    [ObservableProperty]
    private bool _onlyTransparentWhenPinned;
    partial void OnOnlyTransparentWhenPinnedChanged(bool oldValue, bool newValue) =>
        UpdateSetting(nameof(OnlyTransparentWhenPinned), oldValue, newValue);

    [ObservableProperty]
    private ColorModes _colorMode;
    partial void OnColorModeChanged(ColorModes oldValue, ColorModes newValue) =>
        UpdateSetting(nameof(ColorMode), (int)oldValue, (int)newValue);

    [ObservableProperty]
    private bool _useMonoFont;
    partial void OnUseMonoFontChanged(bool oldValue, bool newValue) =>
        UpdateSetting(nameof(UseMonoFont), oldValue, newValue);

    [ObservableProperty]
    private bool _hideTitleBar;
    partial void OnHideTitleBarChanged(bool oldValue, bool newValue) =>
        UpdateSetting(nameof(HideTitleBar), oldValue, newValue);

    [ObservableProperty]
    private bool _showTrayIcon;
    partial void OnShowTrayIconChanged(bool oldValue, bool newValue) =>
        UpdateSetting(nameof(ShowTrayIcon), oldValue, newValue);

    [ObservableProperty]
    private bool _showNotesInTaskbar;
    partial void OnShowNotesInTaskbarChanged(bool oldValue, bool newValue) =>
        UpdateSetting(nameof(ShowNotesInTaskbar), oldValue, newValue);

    [ObservableProperty]
    private bool _checkForUpdates;
    partial void OnCheckForUpdatesChanged(bool oldValue, bool newValue) =>
        UpdateSetting(nameof(CheckForUpdates), oldValue, newValue);

    #region Tools

    #region Base64

    [ObservableProperty]
    private bool _base64ToolEnabled;
    partial void OnBase64ToolEnabledChanged(bool oldValue, bool newValue) =>
        UpdateToolSetting(nameof(Base64ToolEnabled), oldValue, newValue);

    [ObservableProperty]
    private bool _base64ToolFavourite;
    partial void OnBase64ToolFavouriteChanged(bool oldValue, bool newValue) =>
        UpdateToolSetting(nameof(Base64ToolFavourite), oldValue, newValue);

    #endregion

    #region Bracket

    [ObservableProperty]
    private bool _bracketToolEnabled;
    partial void OnBracketToolEnabledChanged(bool oldValue, bool newValue) =>
        UpdateToolSetting(nameof(BracketToolEnabled), oldValue, newValue);

    [ObservableProperty]
    private bool _bracketToolFavourite;
    partial void OnBracketToolFavouriteChanged(bool oldValue, bool newValue) =>
        UpdateToolSetting(nameof(BracketToolFavourite), oldValue, newValue);

    #endregion

    #region Case

    [ObservableProperty]
    private bool _caseToolEnabled;
    partial void OnCaseToolEnabledChanged(bool oldValue, bool newValue) =>
        UpdateToolSetting(nameof(CaseToolEnabled), oldValue, newValue);

    [ObservableProperty]
    private bool _caseToolFavourite;
    partial void OnCaseToolFavouriteChanged(bool oldValue, bool newValue) =>
        UpdateToolSetting(nameof(CaseToolFavourite), oldValue, newValue);

    #endregion

    #region DateTime

    [ObservableProperty]
    private bool _dateTimeToolEnabled;
    partial void OnDateTimeToolEnabledChanged(bool oldValue, bool newValue) =>
        UpdateToolSetting(nameof(DateTimeToolEnabled), oldValue, newValue);

    [ObservableProperty]
    private bool _dateTimeToolFavourite;
    partial void OnDateTimeToolFavouriteChanged(bool oldValue, bool newValue) =>
        UpdateToolSetting(nameof(DateTimeToolFavourite), oldValue, newValue);

    #endregion

    #region Gibberish

    [ObservableProperty]
    private bool _gibberishToolEnabled;
    partial void OnGibberishToolEnabledChanged(bool oldValue, bool newValue) =>
        UpdateToolSetting(nameof(GibberishToolEnabled), oldValue, newValue);

    [ObservableProperty]
    private bool _gibberishToolFavourite;
    partial void OnGibberishToolFavouriteChanged(bool oldValue, bool newValue) =>
        UpdateToolSetting(nameof(GibberishToolFavourite), oldValue, newValue);

    #endregion

    #region Hash

    [ObservableProperty]
    private bool _hashToolEnabled;
    partial void OnHashToolEnabledChanged(bool oldValue, bool newValue) =>
        UpdateToolSetting(nameof(HashToolEnabled), oldValue, newValue);

    [ObservableProperty]
    private bool _hashToolFavourite;
    partial void OnHashToolFavouriteChanged(bool oldValue, bool newValue) =>
        UpdateToolSetting(nameof(HashToolFavourite), oldValue, newValue);

    #endregion

    #region HTMLEntity

    [ObservableProperty]
    private bool _htmlEntityToolEnabled;
    partial void OnHtmlEntityToolEnabledChanged(bool oldValue, bool newValue) =>
        UpdateToolSetting(nameof(HtmlEntityToolEnabled), oldValue, newValue);

    [ObservableProperty]
    private bool _htmlEntityToolFavourite;
    partial void OnHtmlEntityToolFavouriteChanged(bool oldValue, bool newValue) =>
        UpdateToolSetting(nameof(HtmlEntityToolFavourite), oldValue, newValue);

    #endregion

    #region Indent

    [ObservableProperty]
    private bool _indentToolEnabled;
    partial void OnIndentToolEnabledChanged(bool oldValue, bool newValue) =>
        UpdateToolSetting(nameof(IndentToolEnabled), oldValue, newValue);

    [ObservableProperty]
    private bool _indentToolFavourite;
    partial void OnIndentToolFavouriteChanged(bool oldValue, bool newValue) =>
        UpdateToolSetting(nameof(IndentToolFavourite), oldValue, newValue);

    #endregion

    #region Join

    [ObservableProperty]
    private bool _joinToolEnabled;
    partial void OnJoinToolEnabledChanged(bool oldValue, bool newValue) =>
        UpdateToolSetting(nameof(JoinToolEnabled), oldValue, newValue);

    [ObservableProperty]
    private bool _joinToolFavourite;
    partial void OnJoinToolFavouriteChanged(bool oldValue, bool newValue) =>
        UpdateToolSetting(nameof(JoinToolFavourite), oldValue, newValue);

    #endregion

    #region JSON

    [ObservableProperty]
    private bool _jsonToolEnabled;
    partial void OnJsonToolEnabledChanged(bool oldValue, bool newValue) =>
        UpdateToolSetting(nameof(JsonToolEnabled), oldValue, newValue);

    [ObservableProperty]
    private bool _jsonToolFavourite;
    partial void OnJsonToolFavouriteChanged(bool oldValue, bool newValue) =>
        UpdateToolSetting(nameof(JsonToolFavourite), oldValue, newValue);

    #endregion

    #region List

    [ObservableProperty]
    private bool _listToolEnabled;
    partial void OnListToolEnabledChanged(bool oldValue, bool newValue) =>
        UpdateToolSetting(nameof(ListToolEnabled), oldValue, newValue);

    [ObservableProperty]
    private bool _listToolFavourite;
    partial void OnListToolFavouriteChanged(bool oldValue, bool newValue) =>
        UpdateToolSetting(nameof(ListToolFavourite), oldValue, newValue);

    #endregion

    #region Quote

    [ObservableProperty]
    private bool _quoteToolEnabled;
    partial void OnQuoteToolEnabledChanged(bool oldValue, bool newValue) =>
        UpdateToolSetting(nameof(QuoteToolEnabled), oldValue, newValue);

    [ObservableProperty]
    private bool _quoteToolFavourite;
    partial void OnQuoteToolFavouriteChanged(bool oldValue, bool newValue) =>
        UpdateToolSetting(nameof(QuoteToolFavourite), oldValue, newValue);

    #endregion

    #region Remove

    [ObservableProperty]
    private bool _removeToolEnabled;
    partial void OnRemoveToolEnabledChanged(bool oldValue, bool newValue) =>
        UpdateToolSetting(nameof(RemoveToolEnabled), oldValue, newValue);

    [ObservableProperty]
    private bool _removeToolFavourite;
    partial void OnRemoveToolFavouriteChanged(bool oldValue, bool newValue) =>
        UpdateToolSetting(nameof(RemoveToolFavourite), oldValue, newValue);

    #endregion

    #region Slash

    [ObservableProperty]
    private bool _slashToolEnabled;
    partial void OnSlashToolEnabledChanged(bool oldValue, bool newValue) =>
        UpdateToolSetting(nameof(SlashToolEnabled), oldValue, newValue);

    [ObservableProperty]
    private bool _slashToolFavourite;
    partial void OnSlashToolFavouriteChanged(bool oldValue, bool newValue) =>
        UpdateToolSetting(nameof(SlashToolFavourite), oldValue, newValue);

    #endregion

    #region Sort

    [ObservableProperty]
    private bool _sortToolEnabled;
    partial void OnSortToolEnabledChanged(bool oldValue, bool newValue) =>
        UpdateToolSetting(nameof(SortToolEnabled), oldValue, newValue);

    [ObservableProperty]
    private bool _sortToolFavourite;
    partial void OnSortToolFavouriteChanged(bool oldValue, bool newValue) =>
        UpdateToolSetting(nameof(SortToolFavourite), oldValue, newValue);

    #endregion

    #region Split

    [ObservableProperty]
    private bool _splitToolEnabled;
    partial void OnSplitToolEnabledChanged(bool oldValue, bool newValue) =>
        UpdateToolSetting(nameof(SplitToolEnabled), oldValue, newValue);

    [ObservableProperty]
    private bool _splitToolFavourite;
    partial void OnSplitToolFavouriteChanged(bool oldValue, bool newValue) =>
        UpdateToolSetting(nameof(SplitToolFavourite), oldValue, newValue);

    #endregion

    #region Trim

    [ObservableProperty]
    private bool _trimToolEnabled;
    partial void OnTrimToolEnabledChanged(bool oldValue, bool newValue) =>
        UpdateToolSetting(nameof(TrimToolEnabled), oldValue, newValue);

    [ObservableProperty]
    private bool _trimToolFavourite;
    partial void OnTrimToolFavouriteChanged(bool oldValue, bool newValue) =>
        UpdateToolSetting(nameof(TrimToolFavourite), oldValue, newValue);

    #endregion

    #endregion
}
