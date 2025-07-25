using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;

using PinnyNotes.Core.Enums;
using PinnyNotes.WpfUi.Commands;
using PinnyNotes.WpfUi.Helpers;
using PinnyNotes.WpfUi.Models;
using PinnyNotes.WpfUi.Services;
using PinnyNotes.WpfUi.Themes;

namespace PinnyNotes.WpfUi.ViewModels;

public class NoteViewModel : BaseViewModel
{
    public RelayCommand<ThemeColors> ChangeThemeColorCommand { get; }

    public Theme[] AvailableThemes { get; }

    public NoteSettingsModel NoteSettings { get; set; }
    public EditorSettingsModel EditorSettings { get; set; }

    public NoteViewModel(
        AppMetadataService appMetadata, SettingsService settingsService, MessengerService messengerService,
        NoteViewModel? parent = null
    ) : base(appMetadata, settingsService, messengerService)
    {
        ChangeThemeColorCommand = new RelayCommand<ThemeColors>(ChangeThemeColor);

        AvailableThemes = ThemeHelper.Themes.Values.ToArray();

        NoteSettings = SettingsService.NoteSettings;
        NoteSettings.PropertyChanged += OnNoteSettingsChanged;
        EditorSettings = SettingsService.EditorSettings;

        _width = NoteSettings.DefaultWidth;
        _height = NoteSettings.DefaultHeight;

        InitNoteColor(parent);
        InitNotePosition(parent);
        UpdateOpacity();
    }

    private void InitNoteColor(NoteViewModel? parent = null)
    {
        // Set this first as cycle colors wont trigger a change if the next color if the default for ThemeColors
        CurrentThemeColor = AppMetadataService.AppData.ThemeColor;
        if (NoteSettings.CycleColors)
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

    private static int GetNextThemeColorIndex(int currentIndex)
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

            switch (NoteSettings.StartupPosition)
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

            switch (NoteSettings.StartupPosition)
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
        ColorModes colorMode = NoteSettings.ColorMode;

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
        TransparencyModes transparentMode = NoteSettings.TransparencyMode;
        if (transparentMode == TransparencyModes.Disabled)
        {
            Opacity = 1.0;
            return;
        }

        bool opaqueWhenFocused = NoteSettings.OpaqueWhenFocused;

        double opaqueOpacity = NoteSettings.OpaqueValue;
        double transparentOpacity = NoteSettings.TransparentValue;

        if ((opaqueWhenFocused && IsFocused) || (transparentMode == TransparencyModes.WhenPinned && !IsPinned))
            Opacity = opaqueOpacity;
        else
            Opacity = transparentOpacity;
    }

    private void OnNoteSettingsChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(NoteSettingsModel.TransparencyMode):
            case nameof(NoteSettingsModel.OpaqueWhenFocused):
            case nameof(NoteSettingsModel.OpaqueValue):
            case nameof(NoteSettingsModel.TransparentValue):
                UpdateOpacity();
                break;
            case nameof(NoteSettingsModel.ColorMode):
                UpdateBrushes();
                break;
        }
    }

    public nint WindowHandel { get; set; }

    private ThemeColors _currentThemeColor;
    public ThemeColors CurrentThemeColor
    {
        get => _currentThemeColor;
        set
        {
            SetProperty(ref _currentThemeColor, value);
            AppMetadataService.AppData.ThemeColor = value;
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


    public bool IsPinned { get => _isPinned; set => SetProperty(ref _isPinned, value); }
    private bool _isPinned = false;

    public bool IsFocused { get => _isFocused; set => SetProperty(ref _isFocused, value); }
    private bool _isFocused;

    public bool IsSaved { get => _isSaved; set => SetProperty(ref _isSaved, value); }
    private bool _isSaved = false;


    public string Content { get => _content; set => SetProperty(ref _content, value); }
    private string _content = "";
}
