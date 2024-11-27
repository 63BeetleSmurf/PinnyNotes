using PinnyNotes.WpfUi.Views.ContextMenus;
using System;
using System.Windows.Media;

namespace PinnyNotes.WpfUi.Views;

public interface INoteView
{
    nint Handle { get; }

    double Left { get; set; }
    double Top { get; set; }
    double Width { get; set; }
    double Height { get; set; }

    double Opacity { get; set; }

    bool Topmost { get; set; }

    NoteTitleBarContextMenu TitleBarContextMenu { get; set; }

    public string Text { get; set; }
    public int CaretIndex { get; set; }
    public int SelectionStart { get; set; }
    public int SelectionLength { get; set; }

    public Brush BorderColorBrush { get; set; }
    public Brush TitleBarColorBrush { get; set; }
    public Brush TitleButtonColorBrush { get; set; }
    public Brush BackgroundColorBrush { get; set; }
    public Brush TextColorBrush { get; set; }

    void ScrollToEnd();

    event EventHandler WindowLoaded;
    event EventHandler WindowMoved;
    event EventHandler WindowActivated;
    event EventHandler WindowDeactivated;

    event EventHandler NewNoteClicked;
    event EventHandler CloseNoteClicked;
    event EventHandler TitleBarRightClicked;

    event EventHandler TextChanged;
}
