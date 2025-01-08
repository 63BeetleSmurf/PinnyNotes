using Avalonia;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

using PinnyNotes.AvaloniaUi.Models;

namespace PinnyNotes.AvaloniaUi.ViewModels;

public partial class NoteViewModel : ViewModelBase
{
    private readonly NoteModel _model;

    public NoteViewModel() : this(new NoteModel())
    {
    }
    public NoteViewModel(NoteModel model)
    {
        _model = model;

        MinWidth = 150; // Will come from setting
        MinHeight = 75;

        Position = new(_model.X, _model.Y);
        Width = _model.Width;
        Height = _model.Height;

        IsTopmost = _model.IsPinned;

        Title = _model.Title;

        Text = _model.Text;
    }

    [ObservableProperty]
    private double _minWidth;
    [ObservableProperty]
    private double _minHeight;

    [ObservableProperty]
    private IBrush _windowBorderBrush = Brushes.Black;
    [ObservableProperty]
    private IBrush _noteBackgroundBrush = Brushes.White;
    [ObservableProperty]
    private IBrush _titleBarBackgroundBrush = Brushes.LightGray;
    [ObservableProperty]
    private IBrush _titleBarButtonBrush = Brushes.Black;
    [ObservableProperty]
    private IBrush _noteTextForegroundBrush = Brushes.Black;

    [ObservableProperty]
    private PixelPoint _position;
    partial void OnPositionChanged(PixelPoint value)
    {
        _model.X = value.X;
        _model.Y = value.Y;
    }

    [ObservableProperty]
    private double _width;
    partial void OnWidthChanged(double value)
        => _model.Width = (int)value;

    [ObservableProperty]
    private double _height;
    partial void OnHeightChanged(double value)
        => _model.Height = (int)value;

    [ObservableProperty]
    private bool _isTopmost;
    partial void OnIsTopmostChanged(bool value)
        => _model.IsPinned = value;

    [ObservableProperty]
    private string _title;
    partial void OnTitleChanged(string value)
        => _model.Title = value;

    [ObservableProperty]
    private string _text;
    partial void OnTextChanged(string value)
        => _model.Text = value;
}
