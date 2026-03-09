using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace PinnyNotes.WpfUi.Controls.BackgroundSpellCheck;

public class BackgroundSpellingErrorAdorner : Adorner
{
    private readonly TextBox _textBox;
    private readonly Pen _errorPen;
    private List<BackgroundSpellingError> _errors = [];

    private const int RenderMarginChars = 20;

    public BackgroundSpellingErrorAdorner(TextBox adornedElement) : base(adornedElement)
    {
        _textBox = adornedElement;

        double thickness = Math.Max(1.0, _textBox.FontSize * 0.08);
        _errorPen = new Pen(Brushes.Red, thickness);
        _errorPen.Freeze();

        IsHitTestVisible = false;
    }

    public void UpdateErrors(IEnumerable<BackgroundSpellingError>? errors)
    {
        _errors = errors?.ToList() ?? [];

        InvalidateVisual();
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);

        if (_errors.Count == 0 || string.IsNullOrEmpty(_textBox.Text))
            return;

        (int visibleStart, int visibleEnd) = GetVisibleRange();

        List<BackgroundSpellingError> visibleErrors = [.. _errors.Where(e => e.StartIndex + e.Length >= visibleStart && e.StartIndex <= visibleEnd)];
        if (visibleErrors.Count == 0)
            return;

        StreamGeometry geometry = new();
        using StreamGeometryContext geometryContext = geometry.Open();

        foreach (BackgroundSpellingError error in visibleErrors)
        {
            Rect startRect = GetCharacterRect(error.StartIndex);
            if (startRect.IsEmpty)
                continue;

            int endIndex = Math.Min(error.StartIndex + error.Length, _textBox.Text.Length - 1);
            Rect endRect = GetCharacterRect(endIndex);
            if (endRect.IsEmpty)
                continue;

            double y = startRect.Bottom - _textBox.FontSize * 0.15;
            double startX = startRect.Left;
            double endX = endRect.Right;

            if (startX < 0 || endX <= startX || y < 0)
                continue;

            geometryContext.BeginFigure(new Point(startX, y), false, false);
            geometryContext.LineTo(new Point(endX, y), true, false);
        }

        geometry.Freeze();
        drawingContext.DrawGeometry(null, _errorPen, geometry);
    }

    private (int visibleStartIndex, int visibleEndIndex) GetVisibleRange()
    {
        if (_textBox.Text.Length == 0)
            return (0, 0);

        Point topLeft = new(0, 0);
        Point bottomRight = new(_textBox.ActualWidth, _textBox.ActualHeight);

        int visibleStart = _textBox.GetCharacterIndexFromPoint(topLeft, true);
        int visibleEnd = _textBox.GetCharacterIndexFromPoint(bottomRight, true);

        visibleStart = Math.Max(0, visibleStart - RenderMarginChars);
        visibleEnd = Math.Min(_textBox.Text.Length - 1, visibleEnd + RenderMarginChars);

        return (visibleStart, visibleEnd);
    }

    private Rect GetCharacterRect(int index)
    {
        if (index < 0 || index >= _textBox.Text.Length)
            return Rect.Empty;

        return _textBox.GetRectFromCharacterIndex(index, false);
    }
}
