using System.Windows;
using System.Windows.Controls;

namespace PinnyNotes.WpfUi.Controls.TitleBar;

internal class TitleBarPanel : Panel
{
    public Thickness Padding
    {
        get => (Thickness)GetValue(PaddingProperty);
        set => SetValue(PaddingProperty, value);
    }
    public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register(
        nameof(Padding),
        typeof(Thickness),
        typeof(TitleBarPanel),
        new FrameworkPropertyMetadata(
            new Thickness(0),
            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender
        )
    );

    protected override Size MeasureOverride(Size availableSize)
    {
        double totalXPadding = Padding.Left + Padding.Right;
        double totalYPadding = Padding.Top + Padding.Bottom;

        Size minPanelSize = new(totalXPadding, totalYPadding);

        foreach (UIElement child in InternalChildren)
        {
            child.Measure(new Size(double.PositiveInfinity, Math.Clamp(availableSize.Height - totalYPadding, 0, double.MaxValue)));

            if (child is not ITitleBarSpacerElement) // Need to measure all elements but dont add spacers values
            {
                minPanelSize.Width += child.DesiredSize.Width;
                minPanelSize.Height = Math.Max(minPanelSize.Height, child.DesiredSize.Height + totalYPadding);
            }
        }

        return minPanelSize;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        double usableWidth = Math.Clamp(finalSize.Width - (Padding.Left + Padding.Right), 0, double.MaxValue);
        double usableHeight = Math.Clamp(finalSize.Height - (Padding.Top + Padding.Bottom), 0, double.MaxValue);

        double usedHorizontalSpace = 0;
        int stretchedChildrenCount = 0;

        foreach (UIElement child in InternalChildren)
        {
            if (child is ITitleBarSpacerElement)
            {
                stretchedChildrenCount++;
                continue;
            }

            usedHorizontalSpace += child.DesiredSize.Width;
        }

        double freeHorizontalSpace = Math.Max(0, usableWidth - usedHorizontalSpace);

        double stretchElementsWidth = 0;
        if (stretchedChildrenCount > 0)
        {
            stretchElementsWidth = Math.Max(0, freeHorizontalSpace / stretchedChildrenCount);
        }

        double currentX = Padding.Left;
        foreach (UIElement child in InternalChildren)
        {
            double childWidth = (child is ITitleBarSpacerElement) ? stretchElementsWidth : child.DesiredSize.Width;

            Rect childRect = new(
                currentX,
                Padding.Top,
                childWidth,
                usableHeight // Allows children use of vertical alignment
            );

            child.Arrange(childRect);

            currentX += childWidth;
        }

        return finalSize;
    }
}
