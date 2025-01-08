namespace PinnyNotes.AvaloniaUi.Models;

public class NoteModel
{
    public int X { get; set; } = 40;
    public int Y { get; set; } = 40;

    public int Width { get; set; } = 300;
    public int Height { get; set; } = 300;

    public bool IsPinned { get; set; } = false;

    public string Title { get; set; } = "New Note 1";

    public string Text { get; set; } = "Pinny Notes!";
}
