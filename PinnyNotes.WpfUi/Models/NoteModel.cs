using System.Windows.Media;

using PinnyNotes.Core.DataTransferObjects;
using PinnyNotes.Core.Enums;
using PinnyNotes.WpfUi.Themes;

namespace PinnyNotes.WpfUi.Models;

public class NoteModel : BaseModel
{
    public NoteModel(NoteSettingsModel NoteSettings)
    {

        Width = NoteSettings.DefaultWidth;
        Height = NoteSettings.DefaultHeight;

        TransparencyEnabled = (NoteSettings.TransparencyMode != TransparencyModes.Disabled);
    }

    public NoteModel(NoteDto noteDto, NoteSettingsModel NoteSettings)
    {
        Id = noteDto.Id;

        Content = noteDto.Content;

        X = noteDto.X;
        Y = noteDto.Y;
        Width = noteDto.Width;
        Height = noteDto.Height;

        GravityX = noteDto.GravityX;
        GravityY = noteDto.GravityY;

        ThemeColorScheme = noteDto.ThemeColorScheme;

        IsPinned = noteDto.IsPinned;
        IsOpen = noteDto.IsOpen;

        TransparencyEnabled = (NoteSettings.TransparencyMode != TransparencyModes.Disabled);
    }

    public int Id { get; set => SetProperty(ref field, value); }

    public string Content { get; set => SetProperty(ref field, value); } = "";

    public double X { get; set => SetProperty(ref field, value); }
    public double Y { get; set => SetProperty(ref field, value); }

    public double Width { get; set => SetProperty(ref field, value); }
    public double Height { get; set => SetProperty(ref field, value); }

    public int GravityX { get; set => SetProperty(ref field, value); }
    public int GravityY { get; set => SetProperty(ref field, value); }

    public string? ThemeColorScheme { get; set => SetProperty(ref field, value); }

    public bool IsPinned { get; set => SetProperty(ref field, value); }
    public bool IsOpen { get; set => SetProperty(ref field, value); }


    public nint WindowHandle { get; set; }

    public bool IsFocused { get; set => SetProperty(ref field, value); }

    public bool TransparencyEnabled { get; set => SetProperty(ref field, value); }
    public double Opacity { get; set => SetProperty(ref field, value); }
    public bool ShowInTaskbar { get; set => SetProperty(ref field, value); }

    public Brush Background { get; set => SetProperty(ref field, value); } = Brushes.LightGray;
    public Brush BorderBrush { get; set => SetProperty(ref field, value); } = Brushes.DarkGray;
    public Brush TitleGridBackground { get; set => SetProperty(ref field, value); } = Brushes.Gray;
    public Brush TitleGridButtonForeground { get; set => SetProperty(ref field, value); } = Brushes.DarkGray;
    public Brush ContentTextBoxForeground { get; set => SetProperty(ref field, value); } = Brushes.Black;


    public void UpdateBrushes(Palette palette)
    {
        Background = new SolidColorBrush(palette.Background);
        BorderBrush = new SolidColorBrush(palette.Border);
        TitleGridBackground = new SolidColorBrush(palette.Title);
        TitleGridButtonForeground = new SolidColorBrush(palette.Button);
        ContentTextBoxForeground = new SolidColorBrush(palette.Text);
    }

    public NoteDto ToDto()
    {
        NoteDto noteDto = new(
            Id,

            Content,

            X,
            Y,
            Width,
            Height,

            GravityX,
            GravityY,

            ThemeColorScheme,

            IsPinned,
            IsOpen
        );

        return noteDto;
    }
}
