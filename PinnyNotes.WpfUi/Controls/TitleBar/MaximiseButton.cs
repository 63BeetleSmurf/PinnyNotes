using System.Windows.Media;

namespace PinnyNotes.WpfUi.Controls.TitleBar;

internal class MaximiseButton : BaseTitleBarButton
{
    public MaximiseButton() : base()
    {
        ToolTip = "Maximise";
        IconData = Geometry.Parse("M 2,2 h 16 v 16 h -16 Z");
    }
}
