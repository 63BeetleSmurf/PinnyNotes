using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PinnyNotes.WpfUi.Controls.TitleBar;

internal class TitleBarBorderSpacer : Border, ITitleBarSpacerElement
{
    public TitleBarBorderSpacer() : base()
    {
        Background = Brushes.CadetBlue;
        Width = 30;
    }
}
