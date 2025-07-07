using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

using PinnyNotes.WpfUi.Commands;
using PinnyNotes.Core.Enums;
using PinnyNotes.WpfUi.Helpers;
using PinnyNotes.WpfUi.Messages;
using PinnyNotes.WpfUi.Services;
using PinnyNotes.WpfUi.Themes;

namespace PinnyNotes.WpfUi.ViewModels;

public class NoteViewModel : BaseViewModel
{
    private readonly Dictionary<string, Action<object>> _settingChangeHandlers;

    public RelayCommand<ThemeColors> ChangeThemeColorCommand { get; }

    public Theme[] AvailableThemes { get; }

    public NoteViewModel(SettingsService settings, MessengerService messenger, NoteViewModel? parent = null) : base(settings, messenger)
    {
        _settingChangeHandlers = new()
        {
            { nameof(AutoCopy), value => AutoCopy = (bool)value },
            { nameof(AutoIndent), value => AutoIndent = (bool)value },
            { nameof(ConvertIndentation), value => ConvertIndentation = (bool)value },
            { nameof(CopyFallbackAction), value => CopyFallbackAction = (CopyFallbackActions)value },
            { nameof(KeepNewLineAtEndVisible), value => KeepNewLineAtEndVisible = (bool)value },
            { nameof(MiddleClickPaste), value => MiddleClickPaste = (bool)value },
            { nameof(NewLineAtEnd), value => NewLineAtEnd = (bool)value },
            { nameof(SpellCheck), value => SpellCheck = (bool)value },
            { nameof(TabSpaces), value => TabSpaces = (bool)value },
            { nameof(TabWidth), value => TabWidth = (int)value },
            { nameof(TrimCopiedText), value => TrimCopiedText = (bool)value },
            { nameof(TrimPastedText), value => TrimPastedText = (bool)value },
            { nameof(ShowNotesInTaskbar), value => ShowNotesInTaskbar = (bool)value },
            { nameof(WrapText), value => WrapText = (TextWrapping)value },
            { "TransparencyMode", _ => UpdateOpacity() },
            { "OpaqueWhenFocused", _ => UpdateOpacity() },
            { "OnlyTransparentWhenPinned", _ => UpdateOpacity() },
            { "OpaqueOpacity", _ => UpdateOpacity() },
            { "TransparentOpacity", _ => UpdateOpacity() },
            { "ColorMode", _ => UpdateBrushes() },
            { "StandardFontFamily", _ => UpdateFontFamily() },
            { "MonoFontFamily", _ => UpdateFontFamily() },
            { "UseMonoFont", _ => UpdateFontFamily() }
        };

        Messenger.Subscribe<SettingChangedMessage>(OnSettingChangedMessage);

        ChangeThemeColorCommand = new RelayCommand<ThemeColors>(ChangeThemeColor);

        AvailableThemes = ThemeHelper.Themes.Values.ToArray();

        _width = Settings.AppSettings.DefaultNoteWidth;
        _height = Settings.AppSettings.DefaultNoteHeight;
        _autoCopy = Settings.AppSettings.CopyTextOnHighlight;
        _autoIndent = Settings.AppSettings.AutoIndent;
        _convertIndentation = Settings.AppSettings.ConvertIndentationOnPaste;
        _copyFallbackAction = Settings.AppSettings.NoSelectionCopyAction;
        _keepNewLineAtEndVisible = Settings.AppSettings.KeepNewLineVisible;
        _middleClickPaste = Settings.AppSettings.MiddleClickPaste;
        _newLineAtEnd = Settings.AppSettings.NewLineAtEnd;
        _spellCheck = Settings.AppSettings.SpellCheck;
        _tabSpaces = Settings.AppSettings.TabUsesSpaces;
        _tabWidth = Settings.AppSettings.TabWidth;
        _trimCopiedText = Settings.AppSettings.TrimCopiedText;
        _trimPastedText = Settings.AppSettings.TrimPastedText;
        _wrapText = (Settings.AppSettings.WrapText) ? TextWrapping.Wrap : TextWrapping.NoWrap;
        _fontFamily = (Settings.AppSettings.UseMonoFont) ? Settings.AppSettings.MonoFontFamily : Settings.AppSettings.StandardFontFamily;
        _showNotesInTaskbar = Settings.AppSettings.ShowNotesInTaskbar;

        InitNoteColor(parent);
        InitNotePosition(parent);
        UpdateOpacity();
    }

    private void InitNoteColor(NoteViewModel? parent = null)
    {
        // Set this first as cycle colors wont trigger a change if the next color if the default for ThemeColors
        CurrentThemeColor = (ThemeColors)Properties.Settings.Default.Color;
        if (Settings.AppSettings.CycleColors)
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
            UpdateBrushes();
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

            switch (Settings.AppSettings.StartupPosition)
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

            switch (Settings.AppSettings.StartupPosition)
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

    private void UpdateBrushes()
    {
        ColorModes colorMode = Settings.AppSettings.ColorMode;

        NotePalette notePalette;
        if (colorMode == ColorModes.Dark || (colorMode == ColorModes.System && SystemThemeHelper.IsDarkMode()))
            notePalette = ThemeHelper.Themes[CurrentThemeColor].NoteDarkPalette;
        else
            notePalette = ThemeHelper.Themes[CurrentThemeColor].NoteLightPalette;

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
        TransparencyModes transparentMode = Settings.AppSettings.TransparencyMode;
        if (transparentMode == TransparencyModes.Disabled)
        {
            Opacity = 1.0;
            return;
        }

        bool opaqueWhenFocused = Settings.AppSettings.OpaqueWhenFocused;

        double opaqueOpacity = Settings.AppSettings.OpaqueOpacity;
        double transparentOpacity = Settings.AppSettings.TransparentOpacity;

        if ((opaqueWhenFocused && IsFocused) || (transparentMode == TransparencyModes.WhenPinned && !IsPinned))
            Opacity = opaqueOpacity;
        else
            Opacity = transparentOpacity;
    }

    public void UpdateFontFamily()
    {
        FontFamily = (Settings.AppSettings.UseMonoFont) ? Settings.AppSettings.MonoFontFamily : Settings.AppSettings.StandardFontFamily;
    }

    private void OnSettingChangedMessage(SettingChangedMessage message)
    {
        if (_settingChangeHandlers.TryGetValue(message.SettingName, out Action<object>? handler))
            handler(message.NewValue);
    }

    public nint WindowHandel { get; set; }

    private ThemeColors _currentThemeColor;
    public ThemeColors CurrentThemeColor
    {
        get => _currentThemeColor;
        set
        {
            SetProperty(ref _currentThemeColor, value);
            Properties.Settings.Default.Color = (int)value;
            Properties.Settings.Default.Save();
            UpdateBrushes();
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
    private double _width;

    public double Height { get => _height; set => SetProperty(ref _height, value); }
    private double _height;


    public double Opacity { get => _opacity; set => SetProperty(ref _opacity, value); }
    private double _opacity;


    public bool AutoCopy { get => _autoCopy; set => SetProperty(ref _autoCopy, value); }
    private bool _autoCopy;

    public bool AutoIndent { get => _autoIndent; set => SetProperty(ref _autoIndent, value); }
    private bool _autoIndent;

    public bool ConvertIndentation { get => _convertIndentation; set => SetProperty(ref _convertIndentation, value); }
    private bool _convertIndentation;

    public CopyFallbackActions CopyFallbackAction { get => _copyFallbackAction; set => SetProperty(ref _copyFallbackAction, value); }
    private CopyFallbackActions _copyFallbackAction;

    public bool KeepNewLineAtEndVisible { get => _keepNewLineAtEndVisible; set => SetProperty(ref _keepNewLineAtEndVisible, value); }
    private bool _keepNewLineAtEndVisible;

    public bool MiddleClickPaste { get => _middleClickPaste; set => SetProperty(ref _middleClickPaste, value); }
    private bool _middleClickPaste;

    public bool NewLineAtEnd { get => _newLineAtEnd; set => SetProperty(ref _newLineAtEnd, value); }
    private bool _newLineAtEnd;

    public bool SpellCheck { get => _spellCheck; set => SetProperty(ref _spellCheck, value); }
    private bool _spellCheck;

    public bool TabSpaces { get => _tabSpaces; set => SetProperty(ref _tabSpaces, value); }
    private bool _tabSpaces;

    public int TabWidth { get => _tabWidth; set => SetProperty(ref _tabWidth, value); }
    private int _tabWidth;

    public bool TrimCopiedText { get => _trimCopiedText; set => SetProperty(ref _trimCopiedText, value); }
    private bool _trimCopiedText;

    public bool TrimPastedText { get => _trimPastedText; set => SetProperty(ref _trimPastedText, value); }
    private bool _trimPastedText;

    public TextWrapping WrapText { get => _wrapText; set => SetProperty(ref _wrapText, value); }
    private TextWrapping _wrapText;


    public bool IsPinned { get => _isPinned; set => SetProperty(ref _isPinned, value); }
    private bool _isPinned = false;

    public bool IsFocused { get => _isFocused; set => SetProperty(ref _isFocused, value); }
    private bool _isFocused;

    public bool IsSaved { get => _isSaved; set => SetProperty(ref _isSaved, value); }
    private bool _isSaved = false;


    public string Content { get => _content; set => SetProperty(ref _content, value); }
    private string _content = "";


    public string FontFamily { get => _fontFamily; set => SetProperty(ref _fontFamily, value); }
    private string _fontFamily;


    public bool ShowNotesInTaskbar { get => _showNotesInTaskbar; set => SetProperty(ref _showNotesInTaskbar, value); }
    private bool _showNotesInTaskbar;
}
