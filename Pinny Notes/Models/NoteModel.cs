using System.Drawing;
using Point = System.Drawing.Point;
using System.Linq;
using System.Windows;

using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Helpers;

namespace PinnyNotes.WpfUi.Models;

public class NoteModel
{
    public NoteModel(SettingsModel settings, NoteModel? parent = null)
    {
        Settings = settings;

        SetDefaultSize();
        InitTheme(parent?.Theme.Key);
        InitPosition(parent);
    }

    private string _text = "";
    public string Text {
        get => _text;
        set
        {
            _text = value;
            IsSaved = false;
        }
    }

    public int Width { get; set; }
    public int Height { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int GravityX { get; set; }
    public int GravityY { get; set; }
    public nint WindowHandle { get; set; }

    public ThemeModel Theme { get; set; } = null!;
    public bool IsPinned { get; set; }

    public bool IsSaved { get; set; }

    public int DefaultWidth { get; set; }
    public int DefaultHeight { get; set; }
    public double DefaultOpaqueOpacity { get; set; }
    public double DefaultTransparentOpacity { get; set; }

    public SettingsModel Settings { get; }

    public void LoadSettings()
    {
        DefaultWidth = Settings.Default.DefaultWidth;
        DefaultHeight = Settings.Default.DefaultHeight;
        DefaultOpaqueOpacity = Settings.Default.OpaqueOpacity;
        DefaultTransparentOpacity = Settings.Default.TransparentOpacity;
    }

    public void SetDefaultSize()
    {
        Width = DefaultWidth;
        Height = DefaultHeight;
    }

    public void UpdateGravity(Rectangle screenBounds)
    {
        GravityX = (X - screenBounds.X < screenBounds.Width / 2) ? 1 : -1;
        GravityY = (Y - screenBounds.Y < screenBounds.Height / 2) ? 1 : -1;
    }

    public void SaveTheme()
    {
        // TODO: Review how this method works after move to database backend
        Settings.Default.Theme = (Settings.Default.Theme.StartsWith(ThemeHelper.CycleThemeKey)) ? $"{ThemeHelper.CycleThemeKey}:{Theme.Key}" : Theme.Key;
        Settings.Default.Save();
    }

    private void InitPosition(NoteModel? parent = null)
    {
        int noteMargin = 45;

        Point position = new(0, 0);
        Rectangle screenBounds;

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

            switch (Settings.Notes_StartupPosition)
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

            switch (Settings.Notes_StartupPosition)
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
                int newX = position.X + (noteMargin * GravityX);
                if (newX < screenBounds.Left)
                    newX = screenBounds.Left;
                else if (newX + Width > screenBounds.Right)
                    newX = screenBounds.Right - Width;

                int newY = position.Y + (noteMargin * GravityY);
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

    private void InitTheme(string? parentThemeKey = null)
    {
        if (Settings.Default.Theme.StartsWith(ThemeHelper.CycleThemeKey))
        {
            Theme = ThemeHelper.GetNextTheme(
                Settings.Default.Theme[(ThemeHelper.CycleThemeKey.Length + 1)..],
                parentThemeKey
            );
            SaveTheme();
        }
        else
        {
            Theme = ThemeHelper.GetThemeOrDefault(Settings.Default.Theme);
        }
    }
}
