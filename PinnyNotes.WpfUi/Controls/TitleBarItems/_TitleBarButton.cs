using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PinnyNotes.WpfUi.Controls.TitleBarItems;

internal abstract class TitleBarButton : Button
{
    private const double ButtonWidth = 20;
    private const double ButtonHeight = 20;

    private const double StandardOpacity = 0.6;
    private const double HoverOpacity = 1.0;

    private const double IconStrokeThickness = 3.0;

    public TitleBarButton() : base()
    {
        Width = ButtonWidth;
        Height = ButtonHeight;
        Padding = new Thickness(0);
        Margin = new Thickness(10);
        Focusable = false;
        Background = Brushes.Transparent;
        Opacity = StandardOpacity;

        Template = CreateTemplate();

        MouseEnter += OnMouseEnter;
        MouseLeave += OnMouseLeave;
    }

    public EventHandler? IsCheckedChanged;

    public Geometry IconData
    {
        get => (Geometry)GetValue(IconDataProperty);
        set => SetValue(IconDataProperty, value);
    }
    public static readonly DependencyProperty IconDataProperty = DependencyProperty.Register(
        nameof(IconData),
        typeof(Geometry),
        typeof(TitleBarButton),
        new PropertyMetadata(Geometry.Empty)
    );

    public Brush IconStroke
    {
        get => (Brush)GetValue(IconStrokeProperty);
        set => SetValue(IconStrokeProperty, value);
    }
    public static readonly DependencyProperty IconStrokeProperty = DependencyProperty.Register(
        nameof(IconStroke),
        typeof(Brush),
        typeof(TitleBarButton),
        new PropertyMetadata(Brushes.Transparent)
    );

    public Brush IconFill
    {
        get => (Brush)GetValue(IconFillProperty);
        set => SetValue(IconFillProperty, value);
    }
    public static readonly DependencyProperty IconFillProperty = DependencyProperty.Register(
        nameof(IconFill),
        typeof(Brush),
        typeof(TitleBarButton),
        new PropertyMetadata(Brushes.Transparent)
    );

    public Transform IconRenderTransform
    {
        get => (Transform)GetValue(IconRenderTransformProperty);
        set => SetValue(IconRenderTransformProperty, value);
    }
    public static readonly DependencyProperty IconRenderTransformProperty = DependencyProperty.Register(
        nameof(IconRenderTransform),
        typeof(Transform),
        typeof(TitleBarButton),
        new PropertyMetadata(null)
    );

    public bool IsChecked
    {
        get => (bool)GetValue(IsCheckedProperty);
        set
        {
            SetValue(IsCheckedProperty, value);
            IsCheckedChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(
        nameof(IsChecked),
        typeof(bool),
        typeof(TitleBarButton),
        new PropertyMetadata(false)
    );

    private static ControlTemplate CreateTemplate()
    {
        ControlTemplate template = new(typeof(TitleBarButton));

        FrameworkElementFactory canvasFactory = new(typeof(Canvas));

        FrameworkElementFactory rectFactory = new(typeof(Rectangle));
        rectFactory.SetValue(Rectangle.WidthProperty, ButtonWidth);
        rectFactory.SetValue(Rectangle.HeightProperty, ButtonHeight);
        rectFactory.SetValue(Rectangle.FillProperty, Brushes.Transparent);

        FrameworkElementFactory pathFactory = new(typeof(Path));
        pathFactory.SetValue(Path.StrokeThicknessProperty, IconStrokeThickness);
        pathFactory.SetValue(Path.StrokeStartLineCapProperty, PenLineCap.Round);
        pathFactory.SetValue(Path.StrokeEndLineCapProperty, PenLineCap.Round);

        pathFactory.SetBinding(
            Path.DataProperty,
            new Binding(nameof(IconData))
            {
                RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent)
            }
        );

        pathFactory.SetBinding(
            Shape.StrokeProperty,
            new Binding(nameof(IconStroke))
            {
                RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent)
            }
        );

        pathFactory.SetBinding(
            Shape.FillProperty,
            new Binding(nameof(IconFill))
            {
                RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent)
            }
        );

        pathFactory.SetBinding(
            Shape.RenderTransformProperty,
            new Binding(nameof(IconRenderTransform))
            {
                RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent)
            }
        );

        canvasFactory.AppendChild(rectFactory);
        canvasFactory.AppendChild(pathFactory);

        template.VisualTree = canvasFactory;

        return template;
    }

    private void OnMouseEnter(object sender, MouseEventArgs e)
    {
        Opacity = HoverOpacity;
    }

    private void OnMouseLeave(object sender, MouseEventArgs e)
    {
        Opacity = StandardOpacity;
    }
}
