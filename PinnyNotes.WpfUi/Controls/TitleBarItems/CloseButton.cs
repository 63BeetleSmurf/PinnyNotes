using System.Windows.Media;

namespace PinnyNotes.WpfUi.Controls.TitleBarItems;

internal class CloseButton : TitleBarButton
{
    public CloseButton() : base()
    {
        ToolTip = "Close";
        IconData = Geometry.Parse("M 3,3 L 17,17 M 3,17 L 17,3");
    }
}
