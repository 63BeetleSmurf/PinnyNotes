using PinnyNotes.WpfUi.Helpers;

namespace PinnyNotes.WpfUi.Models;

public class NotePreviewModel
{
    public NotePreviewModel(NoteModel note)
    {
        Id = note.Id;

        Text = note.Text;

        Width = note.Width;
        Height = note.Height;
        X = note.X;
        Y = note.Y;
        GravityX = note.GravityX;
        GravityY = note.GravityY;

        ThemeColors = ThemeHelper.GetThemeColorForMode(note.Theme, note.Settings.Notes_ColorMode);
    }

    public int Id { get; set; }

    public string Text { get; set; } = null!;

    public int Width { get; set; }
    public int Height { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int GravityX { get; set; }
    public int GravityY { get; set; }

    public ThemeColorsModel ThemeColors { get; set; } = null!;
}
