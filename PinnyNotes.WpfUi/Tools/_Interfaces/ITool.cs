using System.Windows.Controls;

namespace PinnyNotes.WpfUi.Tools;

public interface ITool
{
    bool IsEnabled { get; }
    bool IsFavourite { get; }

    MenuItem GetMenuItem();
}
