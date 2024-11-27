using System.Drawing;
using Point = System.Drawing.Point;
using System.Linq;
using System.Windows;

using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Helpers;
using PinnyNotes.WpfUi.Properties;

namespace PinnyNotes.WpfUi.Models;

public class NoteModel
{
    public NoteModel(NoteModel? parent = null)
    {
        InitNotePosition(parent);
        InitTheme(parent);
    }

    private void InitNotePosition(NoteModel? parent = null)
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

    private void InitTheme(NoteModel? parent = null)
    {
        ThemeModel theme = ThemeHelper.Themes.Find(t => t.Name == Settings.Default.Theme)
            ?? ThemeHelper.Themes[0];

        if (Settings.Default.CycleThemes && ThemeHelper.Themes.Count > 1)
        {
            theme = GetNextTheme(ThemeHelper.Themes.IndexOf(theme));

            if (parent != null && theme == parent.Theme)
                theme = GetNextTheme(ThemeHelper.Themes.IndexOf(theme));
        }

        Theme = theme;
    }

    private ThemeModel GetNextTheme(int currentIndex) => (currentIndex + 1 < ThemeHelper.Themes.Count) ? ThemeHelper.Themes[currentIndex + 1] : ThemeHelper.Themes[0];

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

    public void UpdateGravity(Rectangle screenBounds)
    {
        GravityX = (X - screenBounds.X < screenBounds.Width / 2) ? 1 : -1;
        GravityY = (Y - screenBounds.Y < screenBounds.Height / 2) ? 1 : -1;
    }
}
