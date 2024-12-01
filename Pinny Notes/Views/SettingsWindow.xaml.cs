using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Helpers;
using PinnyNotes.WpfUi.ViewModels;

namespace PinnyNotes.WpfUi.Views;

public partial class SettingsWindow : Window
{
    private Window _lastOwner;

    public SettingsWindow()
    {
        DataContext = new SettingsViewModel();
        InitializeComponent();
        _lastOwner = Owner;
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void Window_Activated(object sender, System.EventArgs e)
    {
        if (Owner == null)
        {
            Left = SystemParameters.PrimaryScreenWidth / 2 - Width / 2;
            Top = SystemParameters.PrimaryScreenHeight / 2 - Height / 2;
        }
        else
        {
            if (Owner == _lastOwner)
                return;
            _lastOwner = Owner;

            Point position = new(
                (Owner.Left + Owner.Width / 2) - Width / 2,
                (Owner.Top + Owner.Height / 2) - Height / 2
            );
            System.Drawing.Rectangle currentScreenBounds = ScreenHelper.GetCurrentScreenBounds(
                ScreenHelper.GetWindowHandle(Owner)
            );

            if (position.X < currentScreenBounds.Left)
                position.X = currentScreenBounds.Left;
            else if (position.X + Width > currentScreenBounds.Right)
                position.X = currentScreenBounds.Right - Width;

            if (position.Y < currentScreenBounds.Top)
                position.Y = currentScreenBounds.Top;
            else if (position.Y + Height > currentScreenBounds.Bottom)
                position.Y = currentScreenBounds.Bottom - Height;

            Left = position.X;
            Top = position.Y;
        }
    }

    private void PopulateComboBox(ComboBox comboBox, List<KeyValuePair<int, string>> items)
    {
        comboBox.Items.Clear();
        foreach (KeyValuePair<int, string> item in items)
            comboBox.Items.Add(item);
    }

    // Application
    // - General
    public bool ShowTrayIcon
    {
        get => ShowTrayIconCheckBox.IsChecked ?? false;
        set => ShowTrayIconCheckBox.IsChecked = value;
    }

    public bool ShowNotesInTaskbar
    {
        get => ShowNotesInTaskbarCheckBox.IsChecked ?? false;
        set => ShowNotesInTaskbarCheckBox.IsChecked = value;
    }

    public bool CheckForUpdates
    {
        get => CheckForUpdatesCheckBox.IsChecked ?? false;
        set => CheckForUpdatesCheckBox.IsChecked = value;
    }

    // Notes
    // - General
    public StartupPositions StartupPosition
    {
        get => (StartupPositions)StartupPositionComboBox.SelectedValue;
        set => StartupPositionComboBox.SelectedValue = value;
    }
    public List<KeyValuePair<int, string>> PopulateStartupPositions
    {
        set => PopulateComboBox(StartupPositionComboBox, value);
    }

    public MinimizeModes MinimizeMode
    {
        get => (MinimizeModes)MinimizeModeComboBox.SelectedValue;
        set => MinimizeModeComboBox.SelectedValue = value;
    }
    public List<KeyValuePair<int, string>> PopulateMinimizeModes
    {
        set => PopulateComboBox(MinimizeModeComboBox, value);
    }

    public bool HideTitleBar
    {
        get => HideTitleBarCheckBox.IsChecked ?? false;
        set => HideTitleBarCheckBox.IsChecked = value;
    }

    // - Theme
    public bool CycleColors
    {
        get => CycleColorsCheckBox.IsChecked ?? false;
        set => CycleColorsCheckBox.IsChecked = value;
    }

    public MinimizeModes ColorMode
    {
        get => (MinimizeModes)ColorModeComboBox.SelectedValue;
        set => ColorModeComboBox.SelectedValue = value;
    }
    public List<KeyValuePair<int, string>> PopulateColorModes
    {
        set => PopulateComboBox(ColorModeComboBox, value);
    }

    // - Transparency
    public bool TransparentNotes
    {
        get => TransparentNotesCheckBox.IsChecked ?? false;
        set => TransparentNotesCheckBox.IsChecked = value;
    }

    public bool OpaqueWhenFocused
    {
        get => OpaqueWhenFocusedCheckBox.IsChecked ?? false;
        set => OpaqueWhenFocusedCheckBox.IsChecked = value;
    }

    public bool OnlyTransparentWhenPinned
    {
        get => OnlyTransparentWhenPinnedCheckBox.IsChecked ?? false;
        set => OnlyTransparentWhenPinnedCheckBox.IsChecked = value;
    }

    // Editor
    // - General
    public bool UseMonoFont
    {
        get => UseMonoFontCheckBox.IsChecked ?? false;
        set => UseMonoFontCheckBox.IsChecked = value;
    }

    public bool SpellChecker
    {
        get => SpellCheckerCheckBox.IsChecked ?? false;
        set => SpellCheckerCheckBox.IsChecked = value;
    }

    public bool AutoIndent
    {
        get => AutoIndentCheckBox.IsChecked ?? false;
        set => AutoIndentCheckBox.IsChecked = value;
    }

    public bool NewLineAtEnd
    {
        get => NewLineAtEndCheckBox.IsChecked ?? false;
        set => NewLineAtEndCheckBox.IsChecked = value;
    }

    public bool KeepNewLineAtEndVisible
    {
        get => KeepNewLineAtEndVisibleCheckBox.IsChecked ?? false;
        set => KeepNewLineAtEndVisibleCheckBox.IsChecked = value;
    }

    // - Indentation
    public bool TabSpaces
    {
        get => TabSpacesCheckBox.IsChecked ?? false;
        set => TabSpacesCheckBox.IsChecked = value;
    }

    public bool ConvertIndentation
    {
        get => ConvertIndentationCheckBox.IsChecked ?? false;
        set => ConvertIndentationCheckBox.IsChecked = value;
    }

    public int TabWidth
    {
        get => int.Parse(TabWidthTextBox.Text);
        set => TabWidthTextBox.Text = value.ToString();
    }

    // - Copy and Paste

    public bool MiddleClickPaste
    {
        get => MiddleClickPasteCheckBox.IsChecked ?? false;
        set => MiddleClickPasteCheckBox.IsChecked = value;
    }

    public bool TrimPastedText
    {
        get => TrimPastedTextCheckBox.IsChecked ?? false;
        set => TrimPastedTextCheckBox.IsChecked = value;
    }

    public bool TrimCopiedText
    {
        get => TrimCopiedTextCheckBox.IsChecked ?? false;
        set => TrimCopiedTextCheckBox.IsChecked = value;
    }

    public bool AutoCopy
    {
        get => AutoCopyCheckBox.IsChecked ?? false;
        set => AutoCopyCheckBox.IsChecked = value;
    }

    // Tools


    private void TransparentNotesCheckBox_Checked(object sender, RoutedEventArgs e)
    {
        bool subSettingsEnabled = TransparentNotesCheckBox.IsChecked ?? false;
        OpaqueWhenFocusedCheckBox.IsEnabled = subSettingsEnabled;
        OpaqueWhenFocusedCheckBox.IsEnabled = subSettingsEnabled;

    }

    private void NewLineAtEndCheckBox_Checked(object sender, RoutedEventArgs e)
    {
        KeepNewLineAtEndVisibleCheckBox.IsEnabled = NewLineAtEndCheckBox.IsChecked ?? false;
    }
}
