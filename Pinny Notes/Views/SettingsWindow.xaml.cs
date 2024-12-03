using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Helpers;

namespace PinnyNotes.WpfUi.Views;

public partial class SettingsWindow : Window
{
    private Window _lastOwner;

    public SettingsWindow()
    {
        InitializeComponent();
        _lastOwner = Owner;
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

    private void PopulateComboBox<TEnum>(ComboBox comboBox, IEnumerable<KeyValuePair<TEnum, string>> items)
    {
        comboBox.Items.Clear();
        foreach (KeyValuePair<TEnum, string> item in items)
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
    public void PopulateStartupPositions(IEnumerable<KeyValuePair<StartupPositions, string>> items)
        => PopulateComboBox(StartupPositionComboBox, items);

    public MinimizeModes MinimizeMode
    {
        get => (MinimizeModes)MinimizeModeComboBox.SelectedValue;
        set => MinimizeModeComboBox.SelectedValue = value;
    }
    public void PopulateMinimizeModes(IEnumerable<KeyValuePair<MinimizeModes, string>> items)
        => PopulateComboBox(MinimizeModeComboBox, items);

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

    public ColorModes ColorMode
    {
        get => (ColorModes)ColorModeComboBox.SelectedValue;
        set => ColorModeComboBox.SelectedValue = value;
    }
    public void PopulateColorModes(IEnumerable<KeyValuePair<ColorModes, string>> items)
        => PopulateComboBox(ColorModeComboBox, items);

    // - Transparency
    public TransparencyModes TransparencyMode
    {
        get => (TransparencyModes)TransparencyModeComboBox.SelectedValue;
        set => TransparencyModeComboBox.SelectedValue = value;
    }
    public void PopulateTransparencyModes(IEnumerable<KeyValuePair<TransparencyModes, string>> items)
        => PopulateComboBox(TransparencyModeComboBox, items);

    public bool OpaqueWhenFocused
    {
        get => OpaqueWhenFocusedCheckBox.IsChecked ?? false;
        set => OpaqueWhenFocusedCheckBox.IsChecked = value;
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
    #region Tools

    public void PopulateToolStates(IEnumerable<KeyValuePair<ToolStates, string>> items)
    {
        // TODO: Probably could use a resource or something here
        PopulateComboBox(Base64ToolStateComboBox, items);
        PopulateComboBox(BracketToolStateComboBox, items);
        PopulateComboBox(CaseToolStateComboBox, items);
        PopulateComboBox(DateTimeToolStateComboBox, items);
        PopulateComboBox(GibberishToolStateComboBox, items);
        PopulateComboBox(HashToolStateComboBox, items);
        PopulateComboBox(HtmlEntityToolStateComboBox, items);
        PopulateComboBox(IndentToolStateComboBox, items);
        PopulateComboBox(JoinToolStateComboBox, items);
        PopulateComboBox(JsonToolStateComboBox, items);
        PopulateComboBox(ListToolStateComboBox, items);
        PopulateComboBox(QuoteToolStateComboBox, items);
        PopulateComboBox(RemoveToolStateComboBox, items);
        PopulateComboBox(SlashToolStateComboBox, items);
        PopulateComboBox(SortToolStateComboBox, items);
        PopulateComboBox(SplitToolStateComboBox, items);
        PopulateComboBox(TrimToolStateComboBox, items);
    }

    public ToolStates Base64ToolState
    {
        get => (ToolStates)Base64ToolStateComboBox.SelectedValue;
        set => Base64ToolStateComboBox.SelectedValue = value;
    }

    public ToolStates BracketToolState
    {
        get => (ToolStates)BracketToolStateComboBox.SelectedValue;
        set => BracketToolStateComboBox.SelectedValue = value;
    }

    public ToolStates CaseToolState
    {
        get => (ToolStates)CaseToolStateComboBox.SelectedValue;
        set => CaseToolStateComboBox.SelectedValue = value;
    }

    public ToolStates DateTimeToolState
    {
        get => (ToolStates)DateTimeToolStateComboBox.SelectedValue;
        set => DateTimeToolStateComboBox.SelectedValue = value;
    }

    public ToolStates GibberishToolState
    {
        get => (ToolStates)GibberishToolStateComboBox.SelectedValue;
        set => GibberishToolStateComboBox.SelectedValue = value;
    }

    public ToolStates HashToolState
    {
        get => (ToolStates)HashToolStateComboBox.SelectedValue;
        set => HashToolStateComboBox.SelectedValue = value;
    }

    public ToolStates HtmlEntityToolState
    {
        get => (ToolStates)HtmlEntityToolStateComboBox.SelectedValue;
        set => HtmlEntityToolStateComboBox.SelectedValue = value;
    }

    public ToolStates IndentToolState
    {
        get => (ToolStates)IndentToolStateComboBox.SelectedValue;
        set => IndentToolStateComboBox.SelectedValue = value;
    }

    public ToolStates JoinToolState
    {
        get => (ToolStates)JoinToolStateComboBox.SelectedValue;
        set => JoinToolStateComboBox.SelectedValue = value;
    }

    public ToolStates JsonToolState
    {
        get => (ToolStates)JsonToolStateComboBox.SelectedValue;
        set => JsonToolStateComboBox.SelectedValue = value;
    }

    public ToolStates ListToolState
    {
        get => (ToolStates)ListToolStateComboBox.SelectedValue;
        set => ListToolStateComboBox.SelectedValue = value;
    }

    public ToolStates QuoteToolState
    {
        get => (ToolStates)QuoteToolStateComboBox.SelectedValue;
        set => QuoteToolStateComboBox.SelectedValue = value;
    }

    public ToolStates RemoveToolState
    {
        get => (ToolStates)RemoveToolStateComboBox.SelectedValue;
        set => RemoveToolStateComboBox.SelectedValue = value;
    }

    public ToolStates SlashToolState
    {
        get => (ToolStates)SlashToolStateComboBox.SelectedValue;
        set => SlashToolStateComboBox.SelectedValue = value;
    }

    public ToolStates SortToolState
    {
        get => (ToolStates)SortToolStateComboBox.SelectedValue;
        set => SortToolStateComboBox.SelectedValue = value;
    }

    public ToolStates SplitToolState
    {
        get => (ToolStates)SplitToolStateComboBox.SelectedValue;
        set => SplitToolStateComboBox.SelectedValue = value;
    }

    public ToolStates TrimToolState
    {
        get => (ToolStates)TrimToolStateComboBox.SelectedValue;
        set => TrimToolStateComboBox.SelectedValue = value;
    }

    #endregion

    public event EventHandler? OkClicked;
    public event EventHandler? CancelClicked;
    public event EventHandler? ApplyClicked;

    private void TransparencyModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        object? selectedValue = TransparencyModeComboBox.SelectedValue;
        OpaqueWhenFocusedCheckBox.IsEnabled = (selectedValue != null && (TransparencyModes)selectedValue != TransparencyModes.Disabled);
    }

    private void NewLineAtEndCheckBox_Checked(object sender, RoutedEventArgs e)
    {
        KeepNewLineAtEndVisibleCheckBox.IsEnabled = NewLineAtEndCheckBox.IsChecked ?? false;
    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        OkClicked?.Invoke(sender, e);
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        CancelClicked?.Invoke(sender, e);
    }

    private void ApplyButton_Click(object sender, RoutedEventArgs e)
    {
        ApplyClicked?.Invoke(sender, e);
    }
}
