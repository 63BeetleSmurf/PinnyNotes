using System;

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

    public string Text { get; set; }
    public int CaretIndex { get; set; }
    public int SelectionStart { get; set; }
    public int SelectionLength { get; set; }

    void ScrollToEnd();

    event EventHandler WindowLoaded;
    event EventHandler WindowMoved;
    event EventHandler WindowActivated;
}
