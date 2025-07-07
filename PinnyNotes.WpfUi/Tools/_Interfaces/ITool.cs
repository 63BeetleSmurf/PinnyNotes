using System.Windows.Controls;

using PinnyNotes.Core.Enums;

namespace PinnyNotes.WpfUi.Tools;

public interface ITool
{
    ToolStates State { get; }

    MenuItem MenuItem { get; }
}
