using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;

using PinnyNotes.Core.Enums;
using PinnyNotes.WpfUi.Messages;
using PinnyNotes.WpfUi.Services;

namespace PinnyNotes.WpfUi.ViewModels;

public class SettingsViewModel(SettingsService settingsService, MessengerService messenger) : BaseViewModel(settingsService, messenger)
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

    public KeyValuePair<StartupPositions, string>[] StartupPositionsList => _startupPositionsList;
    public KeyValuePair<MinimizeModes, string>[] MinimizeModeList => _minimizeModeList;
    public KeyValuePair<ColorModes, string>[] ColorModeList => _colorModeList;
    public KeyValuePair<TransparencyModes, string>[] TransparencyModeList => _transparencyModeList;
    public KeyValuePair<string, string>[] FontFamilyList => _fontFamilyList;
    public KeyValuePair<CopyFallbackActions, string>[] CopyFallbackActionList => _copyFallbackActionList;
    public KeyValuePair<ToolStates, string>[] ToolStateList => _toolStateList;

    public double DefaultNoteHeight
    {
        get => Settings.AppSettings.DefaultNoteHeight;
        set
        {
            Settings.AppSettings.DefaultNoteHeight = value;
            Messenger.Publish(new SettingChangedMessage(nameof(DefaultNoteHeight), value));
        }
    }

    public double DefaultNoteWidth
    {
        get => Settings.AppSettings.DefaultNoteWidth;
        set
        {
            Settings.AppSettings.DefaultNoteWidth = value;
            Messenger.Publish(new SettingChangedMessage(nameof(DefaultNoteWidth), value));
        }
    }

    public StartupPositions StartupPosition
    {
        get => Settings.AppSettings.StartupPosition;
        set
        {
            Settings.AppSettings.StartupPosition = value;
            Messenger.Publish(new SettingChangedMessage(nameof(StartupPosition), value));
        }
    }

    public bool CycleColors
    {
        get => Settings.AppSettings.CycleColors;
        set
        {
            Settings.AppSettings.CycleColors = value;
            Messenger.Publish(new SettingChangedMessage(nameof(CycleColors), value));
        }
    }

    public bool TrimCopiedText
    {
        get => Settings.AppSettings.TrimCopiedText;
        set
        {
            Settings.AppSettings.TrimCopiedText = value;
            Messenger.Publish(new SettingChangedMessage(nameof(TrimCopiedText), value));
        }
    }

    public bool TrimPastedText
    {
        get => Settings.AppSettings.TrimPastedText;
        set
        {
            Settings.AppSettings.TrimPastedText = value;
            Messenger.Publish(new SettingChangedMessage(nameof(TrimPastedText), value));
        }
    }

    public bool MiddleClickPaste
    {
        get => Settings.AppSettings.MiddleClickPaste;
        set
        {
            Settings.AppSettings.MiddleClickPaste = value;
            Messenger.Publish(new SettingChangedMessage(nameof(MiddleClickPaste), value));
        }
    }

    public bool AutoCopy
    {
        get => Settings.AppSettings.CopyTextOnHighlight;
        set
        {
            Settings.AppSettings.CopyTextOnHighlight = value;
            Messenger.Publish(new SettingChangedMessage(nameof(AutoCopy), value));
        }
    }

    public CopyFallbackActions CopyFallbackAction
    {
        get => Settings.AppSettings.NoSelectionCopyAction;
        set
        {
            Settings.AppSettings.NoSelectionCopyAction = value;
            Messenger.Publish(new SettingChangedMessage(nameof(CopyFallbackAction), value));
        }
    }

    public bool SpellChecker
    {
        get => Settings.AppSettings.SpellCheck;
        set
        {
            Settings.AppSettings.SpellCheck = value;
            Messenger.Publish(new SettingChangedMessage(nameof(SpellChecker), value));
        }
    }

    public bool NewLineAtEnd
    {
        get => Settings.AppSettings.NewLineAtEnd;
        set
        {
            Settings.AppSettings.NewLineAtEnd = value;
            Messenger.Publish(new SettingChangedMessage(nameof(NewLineAtEnd), value));
        }
    }

    public bool KeepNewLineAtEndVisible
    {
        get => Settings.AppSettings.KeepNewLineVisible;
        set
        {
            Settings.AppSettings.KeepNewLineVisible = value;
            Messenger.Publish(new SettingChangedMessage(nameof(KeepNewLineAtEndVisible), value));
        }
    }

    public bool WrapText
    {
        get => Settings.AppSettings.WrapText;
        set
        {
            Settings.AppSettings.WrapText = value;
            Messenger.Publish(new SettingChangedMessage(nameof(WrapText), value));
        }
    }

    public bool AutoIndent
    {
        get => Settings.AppSettings.AutoIndent;
        set
        {
            Settings.AppSettings.AutoIndent = value;
            Messenger.Publish(new SettingChangedMessage(nameof(AutoIndent), value));
        }
    }

    public bool TabSpaces
    {
        get => Settings.AppSettings.TabUsesSpaces;
        set
        {
            Settings.AppSettings.TabUsesSpaces = value;
            Messenger.Publish(new SettingChangedMessage(nameof(TabSpaces), value));
        }
    }

    public int TabWidth
    {
        get => Settings.AppSettings.TabWidth;
        set
        {
            Settings.AppSettings.TabWidth = value;
            Messenger.Publish(new SettingChangedMessage(nameof(TabWidth), value));
        }
    }

    public bool ConvertIndentation
    {
        get => Settings.AppSettings.ConvertIndentationOnPaste;
        set
        {
            Settings.AppSettings.ConvertIndentationOnPaste = value;
            Messenger.Publish(new SettingChangedMessage(nameof(ConvertIndentation), value));
        }
    }

    public MinimizeModes MinimizeMode
    {
        get => Settings.AppSettings.MinimizeMode;
        set
        {
            Settings.AppSettings.MinimizeMode = value;
            Messenger.Publish(new SettingChangedMessage(nameof(MinimizeMode), value));
        }
    }

    public TransparencyModes TransparencyMode
    {
        get => Settings.AppSettings.TransparencyMode;
        set
        {
            Settings.AppSettings.TransparencyMode = value;
            IsTransparencyEnabled = (TransparencyMode != TransparencyModes.Disabled);
            Messenger.Publish(new SettingChangedMessage(nameof(TransparencyMode), value));
        }
    }

    public bool IsTransparencyEnabled { get => _isTransparencyEnabled; set => SetProperty(ref _isTransparencyEnabled, value); }
    private bool _isTransparencyEnabled;

    public bool OpaqueWhenFocused
    {
        get => Settings.AppSettings.OpaqueWhenFocused;
        set
        {
            Settings.AppSettings.OpaqueWhenFocused = value;
            Messenger.Publish(new SettingChangedMessage(nameof(OpaqueWhenFocused), value));
        }
    }

    public double OpaqueOpacity
    {
        get => Settings.AppSettings.OpaqueOpacity;
        set
        {
            Settings.AppSettings.OpaqueOpacity = value;
            Messenger.Publish(new SettingChangedMessage(nameof(OpaqueOpacity), value));
        }
    }

    public double TransparentOpacity
    {
        get => Settings.AppSettings.TransparentOpacity;
        set
        {
            Settings.AppSettings.TransparentOpacity = value;
            Messenger.Publish(new SettingChangedMessage(nameof(TransparentOpacity), value));
        }
    }

    public ColorModes ColorMode
    {
        get => Settings.AppSettings.ColorMode;
        set
        {
            Settings.AppSettings.ColorMode = value;
            Messenger.Publish(new SettingChangedMessage(nameof(ColorMode), value));
        }
    }

    public string StandardFontFamily
    {
        get => Settings.AppSettings.StandardFontFamily;
        set
        {
            Settings.AppSettings.StandardFontFamily = value;
            Messenger.Publish(new SettingChangedMessage(nameof(StandardFontFamily), value));
        }
    }

    public string MonoFontFamily
    {
        get => Settings.AppSettings.MonoFontFamily;
        set
        {
            Settings.AppSettings.MonoFontFamily = value;
            Messenger.Publish(new SettingChangedMessage(nameof(MonoFontFamily), value));
        }
    }

    public bool UseMonoFont
    {
        get => Settings.AppSettings.UseMonoFont;
        set
        {
            Settings.AppSettings.UseMonoFont = value;
            Messenger.Publish(new SettingChangedMessage(nameof(UseMonoFont), value));
        }
    }

    public bool HideTitleBar
    {
        get => Settings.AppSettings.HideTitleBar;
        set
        {
            Settings.AppSettings.HideTitleBar = value;
            Messenger.Publish(new SettingChangedMessage(nameof(HideTitleBar), value));
        }
    }

    public bool ShowTrayIcon
    {
        get => Settings.AppSettings.ShowTrayIcon;
        set
        {
            Settings.AppSettings.ShowTrayIcon = value;
            Messenger.Publish(new SettingChangedMessage(nameof(ShowTrayIcon), value));
        }
    }

    public bool ShowNotesInTaskbar
    {
        get => Settings.AppSettings.ShowNotesInTaskbar;
        set
        {
            Settings.AppSettings.ShowNotesInTaskbar = value;
            Messenger.Publish(new SettingChangedMessage(nameof(ShowNotesInTaskbar), value));
        }
    }

    public bool CheckForUpdates
    {
        get => Settings.AppSettings.CheckForUpdates;
        set
        {
            Settings.AppSettings.CheckForUpdates = value;
            Messenger.Publish(new SettingChangedMessage(nameof(CheckForUpdates), value));
        }
    }

    #region Tools

    public ToolStates Base64ToolState
    {
        get => Settings.AppSettings.Base64State;
        set
        {
            Settings.AppSettings.Base64State = value;
            Messenger.Publish(new SettingChangedMessage(nameof(Base64ToolState), value));
        }
    }

    public ToolStates BracketToolState
    {
        get => Settings.AppSettings.BracketState;
        set
        {
            Settings.AppSettings.BracketState = value;
            Messenger.Publish(new SettingChangedMessage(nameof(BracketToolState), value));
        }
    }

    public ToolStates CaseToolState
    {
        get => Settings.AppSettings.CaseState;
        set
        {
            Settings.AppSettings.CaseState = value;
            Messenger.Publish(new SettingChangedMessage(nameof(CaseToolState), value));
        }
    }

    public ToolStates ColorToolState
    {
        get => Settings.AppSettings.ColorState;
        set
        {
            Settings.AppSettings.ColorState = value;
            Messenger.Publish(new SettingChangedMessage(nameof(ColorToolState), value));
        }
    }

    public ToolStates DateTimeToolState
    {
        get => Settings.AppSettings.DateTimeState;
        set
        {
            Settings.AppSettings.DateTimeState = value;
            Messenger.Publish(new SettingChangedMessage(nameof(DateTimeToolState), value));
        }
    }

    public ToolStates GibberishToolState
    {
        get => Settings.AppSettings.GibberishState;
        set
        {
            Settings.AppSettings.GibberishState = value;
            Messenger.Publish(new SettingChangedMessage(nameof(GibberishToolState), value));
        }
    }

    public ToolStates HashToolState
    {
        get => Settings.AppSettings.HashState;
        set
        {
            Settings.AppSettings.HashState = value;
            Messenger.Publish(new SettingChangedMessage(nameof(HashToolState), value));
        }
    }

    public ToolStates HtmlEntityToolState
    {
        get => Settings.AppSettings.HTMLEntityState;
        set
        {
            Settings.AppSettings.HTMLEntityState = value;
            Messenger.Publish(new SettingChangedMessage(nameof(HtmlEntityToolState), value));
        }
    }

    public ToolStates IndentToolState
    {
        get => Settings.AppSettings.IndentState;
        set
        {
            Settings.AppSettings.IndentState = value;
            Messenger.Publish(new SettingChangedMessage(nameof(IndentToolState), value));
        }
    }

    public ToolStates JoinToolState
    {
        get => Settings.AppSettings.JoinState;
        set
        {
            Settings.AppSettings.JoinState = value;
            Messenger.Publish(new SettingChangedMessage(nameof(JoinToolState), value));
        }
    }

    public ToolStates JsonToolState
    {
        get => Settings.AppSettings.JSONState;
        set
        {
            Settings.AppSettings.JSONState = value;
            Messenger.Publish(new SettingChangedMessage(nameof(JsonToolState), value));
        }
    }

    public ToolStates ListToolState
    {
        get => Settings.AppSettings.ListState;
        set
        {
            Settings.AppSettings.ListState = value;
            Messenger.Publish(new SettingChangedMessage(nameof(ListToolState), value));
        }
    }

    public ToolStates QuoteToolState
    {
        get => Settings.AppSettings.QuoteState;
        set
        {
            Settings.AppSettings.QuoteState = value;
            Messenger.Publish(new SettingChangedMessage(nameof(QuoteToolState), value));
        }
    }

    public ToolStates RemoveToolState
    {
        get => Settings.AppSettings.RemoveState;
        set
        {
            Settings.AppSettings.RemoveState = value;
            Messenger.Publish(new SettingChangedMessage(nameof(RemoveToolState), value));
        }
    }

    public ToolStates SlashToolState
    {
        get => Settings.AppSettings.SlashState;
        set
        {
            Settings.AppSettings.SlashState = value;
            Messenger.Publish(new SettingChangedMessage(nameof(SlashToolState), value));
        }
    }

    public ToolStates SortToolState
    {
        get => Settings.AppSettings.SortState;
        set
        {
            Settings.AppSettings.SortState = value;
            Messenger.Publish(new SettingChangedMessage(nameof(SortToolState), value));
        }
    }

    public ToolStates SplitToolState
    {
        get => Settings.AppSettings.SplitState;
        set
        {
            Settings.AppSettings.SplitState = value;
            Messenger.Publish(new SettingChangedMessage(nameof(SplitToolState), value));
        }
    }

    public ToolStates TrimToolState
    {
        get => Settings.AppSettings.TrimState;
        set
        {
            Settings.AppSettings.TrimState = value;
            Messenger.Publish(new SettingChangedMessage(nameof(TrimToolState), value));
        }
    }

    #endregion
}
