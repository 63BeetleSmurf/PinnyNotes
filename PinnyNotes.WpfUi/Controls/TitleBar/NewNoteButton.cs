using System.Windows.Media;

namespace PinnyNotes.WpfUi.Controls.TitleBar;

internal class NewNoteButton : BaseTitleBarButton
{
    public NewNoteButton() : base()
    {
        ToolTip = "New Note";
        IconData = Geometry.Parse("M 10,2 v 16 M 2,10 h 16");
    }
}
