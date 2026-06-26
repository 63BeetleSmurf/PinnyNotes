using System.Windows.Media;

namespace PinnyNotes.WpfUi.Controls.TitleBar;

internal class MinimiseButton : BaseTitleBarButton
{
    public MinimiseButton() : base()
    {
        ToolTip = "Minimise";
        IconData = Geometry.Parse("M 2,18 h 16");
    }
}
