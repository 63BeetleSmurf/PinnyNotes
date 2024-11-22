using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Properties;
using System;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Forms;


namespace PinnyNotes.WpfUi.Models;

public class NoteModel
{
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

    public ThemeColors Color { get; set; }
    public bool IsPinned { get; set; }

    public bool IsSaved { get; set; }

    public void UpdateGravity(Rectangle screenBounds)
    {
        GravityX = (X - screenBounds.X < screenBounds.Width / 2) ? 1 : -1;
        GravityY = (Y - screenBounds.Y < screenBounds.Height / 2) ? 1 : -1;
    }

    private bool CheckNewLineAtEnd()
    {
        if (!Settings.Default.NewLineAtEnd || Text == "" || Text.EndsWith(Environment.NewLine))
            return false;

        Text += Environment.NewLine;

        return true;
    }
}
