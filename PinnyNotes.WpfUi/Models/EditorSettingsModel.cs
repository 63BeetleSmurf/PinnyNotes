using PinnyNotes.Core.Enums;
using PinnyNotes.WpfUi.Base;

namespace PinnyNotes.WpfUi.Models;

public class EditorSettingsModel : NotifyPropertyChangedBase
{
    // General
    public bool CheckSpelling { get; set => SetProperty(ref field, value); }
    public bool NewLineAtEnd { get; set => SetProperty(ref field, value); }
    public bool KeepNewLineVisible { get; set => SetProperty(ref field, value); }
    public bool WrapText { get; set => SetProperty(ref field, value); }

    // Fonts
    public string StandardFontFamily { get; set => SetProperty(ref field, value); } = string.Empty;
    public string MonoFontFamily { get; set => SetProperty(ref field, value); } = string.Empty;
    public bool UseMonoFont { get; set => SetProperty(ref field, value); }

    // Indentation
    public bool AutoIndent { get; set => SetProperty(ref field, value); }
    public bool UseSpacesForTab { get; set => SetProperty(ref field, value); }
    public int TabSpacesWidth { get; set => SetProperty(ref field, value); }
    public bool ConvertIndentationOnPaste { get; set => SetProperty(ref field, value); }

    // Clipboard
    public CopyActions CopyAction { get; set => SetProperty(ref field, value); }
    public bool TrimTextOnCopy { get; set => SetProperty(ref field, value); }
    public CopyActions CopyAltAction { get; set => SetProperty(ref field, value); }
    public bool TrimTextOnAltCopy { get; set => SetProperty(ref field, value); }
    public CopyFallbackActions CopyFallbackAction { get; set => SetProperty(ref field, value); }
    public bool TrimTextOnFallbackCopy { get; set => SetProperty(ref field, value); }
    public CopyFallbackActions CopyAltFallbackAction { get; set => SetProperty(ref field, value); }
    public bool TrimTextOnAltFallbackCopy { get; set => SetProperty(ref field, value); }
    public bool CopyOnSelect { get; set => SetProperty(ref field, value); }
    public PasteActions PasteAction { get; set => SetProperty(ref field, value); }
    public bool TrimTextOnPaste { get; set => SetProperty(ref field, value); }
    public PasteActions PasteAltAction { get; set => SetProperty(ref field, value); }
    public bool TrimTextOnAltPaste { get; set => SetProperty(ref field, value); }
    public bool MiddleClickPaste { get; set => SetProperty(ref field, value); }
}
