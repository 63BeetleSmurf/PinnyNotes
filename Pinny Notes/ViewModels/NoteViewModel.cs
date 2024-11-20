using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Helpers;
using PinnyNotes.WpfUi.Properties;

namespace PinnyNotes.WpfUi.ViewModels;

public partial class NoteViewModel : ObservableRecipient, IRecipient<PropertyChangedMessage<object>>
{
    public const double DefaultWidth = 300.0;
    public const double DefaultHeight = 300.0;

    private const double _opaqueOpacity = 1.0;
    private const double _transparentOpacity = 0.8;

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

    public void Receive(PropertyChangedMessage<object> message)
    {
        switch (message.PropertyName)
        {
            case "SpellCheck":
                SpellCheck = (bool)message.NewValue;
                break;
            case "TransparentNotes":
            case "OpaqueWhenFocused":
            case "OnlyTransparentWhenPinned":
                UpdateOpacity();
                break;
            case "ColorMode":
                UpdateBrushes(CurrentThemeColor);
                break;
            case "UseMonoFont":
                FontFamily = ((bool)message.NewValue) ? _monoFontFamily : "";
                break;
            case "ShowNotesInTaskbar":
                ShowNotesInTaskbar = (bool)message.NewValue;
                break;
        }
    }

    public NoteViewModel(NoteViewModel? parent = null)
    {
        Messenger.Register(this);

        InitNoteColor(parent);
        InitNotePosition(parent);
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

    public nint WindowHandel { get; set; }

    [ObservableProperty]
    private ThemeColors _currentThemeColor;
    partial void OnCurrentThemeColorChanged(ThemeColors value)
    {
        Settings.Default.Color = (int)value;
        Settings.Default.Save();
        UpdateBrushes(value);
    }

    [ObservableProperty]
    private SolidColorBrush _titleBarColorBrush = null!;
    [ObservableProperty]
    private SolidColorBrush _titleButtonColorBrush = null!;
    [ObservableProperty]
    private SolidColorBrush _backgroundColorBrush = null!;
    [ObservableProperty]
    private SolidColorBrush _borderColorBrush = null!;
    [ObservableProperty]
    private SolidColorBrush _textColorBrush = null!;

    public int GravityX;
    public int GravityY;

    [ObservableProperty]
    private double _x;
    [ObservableProperty]
    private double _y;

    [ObservableProperty]
    private double _width = DefaultWidth;
    [ObservableProperty]
    private double _height = DefaultHeight;

    [ObservableProperty]
    private double _opacity = (Settings.Default.TransparentNotes && !Settings.Default.OnlyTransparentWhenPinned) ? 0.8 : 1.0;

    [ObservableProperty]
    private bool _spellCheck = Settings.Default.SpellCheck;

    [ObservableProperty]
    private bool _isPinned = false;
    [ObservableProperty]
    private bool _isFocused;
    [ObservableProperty]
    private bool _isSaved = false;

    [ObservableProperty]
    private string _content = "";

    [ObservableProperty]
    private string _fontFamily = (Settings.Default.UseMonoFont) ? _monoFontFamily : "";

    [ObservableProperty]
    private bool _showNotesInTaskbar = Settings.Default.ShowNotesInTaskbar;

    [RelayCommand]
    private void ChangeThemeColor(ThemeColors themeColor) => CurrentThemeColor = themeColor;
}
