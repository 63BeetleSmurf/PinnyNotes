using System.Windows.Controls;

namespace Pinny_Notes.Tools;

public interface ITool
{
    bool IsEnabled { get; }
    bool IsFavourite { get; }

    MenuItem GetMenuItem();
}
