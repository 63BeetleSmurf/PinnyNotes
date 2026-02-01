using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

using PinnyNotes.Core.Enums;
using PinnyNotes.WpfUi.Commands;
using PinnyNotes.WpfUi.Helpers;
using PinnyNotes.WpfUi.Interop;
using PinnyNotes.WpfUi.Interop.Constants;
using PinnyNotes.WpfUi.Models;
using PinnyNotes.WpfUi.Services;
using PinnyNotes.WpfUi.Themes;

namespace PinnyNotes.WpfUi.ViewModels;

public class NoteViewModel : BaseViewModel
{
    public RelayCommand<string> ChangeThemeColorCommand { get; }

    public Theme[] AvailableThemes { get; }

    public NoteSettingsModel NoteSettings { get; set; }
    public EditorSettingsModel EditorSettings { get; set; }

    public nint WindowHandle { get; set; }

    public string? CurrentColorScheme
    {
        get;
        set
        {
            SetProperty(ref field, value);
            AppMetadataService.Metadata.ColorScheme = value;
            UpdateBrushes();
        }
    }

    public Brush Background { get; set => SetProperty(ref field, value); } = Brushes.LightGray;
    public Brush BorderBrush { get; set => SetProperty(ref field, value); } = Brushes.DarkGray;
    public Brush TitleGridBackground { get; set => SetProperty(ref field, value); } = Brushes.Gray;
    public Brush TitleGridButtonForeground { get; set => SetProperty(ref field, value); } = Brushes.DarkGray;
    public Brush ContentTextBoxForeground { get; set => SetProperty(ref field, value); } = Brushes.Black;

    public int GravityX;
    public int GravityY;

    public double X { get; set => SetProperty(ref field, value); }
    public double Y { get; set => SetProperty(ref field, value); }

    public double Width { get; set => SetProperty(ref field, value); }
    public double Height { get; set => SetProperty(ref field, value); }

    public double Opacity { get; set => SetProperty(ref field, value); }
    public bool ShowInTaskbar { get; set => SetProperty(ref field, value); }

    public bool IsPinned { get; set => SetProperty(ref field, value); } = false;
    public bool IsFocused { get; set => SetProperty(ref field, value); }
    public bool IsSaved { get; set => SetProperty(ref field, value); } = false;

    public string Content { get; set => SetProperty(ref field, value); } = "";

    public NoteViewModel(
        AppMetadataService appMetadata, SettingsService settingsService, MessengerService messengerService,
        NoteViewModel? parent = null
    ) : base(appMetadata, settingsService, messengerService)
    {
        ChangeThemeColorCommand = new RelayCommand<string>(ChangeThemeColor);

        AvailableThemes = [
            new DefaultTheme()
        ];

        NoteSettings = SettingsService.NoteSettings;
        NoteSettings.PropertyChanged += OnNoteSettingsChanged;
        EditorSettings = SettingsService.EditorSettings;

        Width = NoteSettings.DefaultWidth;
        Height = NoteSettings.DefaultHeight;

        InitNoteColor(parent?.CurrentColorScheme);
        InitNotePosition(parent);
        UpdateOpacity();
    }

    public void OnWindowLoaded(nint windowHandle)
    {
        WindowHandle = windowHandle;
        UpdateVisibility();
        UpdateAlwaysOnTop();
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

    public void UpdateAlwaysOnTop()
    {
        if (WindowHandle == 0)
            return;

        nint hWndInsertAfter = (IsFocused || IsPinned) ? HWND.TOPMOST : HWND.NOTOPMOST;

        uint uFlags = SWP.NOMOVE | SWP.NOSIZE | SWP.NOACTIVATE;

        _ = User32.SetWindowPos(WindowHandle, hWndInsertAfter, 0, 0, 0, 0, uFlags);
    }

    private void InitNoteColor(string? parentColorScheme = null)
    {
        // Set this first as cycle colors wont trigger a change if the next color if the default for ThemeColors
        CurrentColorScheme = AppMetadataService.Metadata.ColorScheme;
        if (NoteSettings.CycleColors)
            CurrentColorScheme = AvailableThemes[0].GetNextColorScheme(CurrentColorScheme, parentColorScheme);
        else
            // Need to update brushes if color isn't changing.
            // If color is changed/cycled UpdateBrushes is called in OnCurrentThemeColorChanged.
            // Would probably work fine if enums were started at 1 rather than 0.
            UpdateBrushes();
    }

    private void InitNotePosition(NoteViewModel? parent = null)
    {
        int noteMargin = 45;

        Point position = new(0, 0);
        Rect screenBounds;

        if (parent != null)
        {
            screenBounds = ScreenHelper.GetCurrentScreenBounds(parent.WindowHandle);

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
        CurrentColorScheme ??= AvailableThemes[0].GetNextColorScheme(null);

        ColorScheme colorScheme = AvailableThemes[0].ColorSchemes[CurrentColorScheme];

        ColorModes colorMode = NoteSettings.ColorMode;
        Palette palette;
        if (colorMode == ColorModes.Dark || (colorMode == ColorModes.System && SystemThemeHelper.IsDarkMode()))
            palette = colorScheme.Dark;
        else
            palette = colorScheme.Light;

        Background = new SolidColorBrush(palette.Background);
        BorderBrush = new SolidColorBrush(palette.Border);
        TitleGridBackground = new SolidColorBrush(palette.Title);
        TitleGridButtonForeground = new SolidColorBrush(palette.Button);
        ContentTextBoxForeground = new SolidColorBrush(palette.Text);
    }

    private void ChangeThemeColor(string colorScheme)
    {
        CurrentColorScheme = colorScheme;
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
            case nameof(NoteSettingsModel.VisibilityMode):
                UpdateVisibility();
                break;
        }
    }

    private void UpdateVisibility()
    {
        if (WindowHandle == 0)
            return;

        nint exStyle = User32.GetWindowLongPtrW(WindowHandle, GWL.EXSTYLE);

        switch (NoteSettings.VisibilityMode)
        {
            default:
            case VisibilityModes.ShowInTaskbar:
                exStyle &= ~WS_EX.TOOLWINDOW;
                ShowInTaskbar = true;
                break;
            case VisibilityModes.HideInTaskbar:
                exStyle &= ~WS_EX.TOOLWINDOW;
                ShowInTaskbar = false;
                break;
            case VisibilityModes.HideInTaskbarAndTaskSwitcher:
                exStyle |= WS_EX.TOOLWINDOW;
                ShowInTaskbar = false;
                break;
        }

        _ = User32.SetWindowLongPtrW(WindowHandle, GWL.EXSTYLE, exStyle);
    }
}
