using System.Windows;
using System.Windows.Media;

namespace PinnyNotes.WpfUi.Controls.TitleBarItems;

internal class PinButton : TitleBarButton
{
    private const double StandardAngle = 45.0;
    private const double PinnedAngle = 0.0;

    private readonly RotateTransform _rotateTransform;

    public PinButton() : base()
    {
        ToolTip = "Toggle pinned";
        IconData = Geometry.Parse("M 5,0 h 10 c 1,0 1,2 0,2 h -2 l 1,6 c 0,0 4,1 4,5 0,1 -7,1 -7,1 0,0 0,6 -1,6 C 9,20 9,14 9,14 9,14 2,14 2,13 2,9 6,8 6,8 L 7,2 H 5 C 4,2 4,0 5,0 Z");

        _rotateTransform = new(
            GetRotateTransformAngle(),
            Width / 2,
            Height / 2
        );
        IconRenderTransform = _rotateTransform;

        Click += PinButton_Click;
        IsCheckedChanged += PinButton_IsCheckedChanged;
    }

    private void PinButton_Click(object sender, RoutedEventArgs e)
    {
        IsChecked = !IsChecked;
    }

    private void PinButton_IsCheckedChanged(object? sender, EventArgs e)
    {
        _rotateTransform.Angle = GetRotateTransformAngle();
    }

    private double GetRotateTransformAngle()
    {
        return (IsChecked) ? PinnedAngle : StandardAngle;
    }
}
