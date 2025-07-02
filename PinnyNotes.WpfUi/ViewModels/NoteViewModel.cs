using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;

using PinnyNotes.WpfUi.Commands;
using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Helpers;
using PinnyNotes.WpfUi.Properties;
using PinnyNotes.WpfUi.Services;
using PinnyNotes.WpfUi.Themes;

namespace PinnyNotes.WpfUi.ViewModels;


public class NoteViewModel : BaseViewModel
{
    private readonly MessengerService _messenger;

    public RelayCommand<ThemeColors> ChangeThemeColorCommand;

    public void OnSettingChanged(string settingName, object settingValue)
    {
        switch (settingName)
        {
            case "AutoCopy":
                AutoCopy = (bool)settingValue;
                break;
            case "AutoIndent":
                AutoIndent = (bool)settingValue;
                break;
            case "ConvertIndentation":
                ConvertIndentation = (bool)settingValue;
                break;
            case "CopyFallbackAction":
                CopyFallbackAction = (CopyFallbackActions)settingValue;
                break;
            case "KeepNewLineAtEndVisible":
                KeepNewLineAtEndVisible = (bool)settingValue;
                break;
            case "MiddleClickPaste":
                MiddleClickPaste = (bool)settingValue;
                break;
            case "NewLineAtEnd":
                NewLineAtEnd = (bool)settingValue;
                break;
            case "SpellCheck":
                SpellCheck = (bool)settingValue;
                break;
            case "TabSpaces":
                TabSpaces = (bool)settingValue;
                break;
            case "TabWidth":
                TabWidth = (int)settingValue;
                break;
            case "TrimCopiedText":
                TrimCopiedText = (bool)settingValue;
                break;
            case "TrimPastedText":
                TrimPastedText = (bool)settingValue;
                break;
            case "TransparencyMode":
            case "OpaqueWhenFocused":
            case "OnlyTransparentWhenPinned":
            case "OpaqueOpacity":
            case "TransparentOpacity":
                UpdateOpacity();
                break;
            case "ColorMode":
                UpdateBrushes(CurrentThemeColor);
                break;
            case "StandardFontFamily":
                if (!Settings.Default.UseMonoFont)
                    FontFamily = (string)settingValue;
                break;
            case "MonoFontFamily":
                if (Settings.Default.UseMonoFont)
                    FontFamily = (string)settingValue;
                break;
            case "UseMonoFont":
                FontFamily = ((bool)settingValue) ? Settings.Default.MonoFontFamily : Settings.Default.StandardFontFamily;
                break;
            case "ShowNotesInTaskbar":
                ShowNotesInTaskbar = (bool)settingValue;
                break;
            case "WrapText":
                WrapText = (TextWrapping)settingValue;
                break;
        }
    }

    public NoteViewModel(MessengerService messenger, NoteViewModel? parent = null)
    {
        _messenger = messenger;
        _messenger.NotifySettingChanged += OnSettingChanged;

        ChangeThemeColorCommand = new RelayCommand<ThemeColors>(ChangeThemeColor);

        InitNoteColor(parent);
        InitNotePosition(parent);
        UpdateOpacity();
    }

    private void InitNoteColor(NoteViewModel? parent = null)
    {
        // Set this first as cycle colors wont trigger a change if the next color if the default for ThemeColors
        CurrentThemeColor = (ThemeColors)Settings.Default.Color;
        if (Settings.Default.CycleColors)
        {
            int themeColorIndex = GetNextThemeColorIndex((int)CurrentThemeColor);
            if (parent != null && themeColorIndex == (int)parent.CurrentThemeColor)
                themeColorIndex = GetNextThemeColorIndex(themeColorIndex);
            CurrentThemeColor = (ThemeColors)themeColorIndex;
        }
        else
        {
            // Need to update brushes if color isn't changing.
            // If color is changed/cycled UpdateBrushes is called in OnCurrentThemeColorChanged.
            // Would probably work fine if enums were started at 1 rather than 0.
            UpdateBrushes(CurrentThemeColor);
        }
    }

    private int GetNextThemeColorIndex(int currentIndex)
    {
        int nextIndex = currentIndex + 1;
        if (!Enum.IsDefined((ThemeColors)nextIndex))
            nextIndex = 0;
        return nextIndex;
    }

    private void InitNotePosition(NoteViewModel? parent = null)
    {
        int noteMargin = 45;

        Point position = new(0, 0);
        System.Drawing.Rectangle screenBounds;

        if (parent != null)
        {
            screenBounds = ScreenHelper.GetCurrentScreenBounds(parent.WindowHandel);

            GravityX = parent.GravityX;
            GravityY = parent.GravityY;

            position.X = parent.X + (noteMargin * GravityX);
            position.Y = parent.Y + (noteMargin * GravityY);
        }
        else
        {
            int screenMargin = 78;
            screenBounds = ScreenHelper.GetPrimaryScreenBounds();

            switch ((StartupPositions)Settings.Default.StartupPosition)
            {
                case StartupPositions.TopLeft:
                case StartupPositions.MiddleLeft:
                case StartupPositions.BottomLeft:
                    position.X = screenMargin;
                    GravityX = 1;
                    break;
                case StartupPositions.TopCenter:
                case StartupPositions.MiddleCenter:
                case StartupPositions.BottomCenter:
                    position.X = screenBounds.Width / 2 - Width / 2;
                    GravityX = 1;
                    break;
                case StartupPositions.TopRight:
                case StartupPositions.MiddleRight:
                case StartupPositions.BottomRight:
                    position.X = (screenBounds.Width - screenMargin) - Width;
                    GravityX = -1;
                    break;
            }

            switch ((StartupPositions)Settings.Default.StartupPosition)
            {
                case StartupPositions.TopLeft:
                case StartupPositions.TopCenter:
                case StartupPositions.TopRight:
                    position.Y = screenMargin;
                    GravityY = 1;
                    break;
                case StartupPositions.MiddleLeft:
                case StartupPositions.MiddleCenter:
                case StartupPositions.MiddleRight:
                    position.Y = screenBounds.Height / 2 - Height / 2;
                    GravityY = -1;
                    break;
                case StartupPositions.BottomLeft:
                case StartupPositions.BottomCenter:
                case StartupPositions.BottomRight:
                    position.Y = (screenBounds.Height - screenMargin) - Height;
                    GravityY = -1;
                    break;
            }
        }

        // Apply noteMargin if another note is already in that position
        if (Application.Current.Windows.Count > 1)
        {
            Window[] otherWindows = new Window[Application.Current.Windows.Count];
            Application.Current.Windows.CopyTo(otherWindows, 0);
            while (otherWindows.Any(w => w.Left == position.X && w.Top == position.Y))
            {
                double newX = position.X + (noteMargin * GravityX);
                if (newX < screenBounds.Left)
                    newX = screenBounds.Left;
                else if (newX + Width > screenBounds.Right)
                    newX = screenBounds.Right - Width;

                double newY = position.Y + (noteMargin * GravityY);
                if (newY < screenBounds.Top)
                    newY = screenBounds.Top;
                else if (newY + Height > screenBounds.Bottom)
                    newY = screenBounds.Bottom - Height;

                if (position.X == newX && position.Y == newY)
                    break;

                position.X = newX;
                position.Y = newY;
            }
        }

        X = position.X;
        Y = position.Y;
    }

    private void UpdateBrushes(ThemeColors themeColor)
    {
        ColorModes colorMode = (ColorModes)Settings.Default.ColorMode;

        NotePalette notePalette;
        if (colorMode == ColorModes.Dark || (colorMode == ColorModes.System && SystemThemeHelper.IsDarkMode()))
            notePalette = ThemeHelper.Themes[themeColor].NoteDarkPalette;
        else
            notePalette = ThemeHelper.Themes[themeColor].NoteLightPalette;

        TitleBarColorBrush.Color = notePalette.TitleBar.Color;
        TitleButtonColorBrush.Color = notePalette.TitleButton.Color;
        BackgroundColorBrush.Color = notePalette.Background.Color;
        BorderColorBrush.Color = notePalette.Border.Color;
        TextColorBrush.Color = notePalette.Text.Color;
    }

    private void ChangeThemeColor(ThemeColors themeColor)
    {
        CurrentThemeColor = themeColor;
    }

    public void UpdateOpacity()
    {
        TransparencyModes transparentMode = (TransparencyModes)Settings.Default.TransparencyMode;
        if (transparentMode == TransparencyModes.Disabled)
        {
            Opacity = 1.0;
            return;
        }

        bool opaqueWhenFocused = Settings.Default.OpaqueWhenFocused;

        double opaqueOpacity = Settings.Default.OpaqueOpacity;
        double transparentOpacity = Settings.Default.TransparentOpacity;

        if ((opaqueWhenFocused && IsFocused) || (transparentMode == TransparencyModes.WhenPinned && !IsPinned))
            Opacity = opaqueOpacity;
        else
            Opacity = transparentOpacity;
    }

    public nint WindowHandel { get; set; }

    private ThemeColors _currentThemeColor;
    public ThemeColors CurrentThemeColor
    {
        get => _currentThemeColor;
        set
        {
            SetProperty(ref _currentThemeColor, value);
            Settings.Default.Color = (int)value;
            Settings.Default.Save();
            UpdateBrushes(value);
        }
    }

    public SolidColorBrush TitleBarColorBrush { get => _titleBarColorBrush; set => SetProperty(ref _titleBarColorBrush, value); }
    private SolidColorBrush _titleBarColorBrush = new();

    public SolidColorBrush TitleButtonColorBrush { get => _titleButtonColorBrush; set => SetProperty(ref _titleButtonColorBrush, value); }
    private SolidColorBrush _titleButtonColorBrush = new();

    public SolidColorBrush BackgroundColorBrush { get => _backgroundColorBrush; set => SetProperty(ref _backgroundColorBrush, value); }
    private SolidColorBrush _backgroundColorBrush = new();

    public SolidColorBrush BorderColorBrush { get => _borderColorBrush; set => SetProperty(ref _borderColorBrush, value); }
    private SolidColorBrush _borderColorBrush = new();

    public SolidColorBrush TextColorBrush { get => _textColorBrush; set => SetProperty(ref _textColorBrush, value); }
    private SolidColorBrush _textColorBrush = new();

    public int GravityX;
    public int GravityY;

    public double X { get => _x; set => SetProperty(ref _x, value); }
    private double _x;

    public double Y { get => _y; set => SetProperty(ref _y, value); }
    private double _y;


    public double Width { get => _width; set => SetProperty(ref _width, value); }
    private double _width = Settings.Default.DefaultNoteWidth;

    public double Height { get => _height; set => SetProperty(ref _height, value); }
    private double _height = Settings.Default.DefaultNoteHeight;


    public double Opacity { get => _opacity; set => SetProperty(ref _opacity, value); }
    private double _opacity;


    public bool AutoCopy { get => _autoCopy; set => SetProperty(ref _autoCopy, value); }
    private bool _autoCopy = Settings.Default.AutoCopy;

    public bool AutoIndent { get => _autoIndent; set => SetProperty(ref _autoIndent, value); }
    private bool _autoIndent = Settings.Default.AutoIndent;

    public bool ConvertIndentation { get => _convertIndentation; set => SetProperty(ref _convertIndentation, value); }
    private bool _convertIndentation = Settings.Default.ConvertIndentation;

    public CopyFallbackActions CopyFallbackAction { get => _copyFallbackAction; set => SetProperty(ref _copyFallbackAction, value); }
    private CopyFallbackActions _copyFallbackAction = (CopyFallbackActions)Settings.Default.CopyFallbackAction;

    public bool KeepNewLineAtEndVisible { get => _keepNewLineAtEndVisible; set => SetProperty(ref _keepNewLineAtEndVisible, value); }
    private bool _keepNewLineAtEndVisible = Settings.Default.KeepNewLineAtEndVisible;

    public bool MiddleClickPaste { get => _middleClickPaste; set => SetProperty(ref _middleClickPaste, value); }
    private bool _middleClickPaste = Settings.Default.MiddleClickPaste;

    public bool NewLineAtEnd { get => _newLineAtEnd; set => SetProperty(ref _newLineAtEnd, value); }
    private bool _newLineAtEnd = Settings.Default.NewLineAtEnd;

    public bool SpellCheck { get => _spellCheck; set => SetProperty(ref _spellCheck, value); }
    private bool _spellCheck = Settings.Default.SpellCheck;

    public bool TabSpaces { get => _tabSpaces; set => SetProperty(ref _tabSpaces, value); }
    private bool _tabSpaces = Settings.Default.TabSpaces;

    public int TabWidth { get => _tabWidth; set => SetProperty(ref _tabWidth, value); }
    private int _tabWidth = Settings.Default.TabWidth;

    public bool TrimCopiedText { get => _trimCopiedText; set => SetProperty(ref _trimCopiedText, value); }
    private bool _trimCopiedText = Settings.Default.TrimCopiedText;

    public bool TrimPastedText { get => _trimPastedText; set => SetProperty(ref _trimPastedText, value); }
    private bool _trimPastedText = Settings.Default.TrimPastedText;

    public TextWrapping WrapText { get => _wrapText; set => SetProperty(ref _wrapText, value); }
    private TextWrapping _wrapText = (TextWrapping)Settings.Default.WrapText;


    public bool IsPinned { get => _isPinned; set => SetProperty(ref _isPinned, value); }
    private bool _isPinned = false;

    public bool IsFocused { get => _isFocused; set => SetProperty(ref _isFocused, value); }
    private bool _isFocused;

    public bool IsSaved { get => _isSaved; set => SetProperty(ref _isSaved, value); }
    private bool _isSaved = false;


    public string Content { get => _content; set => SetProperty(ref _content, value); }
    private string _content = "";


    public string FontFamily { get => _fontFamily; set => SetProperty(ref _fontFamily, value); }
    private string _fontFamily = (Settings.Default.UseMonoFont) ? Settings.Default.MonoFontFamily : Settings.Default.StandardFontFamily;


    public bool ShowNotesInTaskbar { get => _showNotesInTaskbar; set => SetProperty(ref _showNotesInTaskbar, value); }
    private bool _showNotesInTaskbar = Settings.Default.ShowNotesInTaskbar;
}
