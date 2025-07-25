using PinnyNotes.Core.Enums;
using PinnyNotes.WpfUi.Base;

namespace PinnyNotes.WpfUi.Models;

public class EditorSettingsModel : NotifyPropertyChangedBase
{
    // General
    public bool CheckSpelling { get => _checkSpelling; set => SetProperty(ref _checkSpelling, value); }
    private bool _checkSpelling;
    public bool NewLineAtEnd { get => _newLineAtEnd; set => SetProperty(ref _newLineAtEnd, value); }
    private bool _newLineAtEnd;
    public bool KeepNewLineVisible { get => _keepNewLineVisible; set => SetProperty(ref _keepNewLineVisible, value); }
    private bool _keepNewLineVisible;
    public bool WrapText { get => _wrapText; set => SetProperty(ref _wrapText, value); }
    private bool _wrapText;

    // Fonts
    public string StandardFontFamily { get => _standardFontFamily; set => SetProperty(ref _standardFontFamily, value); }
    private string _standardFontFamily;
    public string MonoFontFamily { get => _monoFontFamily; set => SetProperty(ref _monoFontFamily, value); }
    private string _monoFontFamily;
    public bool UseMonoFont { get => _useMonoFont; set => SetProperty(ref _useMonoFont, value); }
    private bool _useMonoFont;

    // Indentation
    public bool AutoIndent { get => _autoIndent; set => SetProperty(ref _autoIndent, value); }
    private bool _autoIndent;
    public bool UseSpacesForTab { get => _useSpacesForTab; set => SetProperty(ref _useSpacesForTab, value); }
    private bool _useSpacesForTab;
    public int TabSpacesWidth { get => _tabSpacesWidth; set => SetProperty(ref _tabSpacesWidth, value); }
    private int _tabSpacesWidth;
    public bool ConvertIndentationOnPaste { get => _convertIndentationOnPaste; set => SetProperty(ref _convertIndentationOnPaste, value); }
    private bool _convertIndentationOnPaste;

    // Clipboard
    public CopyActions CopyAction { get => _copyAction; set => SetProperty(ref _copyAction, value); }
    private CopyActions _copyAction;
    public CopyActions CopyAltAction { get => _copyAltAction; set => SetProperty(ref _copyAltAction, value); }
    private CopyActions _copyAltAction;
    public CopyFallbackActions CopyFallbackAction { get => _copyFallbackAction; set => SetProperty(ref _copyFallbackAction, value); }
    private CopyFallbackActions _copyFallbackAction;
    public CopyFallbackActions CopyAltFallbackAction { get => _copyAltFallbackAction; set => SetProperty(ref _copyAltFallbackAction, value); }
    private CopyFallbackActions _copyAltFallbackAction;
    public bool TrimCopiedText { get => _trimCopiedText; set => SetProperty(ref _trimCopiedText, value); }
    private bool _trimCopiedText;
    public bool CopyOnSelect { get => _copyOnSelect; set => SetProperty(ref _copyOnSelect, value); }
    private bool _copyOnSelect;
    public bool MiddleClickPaste { get => _middleClickPaste; set => SetProperty(ref _middleClickPaste, value); }
    private bool _middleClickPaste;
    public bool TrimPastedText { get => _trimPastedText; set => SetProperty(ref _trimPastedText, value); }
    private bool _trimPastedText;
}
