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
    private string _standardFontFamily = string.Empty;
    public string MonoFontFamily { get => _monoFontFamily; set => SetProperty(ref _monoFontFamily, value); }
    private string _monoFontFamily = string.Empty;
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
    public bool TrimTextOnCopy { get => _trimTextOnCopy; set => SetProperty(ref _trimTextOnCopy, value); }
    private bool _trimTextOnCopy;
    public CopyActions CopyAltAction { get => _copyAltAction; set => SetProperty(ref _copyAltAction, value); }
    private CopyActions _copyAltAction;
    public bool TrimTextOnAltCopy { get => _trimTextOnAltCopy; set => SetProperty(ref _trimTextOnAltCopy, value); }
    private bool _trimTextOnAltCopy;
    public CopyFallbackActions CopyFallbackAction { get => _copyFallbackAction; set => SetProperty(ref _copyFallbackAction, value); }
    private CopyFallbackActions _copyFallbackAction;
    public bool TrimTextOnFallbackCopy { get => _trimTextOnFallbackCopy; set => SetProperty(ref _trimTextOnFallbackCopy, value); }
    private bool _trimTextOnFallbackCopy;
    public CopyFallbackActions CopyAltFallbackAction { get => _copyAltFallbackAction; set => SetProperty(ref _copyAltFallbackAction, value); }
    private CopyFallbackActions _copyAltFallbackAction;
    public bool TrimTextOnAltFallbackCopy { get => _trimTextOnAltFallbackCopy; set => SetProperty(ref _trimTextOnAltFallbackCopy, value); }
    private bool _trimTextOnAltFallbackCopy;
    public bool CopyOnSelect { get => _copyOnSelect; set => SetProperty(ref _copyOnSelect, value); }
    private bool _copyOnSelect;
    public PasteActions PasteAction { get => _pasteAction; set => SetProperty(ref _pasteAction, value); }
    private PasteActions _pasteAction;
    public bool TrimTextOnPaste { get => _trimTextOnPaste; set => SetProperty(ref _trimTextOnPaste, value); }
    private bool _trimTextOnPaste;
    public PasteActions PasteAltAction { get => _pasteAltAction; set => SetProperty(ref _pasteAltAction, value); }
    private PasteActions _pasteAltAction;
    public bool TrimTextOnAltPaste { get => _trimTextOnAltPaste; set => SetProperty(ref _trimTextOnAltPaste, value); }
    private bool _trimTextOnAltPaste;
    public bool MiddleClickPaste { get => _middleClickPaste; set => SetProperty(ref _middleClickPaste, value); }
    private bool _middleClickPaste;
}
