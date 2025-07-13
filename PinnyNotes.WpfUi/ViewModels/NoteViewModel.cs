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
using PinnyNotes.WpfUi.Models;

namespace PinnyNotes.WpfUi.ViewModels;

public class NoteViewModel : BaseViewModel
{
    private readonly Dictionary<string, Action<object>> _settingChangeHandlers;

    public RelayCommand<ThemeColors> ChangeThemeColorCommand { get; }

    public Theme[] AvailableThemes { get; }

    public NoteViewModel(AppMetadataService appMetadata, SettingsService settings, MessengerService messenger, NoteViewModel? parent = null) : base(appMetadata, settings, messenger)
    {
        _settingChangeHandlers = new()
        {
            { nameof(ShowNotesInTaskbar), value => ShowNotesInTaskbar = (bool)value },
            { "TransparencyMode", _ => UpdateOpacity() },
            { "OpaqueWhenFocused", _ => UpdateOpacity() },
            { "OnlyTransparentWhenPinned", _ => UpdateOpacity() },
            { "OpaqueOpacity", _ => UpdateOpacity() },
            { "TransparentOpacity", _ => UpdateOpacity() },
            { "ColorMode", _ => UpdateBrushes() }
        };

        Messenger.Subscribe<SettingChangedMessage>(OnSettingChangedMessage);

        ChangeThemeColorCommand = new RelayCommand<ThemeColors>(ChangeThemeColor);

        AvailableThemes = ThemeHelper.Themes.Values.ToArray();

        _width = Settings.NoteSettings.DefaultWidth;
        _height = Settings.NoteSettings.DefaultHeight;
        _showNotesInTaskbar = Settings.NoteSettings.ShowInTaskBar;

        EditorSettings = settings.EditorSettings;

        InitNoteColor(parent);
        InitNotePosition(parent);
        UpdateOpacity();
    }

    private void InitNoteColor(NoteViewModel? parent = null)
    {
        // Set this first as cycle colors wont trigger a change if the next color if the default for ThemeColors
        CurrentThemeColor = AppMetadata.AppData.ThemeColor;
        if (Settings.NoteSettings.CycleColors)
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

            switch (Settings.NoteSettings.StartupPosition)
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

            switch (Settings.NoteSettings.StartupPosition)
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
        ColorModes colorMode = Settings.NoteSettings.ColorMode;

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
        TransparencyModes transparentMode = Settings.NoteSettings.TransparencyMode;
        if (transparentMode == TransparencyModes.Disabled)
        {
            Opacity = 1.0;
            return;
        }

        bool opaqueWhenFocused = Settings.NoteSettings.OpaqueWhenFocused;

        double opaqueOpacity = Settings.NoteSettings.OpaqueValue;
        double transparentOpacity = Settings.NoteSettings.TransparentValue;

        if ((opaqueWhenFocused && IsFocused) || (transparentMode == TransparencyModes.WhenPinned && !IsPinned))
            Opacity = opaqueOpacity;
        else
            Opacity = transparentOpacity;
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
            AppMetadata.AppData.ThemeColor = value;
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


    public EditorSettingsModel EditorSettings { get; set; }


    public bool IsPinned { get => _isPinned; set => SetProperty(ref _isPinned, value); }
    private bool _isPinned = false;

    public bool IsFocused { get => _isFocused; set => SetProperty(ref _isFocused, value); }
    private bool _isFocused;

    public bool IsSaved { get => _isSaved; set => SetProperty(ref _isSaved, value); }
    private bool _isSaved = false;


    public string Content { get => _content; set => SetProperty(ref _content, value); }
    private string _content = "";


    public bool ShowNotesInTaskbar { get => _showNotesInTaskbar; set => SetProperty(ref _showNotesInTaskbar, value); }
    private bool _showNotesInTaskbar;
}
