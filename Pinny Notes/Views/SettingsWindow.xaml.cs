using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Enums;

namespace PinnyNotes.WpfUi.Views;

public partial class SettingsWindow : Window
{
    public SettingsWindow()
    {
        InitializeComponent();
    }

    // Application
    // - General
    public bool Applicaiton_TrayIcon
    {
        get => Applicaiton_TrayIconCheckBox.IsChecked ?? false;
        set => Applicaiton_TrayIconCheckBox.IsChecked = value;
    }

    public bool Applicaiton_NotesInTaskbar
    {
        get => Applicaiton_NotesInTaskbarCheckBox.IsChecked ?? false;
        set => Applicaiton_NotesInTaskbarCheckBox.IsChecked = value;
    }

    public bool Applicaiton_CheckForUpdates
    {
        get => Applicaiton_CheckForUpdatesCheckBox.IsChecked ?? false;
        set => Applicaiton_CheckForUpdatesCheckBox.IsChecked = value;
    }

    // Notes
    // - General
    public StartupPositions Notes_StartupPosition
    {
        get => (StartupPositions)Notes_StartupPositionComboBox.SelectedValue;
        set => Notes_StartupPositionComboBox.SelectedValue = value;
    }
    public void PopulateNotes_StartupPositions(IEnumerable<KeyValuePair<StartupPositions, string>> items)
        => PopulateComboBox(Notes_StartupPositionComboBox, items);

    public MinimizeModes Notes_MinimizeMode
    {
        get => (MinimizeModes)Notes_MinimizeModeComboBox.SelectedValue;
        set => Notes_MinimizeModeComboBox.SelectedValue = value;
    }
    public void PopulateNotes_MinimizeModes(IEnumerable<KeyValuePair<MinimizeModes, string>> items)
        => PopulateComboBox(Notes_MinimizeModeComboBox, items);

    public bool Notes_HideTitleBar
    {
        get => Notes_HideTitleBarCheckBox.IsChecked ?? false;
        set => Notes_HideTitleBarCheckBox.IsChecked = value;
    }

    // - Theme
    public string Notes_DefaultColor
    {
        get => (string)Notes_DefaultColorComboBox.SelectedValue;
        set => Notes_DefaultColorComboBox.SelectedValue = value;
    }
    public void PopulateNotes_DefaultColors(IEnumerable<KeyValuePair<string, string>> items)
        => PopulateComboBox(Notes_MinimizeModeComboBox, items);

    public ColorModes Notes_ColorMode
    {
        get => (ColorModes)Notes_ColorModeComboBox.SelectedValue;
        set => Notes_ColorModeComboBox.SelectedValue = value;
    }
    public void PopulateNotes_ColorModes(IEnumerable<KeyValuePair<ColorModes, string>> items)
        => PopulateComboBox(Notes_ColorModeComboBox, items);

    // - Transparency
    public TransparencyModes Notes_TransparencyMode
    {
        get => (TransparencyModes)Notes_TransparencyModeComboBox.SelectedValue;
        set => Notes_TransparencyModeComboBox.SelectedValue = value;
    }
    public void PopulateNotes_TransparencyModes(IEnumerable<KeyValuePair<TransparencyModes, string>> items)
        => PopulateComboBox(Notes_TransparencyModeComboBox, items);

    public bool Notes_OpaqueWhenFocused
    {
        get => Notes_OpaqueWhenFocusedCheckBox.IsChecked ?? false;
        set => Notes_OpaqueWhenFocusedCheckBox.IsChecked = value;
    }

    // Editor
    // - General
    public bool Editor_MonoFont
    {
        get => Editor_MonoFontCheckBox.IsChecked ?? false;
        set => Editor_MonoFontCheckBox.IsChecked = value;
    }

    public bool Editor_SpellCheck
    {
        get => Editor_SpellCheckCheckBox.IsChecked ?? false;
        set => Editor_SpellCheckCheckBox.IsChecked = value;
    }

    public bool Editor_AutoIndent
    {
        get => Editor_AutoIndentCheckBox.IsChecked ?? false;
        set => Editor_AutoIndentCheckBox.IsChecked = value;
    }

    public bool Editor_NewLineAtEnd
    {
        get => Editor_NewLineAtEndCheckBox.IsChecked ?? false;
        set => Editor_NewLineAtEndCheckBox.IsChecked = value;
    }

    public bool Editor_KeepNewLineVisible
    {
        get => Editor_KeepNewLineVisibleCheckBox.IsChecked ?? false;
        set => Editor_KeepNewLineVisibleCheckBox.IsChecked = value;
    }

    // - Indentation
    public bool Editor_TabsToSpaces
    {
        get => Editor_TabsToSpacesCheckBox.IsChecked ?? false;
        set => Editor_TabsToSpacesCheckBox.IsChecked = value;
    }

    public bool Editor_ConvertIndentationOnPaste
    {
        get => Editor_ConvertIndentationOnPasteCheckBox.IsChecked ?? false;
        set => Editor_ConvertIndentationOnPasteCheckBox.IsChecked = value;
    }

    public int Editor_TabWidth
    {
        get => int.Parse(Editor_TabWidthTextBox.Text);
        set => Editor_TabWidthTextBox.Text = value.ToString();
    }

    // - Copy and Paste

    public bool Editor_MiddleClickPaste
    {
        get => Editor_MiddleClickPasteCheckBox.IsChecked ?? false;
        set => Editor_MiddleClickPasteCheckBox.IsChecked = value;
    }

    public bool Editor_TrimPastedText
    {
        get => Editor_TrimPastedTextCheckBox.IsChecked ?? false;
        set => Editor_TrimPastedTextCheckBox.IsChecked = value;
    }

    public bool Editor_TrimCopiedText
    {
        get => Editor_TrimCopiedTextCheckBox.IsChecked ?? false;
        set => Editor_TrimCopiedTextCheckBox.IsChecked = value;
    }

    public bool Editor_CopyHighlightedText
    {
        get => Editor_CopyHighlightedTextCheckBox.IsChecked ?? false;
        set => Editor_CopyHighlightedTextCheckBox.IsChecked = value;
    }

    // Tools
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

    public event EventHandler? WindowClosing;
    public event EventHandler? OkClicked;
    public event EventHandler? CancelClicked;
    public event EventHandler? ApplyClicked;

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

    private void Window_Closing(object sender, CancelEventArgs e)
    {
        WindowClosing?.Invoke(sender, e);
        e.Cancel = true;
    }

    private void Notes_TransparencyModeComboBoxComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        object? selectedValue = Notes_TransparencyModeComboBox.SelectedValue;
        Notes_OpaqueWhenFocusedCheckBox.IsEnabled = (selectedValue != null && (TransparencyModes)selectedValue != TransparencyModes.Disabled);
    }

    private void Editor_NewLineAtEndCheckBox_Checked(object sender, RoutedEventArgs e)
    {
        Editor_KeepNewLineVisibleCheckBox.IsEnabled = Editor_NewLineAtEndCheckBox.IsChecked ?? false;
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

    private void PopulateComboBox<TKey>(ComboBox comboBox, IEnumerable<KeyValuePair<TKey, string>> items)
    {
        comboBox.Items.Clear();
        foreach (KeyValuePair<TKey, string> item in items)
            comboBox.Items.Add(item);
    }
}
