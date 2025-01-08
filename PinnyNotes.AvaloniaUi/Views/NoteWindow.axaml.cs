using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.SpellChecker;
using System;
using System.Globalization;

using PinnyNotes.AvaloniaUi.ViewModels;

namespace PinnyNotes.AvaloniaUi.Views;

public partial class NoteWindow : Window
{
    private readonly TextBoxSpellChecker _textBoxSpellChecker;

    public NoteWindow()
    {
        InitializeComponent();

        PositionChanged += NoteWindow_PositionChanged;

        TitleBarGrid.PointerPressed += TitleBarGrid_PointerPressed;
        TitleBarGrid.DoubleTapped += TitleBarGrid_DoubleTapped;

        _textBoxSpellChecker = new TextBoxSpellChecker(SpellCheckerConfig.Create("pt_BR", "en_GB"));
        _textBoxSpellChecker.Initialize(NoteTextBox);
    }

    private void NoteWindow_PositionChanged(object? sender, PixelPointEventArgs e)
    {
        if (DataContext is NoteViewModel viewModel)
            viewModel.Position = Position;
    }

    private void TitleBarGrid_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        BeginMoveDrag(e);
    }

    private void TitleBarGrid_DoubleTapped(object? sender, Avalonia.Input.TappedEventArgs e)
    {
        if (WindowState == WindowState.Normal)
            WindowState = WindowState.Maximized;
        else if (WindowState == WindowState.Maximized)
            WindowState = WindowState.Normal;
    }
}

public class BoolToAngleConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => (value is bool isChecked && isChecked) ? 45 : 0;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => (value is int angle && angle != 0);
}
