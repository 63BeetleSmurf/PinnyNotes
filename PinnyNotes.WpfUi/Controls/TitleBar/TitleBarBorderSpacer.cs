using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PinnyNotes.WpfUi.Controls.TitleBar;

internal class TitleBarBorderSpacer : Border, ITitleBarSpacerElement
{
    public TitleBarBorderSpacer() : base()
    {
        BorderBrush = Brushes.CadetBlue;
        BorderThickness = new Thickness(1);
        VerticalAlignment = VerticalAlignment.Stretch;
    }
}
