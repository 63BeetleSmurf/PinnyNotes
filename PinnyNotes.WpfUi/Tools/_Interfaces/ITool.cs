using System.Windows.Controls;

using PinnyNotes.WpfUi.Enums;

namespace PinnyNotes.WpfUi.Tools;

public interface ITool
{
    ToolStates State { get; }

    MenuItem GetMenuItem();
}
