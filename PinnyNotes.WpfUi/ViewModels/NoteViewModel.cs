using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

using PinnyNotes.WpfUi.Commands;
using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Helpers;
using PinnyNotes.WpfUi.Properties;
using PinnyNotes.WpfUi.Services;

namespace PinnyNotes.WpfUi.ViewModels;


public class NoteViewModel : BaseViewModel
{
    private readonly MessengerService _messenger;

    private const string _monoFontFamily = "Consolas";

    private static readonly Dictionary<ThemeColors, Color[]> _colors = new() {
        {
            ThemeColors.Yellow,
            new[] {
                Color.FromRgb(254, 247, 177),   // #fef7b1
                Color.FromRgb(255, 252, 221),   // #fffcdd
                Color.FromRgb(254, 234, 0),     // #feea00
            }
        },
        {
            ThemeColors.Orange,
            new[] {
                Color.FromRgb(255, 209, 121),   // #ffd179
                Color.FromRgb(254, 232, 185),   // #fee8b9
                Color.FromRgb(255, 171, 0),     // #ffab00
            }
        },
        {
            ThemeColors.Red,
            new[] {
                Color.FromRgb(255, 124, 129),   // #ff7c81
                Color.FromRgb(255, 196, 198),   // #ffc4c6
                Color.FromRgb(227, 48, 54),     // #e33036
            }
        },
        {
            ThemeColors.Pink,
            new[] {
                Color.FromRgb(217, 134, 204),   // #d986cc
                Color.FromRgb(235, 191, 227),   // #ebbfe3
                Color.FromRgb(167, 41, 149),    // #a72995
            }
        },
        {
            ThemeColors.Purple,
            new[] {
                Color.FromRgb(157, 154, 221),   // #9d9add
                Color.FromRgb(208, 206, 243),   // #d0cef3
                Color.FromRgb(98, 91, 184),     // #625bb8
            }
        },
        {
            ThemeColors.Blue,
            new[] {
                Color.FromRgb(122, 195, 230),   // #7ac3e6
                Color.FromRgb(179, 217, 236),   // #b3d9ec
                Color.FromRgb(17, 149, 221),    // #1195dd
            }
        },
        {
            ThemeColors.Aqua,
            new[] {
                Color.FromRgb(151, 207, 198),   // #97cfc6
                Color.FromRgb(192, 226, 225),   // #c0e2e1
                Color.FromRgb(22, 176, 152),    // #16b098
           }
        },
        {
            ThemeColors.Green,
            new[] {
                Color.FromRgb(198, 214, 125),   // #c6d67d
                Color.FromRgb(227, 235, 198),   // #e3ebc6
                Color.FromRgb(170, 204, 4),     // #aacc04
            }
        }
    };

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
            case "UseMonoFont":
                FontFamily = ((bool)settingValue) ? _monoFontFamily : "";
                break;
            case "ShowNotesInTaskbar":
                ShowNotesInTaskbar = (bool)settingValue;
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

        // 0 = Title, 1 = Background, 2 = Border
        BorderColorBrush = new(_colors[themeColor][2]);         // #feea00

        if (colorMode == ColorModes.Dark || (colorMode == ColorModes.System && SystemThemeHelper.IsDarkMode()))
        {
            TitleBarColorBrush = new(Color.FromRgb(70, 70, 70));    // #464646
            TitleButtonColorBrush = new(_colors[themeColor][2]);
            BackgroundColorBrush = new(Color.FromRgb(50, 50, 50));  // #323232
            TextColorBrush = new(Colors.White);
        }
        else
        {
            TitleBarColorBrush = new(_colors[themeColor][0]);        // #464646
            TitleButtonColorBrush = new(Color.FromRgb(70, 70, 70));
            BackgroundColorBrush = new(_colors[themeColor][1]);      // #323232
            TextColorBrush = new(Colors.Black);
        }
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
    private SolidColorBrush _titleBarColorBrush = null!;

    public SolidColorBrush TitleButtonColorBrush { get => _titleButtonColorBrush; set => SetProperty(ref _titleButtonColorBrush, value); }
    private SolidColorBrush _titleButtonColorBrush = null!;

    public SolidColorBrush BackgroundColorBrush { get => _backgroundColorBrush; set => SetProperty(ref _backgroundColorBrush, value); }
    private SolidColorBrush _backgroundColorBrush = null!;

    public SolidColorBrush BorderColorBrush { get => _borderColorBrush; set => SetProperty(ref _borderColorBrush, value); }
    private SolidColorBrush _borderColorBrush = null!;

    public SolidColorBrush TextColorBrush { get => _textColorBrush; set => SetProperty(ref _textColorBrush, value); }
    private SolidColorBrush _textColorBrush = null!;

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


    public bool IsPinned { get => _isPinned; set => SetProperty(ref _isPinned, value); }
    private bool _isPinned = false;

    public bool IsFocused { get => _isFocused; set => SetProperty(ref _isFocused, value); }
    private bool _isFocused;

    public bool IsSaved { get => _isSaved; set => SetProperty(ref _isSaved, value); }
    private bool _isSaved = false;


    public string Content { get => _content; set => SetProperty(ref _content, value); }
    private string _content = "";


    public string FontFamily { get => _fontFamily; set => SetProperty(ref _fontFamily, value); }
    private string _fontFamily = (Settings.Default.UseMonoFont) ? _monoFontFamily : "";


    public bool ShowNotesInTaskbar { get => _showNotesInTaskbar; set => SetProperty(ref _showNotesInTaskbar, value); }
    private bool _showNotesInTaskbar = Settings.Default.ShowNotesInTaskbar;
}
