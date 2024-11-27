using System.Windows.Media;

using PinnyNotes.WpfUi.Helpers;

namespace PinnyNotes.WpfUi.Models;

public class ThemeColorModel
{
    public ThemeColorModel(string titleBarColorHex, string titleBarButtonsColorHex, string backgroundColorHex, string textColorHex, string borderColorHex)
    {
        TitleBarColorHex = titleBarColorHex;
        TitleBarButtonsColorHex = titleBarButtonsColorHex;
        BackgroundColorHex = backgroundColorHex;
        TextColorHex = textColorHex;
        BorderColorHex = borderColorHex;
    }

    // Title Bar
    private string _titleBarColorHex = null!;
    public string TitleBarColorHex {
        get => _titleBarColorHex;
        set
        {
            _titleBarColorHex = value;
            _titleBarColor = ThemeHelper.HexToColor(_titleBarColorHex);
        }
    }

    private Color _titleBarColor;
    public Color TitleBarColor
    {
        get => _titleBarColor;
        set
        {
            _titleBarColor = value;
            _titleBarColorHex = ThemeHelper.ColorToHex(_titleBarColor);
        }
    }

    public SolidColorBrush TitleBarBrush
    {
        get => new SolidColorBrush(_titleBarColor);
    }

    // Title Bar Buttons
    private string _titleBarButtonsColorHex = null!;
    public string TitleBarButtonsColorHex
    {
        get => _titleBarButtonsColorHex;
        set
        {
            _titleBarButtonsColorHex = value;
            _titleBarButtonsColor = ThemeHelper.HexToColor(_titleBarButtonsColorHex);
        }
    }

    private Color _titleBarButtonsColor;
    public Color TitleBarButtonsColor
    {
        get => _titleBarButtonsColor;
        set
        {
            _titleBarButtonsColor = value;
            _titleBarButtonsColorHex = ThemeHelper.ColorToHex(_titleBarButtonsColor);
        }
    }

    public SolidColorBrush TitleBarButtonsBrush
    {
        get => new SolidColorBrush(_titleBarButtonsColor);
    }

    // Background
    private string _backgroundColorHex = null!;
    public string BackgroundColorHex
    {
        get => _backgroundColorHex;
        set
        {
            _backgroundColorHex = value;
            _backgroundColor = ThemeHelper.HexToColor(_backgroundColorHex);
        }
    }

    private Color _backgroundColor;
    public Color BackgroundColor
    {
        get => _backgroundColor;
        set
        {
            _backgroundColor = value;
            _backgroundColorHex = ThemeHelper.ColorToHex(_backgroundColor);
        }
    }

    public SolidColorBrush BackgroundColorBrush
    {
        get => new SolidColorBrush(_backgroundColor);
    }

    // Note Text
    private string _textColorHex = null!;
    public string TextColorHex
    {
        get => _textColorHex;
        set
        {
            _textColorHex = value;
            _textColor = ThemeHelper.HexToColor(_textColorHex);
        }
    }

    private Color _textColor;
    public Color TextColor
    {
        get => _textColor;
        set
        {
            _textColor = value;
            _textColorHex = ThemeHelper.ColorToHex(_textColor);
        }
    }

    public SolidColorBrush TextColorBrush
    {
        get => new SolidColorBrush(_textColor);
    }

    // Border
    private string _borderColorHex = null!;
    public string BorderColorHex
    {
        get => _borderColorHex;
        set
        {
            _borderColorHex = value;
            _borderColor = ThemeHelper.HexToColor(_borderColorHex);
        }
    }

    private Color _borderColor;
    public Color BorderColor
    {
        get => _borderColor;
        set
        {
            _borderColor = value;
            _borderColorHex = ThemeHelper.ColorToHex(_borderColor);
        }
    }

    public SolidColorBrush BorderColorBrush
    {
        get => new SolidColorBrush(_borderColor);
    }
}
