using PinnyNotes.WpfUi.Enums;
using System.Windows.Controls;

namespace PinnyNotes.WpfUi.Tools;

public interface ITool
{
    ToolStates State { get; }

    MenuItem GetMenuItem();
}
