using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Text;
using System.Linq;

using PinnyNotes.Core.Enums;
using PinnyNotes.WpfUi.Messages;
using PinnyNotes.WpfUi.Services;

namespace PinnyNotes.WpfUi.ViewModels;

public class SettingsViewModel : BaseViewModel, INotifyPropertyChanged
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

    public SettingsViewModel(
        AppMetadataService appMetadata, SettingsService settingsService, MessengerService messenger
        ) : base(appMetadata, settingsService, messenger)
    {
        _isTransparencyEnabled = (TransparencyMode != TransparencyModes.Disabled);
        _newLineAtEnd = Settings.EditorSettings.NewLineAtEnd;
    }

    public KeyValuePair<StartupPositions, string>[] StartupPositionsList => _startupPositionsList;
    public KeyValuePair<MinimizeModes, string>[] MinimizeModeList => _minimizeModeList;
    public KeyValuePair<ColorModes, string>[] ColorModeList => _colorModeList;
    public KeyValuePair<TransparencyModes, string>[] TransparencyModeList => _transparencyModeList;
    public KeyValuePair<string, string>[] FontFamilyList => _fontFamilyList;
    public KeyValuePair<CopyFallbackActions, string>[] CopyFallbackActionList => _copyFallbackActionList;
    public KeyValuePair<ToolStates, string>[] ToolStateList => _toolStateList;

    #region Application

    // General
    public bool CheckForUpdates
    {
        get => Settings.ApplicationSettings.CheckForUpdates;
        set
        {
            Settings.ApplicationSettings.CheckForUpdates = value;
            Messenger.Publish(new SettingChangedMessage(nameof(CheckForUpdates), value));
        }
    }

    public bool ShowTrayIcon
    {
        get => Settings.ApplicationSettings.ShowNotifiyIcon;
        set
        {
            Settings.ApplicationSettings.ShowNotifiyIcon = value;
            Messenger.Publish(new SettingChangedMessage(nameof(ShowTrayIcon), value));
        }
    }

    #endregion

    #region Notes

    // General
    public double DefaultNoteHeight
    {
        get => Settings.NoteSettings.DefaultHeight;
        set
        {
            Settings.NoteSettings.DefaultHeight = value;
            Messenger.Publish(new SettingChangedMessage(nameof(DefaultNoteHeight), value));
        }
    }

    public double DefaultNoteWidth
    {
        get => Settings.NoteSettings.DefaultWidth;
        set
        {
            Settings.NoteSettings.DefaultWidth = value;
            Messenger.Publish(new SettingChangedMessage(nameof(DefaultNoteWidth), value));
        }
    }

    public StartupPositions StartupPosition
    {
        get => Settings.NoteSettings.StartupPosition;
        set
        {
            Settings.NoteSettings.StartupPosition = value;
            Messenger.Publish(new SettingChangedMessage(nameof(StartupPosition), value));
        }
    }

    public MinimizeModes MinimizeMode
    {
        get => Settings.NoteSettings.MinimizeMode;
        set
        {
            Settings.NoteSettings.MinimizeMode = value;
            Messenger.Publish(new SettingChangedMessage(nameof(MinimizeMode), value));
        }
    }

    public bool HideTitleBar
    {
        get => Settings.NoteSettings.HideTitleBar;
        set
        {
            Settings.NoteSettings.HideTitleBar = value;
            Messenger.Publish(new SettingChangedMessage(nameof(HideTitleBar), value));
        }
    }

    public bool ShowNotesInTaskbar
    {
        get => Settings.NoteSettings.ShowInTaskBar;
        set
        {
            Settings.NoteSettings.ShowInTaskBar = value;
            Messenger.Publish(new SettingChangedMessage(nameof(ShowNotesInTaskbar), value));
        }
    }

    // Theme
    public bool CycleColors
    {
        get => Settings.NoteSettings.CycleColors;
        set
        {
            Settings.NoteSettings.CycleColors = value;
            Messenger.Publish(new SettingChangedMessage(nameof(CycleColors), value));
        }
    }

    public ColorModes ColorMode
    {
        get => Settings.NoteSettings.ColorMode;
        set
        {
            Settings.NoteSettings.ColorMode = value;
            Messenger.Publish(new SettingChangedMessage(nameof(ColorMode), value));
        }
    }

    // Transparency
    public TransparencyModes TransparencyMode
    {
        get => Settings.NoteSettings.TransparencyMode;
        set
        {
            Settings.NoteSettings.TransparencyMode = value;
            IsTransparencyEnabled = (TransparencyMode != TransparencyModes.Disabled);
            Messenger.Publish(new SettingChangedMessage(nameof(TransparencyMode), value));
        }
    }

    public bool IsTransparencyEnabled
    {
        get => _isTransparencyEnabled;
        set => SetProperty(ref _isTransparencyEnabled, value);
    }
    private bool _isTransparencyEnabled;

    public bool OpaqueWhenFocused
    {
        get => Settings.NoteSettings.OpaqueWhenFocused;
        set
        {
            Settings.NoteSettings.OpaqueWhenFocused = value;
            Messenger.Publish(new SettingChangedMessage(nameof(OpaqueWhenFocused), value));
        }
    }

    public double OpaqueOpacity
    {
        get => Settings.NoteSettings.OpaqueValue;
        set
        {
            Settings.NoteSettings.OpaqueValue = value;
            Messenger.Publish(new SettingChangedMessage(nameof(OpaqueOpacity), value));
        }
    }

    public double TransparentOpacity
    {
        get => Settings.NoteSettings.TransparentValue;
        set
        {
            Settings.NoteSettings.TransparentValue = value;
            Messenger.Publish(new SettingChangedMessage(nameof(TransparentOpacity), value));
        }
    }

    #endregion

    #region Editor

    // General
    public bool SpellChecker
    {
        get => Settings.EditorSettings.CheckSpelling;
        set
        {
            Settings.EditorSettings.CheckSpelling = value;
            Messenger.Publish(new SettingChangedMessage(nameof(SpellChecker), value));
        }
    }

    public bool NewLineAtEnd
    {
        get => _newLineAtEnd;
        set
        {
            SetProperty(ref _newLineAtEnd, value);
            Settings.EditorSettings.NewLineAtEnd = value;
            Messenger.Publish(new SettingChangedMessage(nameof(NewLineAtEnd), value));
        }
    }
    private bool _newLineAtEnd;

    public bool KeepNewLineAtEndVisible
    {
        get => Settings.EditorSettings.KeepNewLineVisible;
        set
        {
            Settings.EditorSettings.KeepNewLineVisible = value;
            Messenger.Publish(new SettingChangedMessage(nameof(KeepNewLineAtEndVisible), value));
        }
    }

    public bool WrapText
    {
        get => Settings.EditorSettings.WrapText;
        set
        {
            Settings.EditorSettings.WrapText = value;
            Messenger.Publish(new SettingChangedMessage(nameof(WrapText), value));
        }
    }

    // Fonts
    public string StandardFontFamily
    {
        get => Settings.EditorSettings.StandardFontFamily;
        set
        {
            Settings.EditorSettings.StandardFontFamily = value;
            Messenger.Publish(new SettingChangedMessage(nameof(StandardFontFamily), value));
        }
    }

    public string MonoFontFamily
    {
        get => Settings.EditorSettings.MonoFontFamily;
        set
        {
            Settings.EditorSettings.MonoFontFamily = value;
            Messenger.Publish(new SettingChangedMessage(nameof(MonoFontFamily), value));
        }
    }

    public bool UseMonoFont
    {
        get => Settings.EditorSettings.UseMonoFont;
        set
        {
            Settings.EditorSettings.UseMonoFont = value;
            Messenger.Publish(new SettingChangedMessage(nameof(UseMonoFont), value));
        }
    }

    // Indentation
    public bool AutoIndent
    {
        get => Settings.EditorSettings.AutoIndent;
        set
        {
            Settings.EditorSettings.AutoIndent = value;
            Messenger.Publish(new SettingChangedMessage(nameof(AutoIndent), value));
        }
    }

    public bool TabSpaces
    {
        get => Settings.EditorSettings.UseSpacesForTab;
        set
        {
            Settings.EditorSettings.UseSpacesForTab = value;
            Messenger.Publish(new SettingChangedMessage(nameof(TabSpaces), value));
        }
    }

    public int TabWidth
    {
        get => Settings.EditorSettings.TabSpacesWidth;
        set
        {
            Settings.EditorSettings.TabSpacesWidth = value;
            Messenger.Publish(new SettingChangedMessage(nameof(TabWidth), value));
        }
    }

    public bool ConvertIndentation
    {
        get => Settings.EditorSettings.ConvertIndentationOnPaste;
        set
        {
            Settings.EditorSettings.ConvertIndentationOnPaste = value;
            Messenger.Publish(new SettingChangedMessage(nameof(ConvertIndentation), value));
        }
    }

    // Copy and Paste
    public bool MiddleClickPaste
    {
        get => Settings.EditorSettings.MiddleClickPaste;
        set
        {
            Settings.EditorSettings.MiddleClickPaste = value;
            Messenger.Publish(new SettingChangedMessage(nameof(MiddleClickPaste), value));
        }
    }

    public bool TrimPastedText
    {
        get => Settings.EditorSettings.TrimPastedText;
        set
        {
            Settings.EditorSettings.TrimPastedText = value;
            Messenger.Publish(new SettingChangedMessage(nameof(TrimPastedText), value));
        }
    }

    public bool TrimCopiedText
    {
        get => Settings.EditorSettings.TrimCopiedText;
        set
        {
            Settings.EditorSettings.TrimCopiedText = value;
            Messenger.Publish(new SettingChangedMessage(nameof(TrimCopiedText), value));
        }
    }

    public bool AutoCopy
    {
        get => Settings.EditorSettings.CopyOnSelect;
        set
        {
            Settings.EditorSettings.CopyOnSelect = value;
            Messenger.Publish(new SettingChangedMessage(nameof(AutoCopy), value));
        }
    }

    public CopyFallbackActions CopyFallbackAction
    {
        get => Settings.EditorSettings.CopyFallbackAction;
        set
        {
            Settings.EditorSettings.CopyFallbackAction = value;
            Messenger.Publish(new SettingChangedMessage(nameof(CopyFallbackAction), value));
        }
    }

    #endregion

    #region Tools

    public ToolStates Base64ToolState
    {
        get => Settings.ToolSettings.Base64ToolState;
        set
        {
            Settings.ToolSettings.Base64ToolState = value;
            Messenger.Publish(new SettingChangedMessage(nameof(Base64ToolState), value));
        }
    }

    public ToolStates BracketToolState
    {
        get => Settings.ToolSettings.BracketToolState;
        set
        {
            Settings.ToolSettings.BracketToolState = value;
            Messenger.Publish(new SettingChangedMessage(nameof(BracketToolState), value));
        }
    }

    public ToolStates CaseToolState
    {
        get => Settings.ToolSettings.CaseToolState;
        set
        {
            Settings.ToolSettings.CaseToolState = value;
            Messenger.Publish(new SettingChangedMessage(nameof(CaseToolState), value));
        }
    }

    public ToolStates ColorToolState
    {
        get => Settings.ToolSettings.ColorToolState;
        set
        {
            Settings.ToolSettings.ColorToolState = value;
            Messenger.Publish(new SettingChangedMessage(nameof(ColorToolState), value));
        }
    }

    public ToolStates DateTimeToolState
    {
        get => Settings.ToolSettings.DateTimeToolState;
        set
        {
            Settings.ToolSettings.DateTimeToolState = value;
            Messenger.Publish(new SettingChangedMessage(nameof(DateTimeToolState), value));
        }
    }

    public ToolStates GibberishToolState
    {
        get => Settings.ToolSettings.GibberishToolState;
        set
        {
            Settings.ToolSettings.GibberishToolState = value;
            Messenger.Publish(new SettingChangedMessage(nameof(GibberishToolState), value));
        }
    }

    public ToolStates HashToolState
    {
        get => Settings.ToolSettings.HashToolState;
        set
        {
            Settings.ToolSettings.HashToolState = value;
            Messenger.Publish(new SettingChangedMessage(nameof(HashToolState), value));
        }
    }

    public ToolStates HtmlEntityToolState
    {
        get => Settings.ToolSettings.HtmlEntityToolState;
        set
        {
            Settings.ToolSettings.HtmlEntityToolState = value;
            Messenger.Publish(new SettingChangedMessage(nameof(HtmlEntityToolState), value));
        }
    }

    public ToolStates IndentToolState
    {
        get => Settings.ToolSettings.IndentToolState;
        set
        {
            Settings.ToolSettings.IndentToolState = value;
            Messenger.Publish(new SettingChangedMessage(nameof(IndentToolState), value));
        }
    }

    public ToolStates JoinToolState
    {
        get => Settings.ToolSettings.JoinToolState;
        set
        {
            Settings.ToolSettings.JoinToolState = value;
            Messenger.Publish(new SettingChangedMessage(nameof(JoinToolState), value));
        }
    }

    public ToolStates JsonToolState
    {
        get => Settings.ToolSettings.JsonToolState;
        set
        {
            Settings.ToolSettings.JsonToolState = value;
            Messenger.Publish(new SettingChangedMessage(nameof(JsonToolState), value));
        }
    }

    public ToolStates ListToolState
    {
        get => Settings.ToolSettings.ListToolState;
        set
        {
            Settings.ToolSettings.ListToolState = value;
            Messenger.Publish(new SettingChangedMessage(nameof(ListToolState), value));
        }
    }

    public ToolStates QuoteToolState
    {
        get => Settings.ToolSettings.QuoteToolState;
        set
        {
            Settings.ToolSettings.QuoteToolState = value;
            Messenger.Publish(new SettingChangedMessage(nameof(QuoteToolState), value));
        }
    }

    public ToolStates RemoveToolState
    {
        get => Settings.ToolSettings.RemoveToolState;
        set
        {
            Settings.ToolSettings.RemoveToolState = value;
            Messenger.Publish(new SettingChangedMessage(nameof(RemoveToolState), value));
        }
    }

    public ToolStates SlashToolState
    {
        get => Settings.ToolSettings.SlashToolState;
        set
        {
            Settings.ToolSettings.SlashToolState = value;
            Messenger.Publish(new SettingChangedMessage(nameof(SlashToolState), value));
        }
    }

    public ToolStates SortToolState
    {
        get => Settings.ToolSettings.SortToolState;
        set
        {
            Settings.ToolSettings.SortToolState = value;
            Messenger.Publish(new SettingChangedMessage(nameof(SortToolState), value));
        }
    }

    public ToolStates SplitToolState
    {
        get => Settings.ToolSettings.SplitToolState;
        set
        {
            Settings.ToolSettings.SplitToolState = value;
            Messenger.Publish(new SettingChangedMessage(nameof(SplitToolState), value));
        }
    }

    public ToolStates TrimToolState
    {
        get => Settings.ToolSettings.TrimToolState;
        set
        {
            Settings.ToolSettings.TrimToolState = value;
            Messenger.Publish(new SettingChangedMessage(nameof(TrimToolState), value));
        }
    }

    #endregion
}
