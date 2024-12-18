using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Helpers;

namespace PinnyNotes.WpfUi.Views;

public partial class SettingsWindow : Window
{
    public SettingsWindow()
    {
        InitializeComponent();

        Notes_StartupPositionComboBox.ItemsSource = SettingsHelper.StartupPositionsList;
        Notes_MinimizeModeComboBox.ItemsSource = SettingsHelper.MinimizeModeList;
        Notes_DefaultThemeColorComboBox.ItemsSource = SettingsHelper.DefaultThemeColorList;
        Notes_ColorModeComboBox.ItemsSource = SettingsHelper.ColorModeList;
        Notes_TransparencyModeComboBox.ItemsSource = SettingsHelper.TransparencyModeList;

        // TODO: Probably could use a resource or something here
        // - Tried ObjectDataProvider, got incompatible type errors when setting ItemsSource so gave up.
        Tool_Base64StateComboBox.ItemsSource = SettingsHelper.ToolStateList;
        Tool_BracketStateComboBox.ItemsSource = SettingsHelper.ToolStateList;
        Tool_CaseStateComboBox.ItemsSource = SettingsHelper.ToolStateList;
        Tool_DateTimeStateComboBox.ItemsSource = SettingsHelper.ToolStateList;
        Tool_GibberishStateComboBox.ItemsSource = SettingsHelper.ToolStateList;
        Tool_HashStateComboBox.ItemsSource = SettingsHelper.ToolStateList;
        Tool_HtmlEntityStateComboBox.ItemsSource = SettingsHelper.ToolStateList;
        Tool_IndentStateComboBox.ItemsSource = SettingsHelper.ToolStateList;
        Tool_JoinStateComboBox.ItemsSource = SettingsHelper.ToolStateList;
        Tool_JsonStateComboBox.ItemsSource = SettingsHelper.ToolStateList;
        Tool_ListStateComboBox.ItemsSource = SettingsHelper.ToolStateList;
        Tool_QuoteStateComboBox.ItemsSource = SettingsHelper.ToolStateList;
        Tool_RemoveStateComboBox.ItemsSource = SettingsHelper.ToolStateList;
        Tool_SlashStateComboBox.ItemsSource = SettingsHelper.ToolStateList;
        Tool_SortStateComboBox.ItemsSource = SettingsHelper.ToolStateList;
        Tool_SplitStateComboBox.ItemsSource = SettingsHelper.ToolStateList;
        Tool_TrimStateComboBox.ItemsSource = SettingsHelper.ToolStateList;
    }

    // Application
    // - General
    public bool Application_TrayIcon
    {
        get => Application_TrayIconCheckBox.IsChecked ?? false;
        set => Application_TrayIconCheckBox.IsChecked = value;
    }

    public bool Application_NotesInTaskbar
    {
        get => Application_NotesInTaskbarCheckBox.IsChecked ?? false;
        set => Application_NotesInTaskbarCheckBox.IsChecked = value;
    }

    public bool Application_CheckForUpdates
    {
        get => Application_CheckForUpdatesCheckBox.IsChecked ?? false;
        set => Application_CheckForUpdatesCheckBox.IsChecked = value;
    }

    // Notes
    // - General
    public int Notes_DefaultWidth
    {
        get => int.Parse(Notes_DefaultWidthTextBox.Text);
        set => Notes_DefaultWidthTextBox.Text = value.ToString();
    }

    public int Notes_DefaultHeight
    {
        get => int.Parse(Notes_DefaultHeightTextBox.Text);
        set => Notes_DefaultHeightTextBox.Text = value.ToString();
    }

    public StartupPositions Notes_StartupPosition
    {
        get => (StartupPositions)Notes_StartupPositionComboBox.SelectedValue;
        set => Notes_StartupPositionComboBox.SelectedValue = value;
    }

    public MinimizeModes Notes_MinimizeMode
    {
        get => (MinimizeModes)Notes_MinimizeModeComboBox.SelectedValue;
        set => Notes_MinimizeModeComboBox.SelectedValue = value;
    }

    public bool Notes_HideTitleBar
    {
        get => Notes_HideTitleBarCheckBox.IsChecked ?? false;
        set => Notes_HideTitleBarCheckBox.IsChecked = value;
    }

    // - Theme
    public string Notes_DefaultThemeColorKey
    {
        get => (string)Notes_DefaultThemeColorComboBox.SelectedValue;
        set => Notes_DefaultThemeColorComboBox.SelectedValue = value;
    }

    public ColorModes Notes_ColorMode
    {
        get => (ColorModes)Notes_ColorModeComboBox.SelectedValue;
        set => Notes_ColorModeComboBox.SelectedValue = value;
    }

    // - Transparency
    public TransparencyModes Notes_TransparencyMode
    {
        get => (TransparencyModes)Notes_TransparencyModeComboBox.SelectedValue;
        set => Notes_TransparencyModeComboBox.SelectedValue = value;
    }

    public bool Notes_OpaqueWhenFocused
    {
        get => Notes_OpaqueWhenFocusedCheckBox.IsChecked ?? false;
        set => Notes_OpaqueWhenFocusedCheckBox.IsChecked = value;
    }

    public double Notes_TransparentOpacity
    {
        get => double.Parse(Notes_TransparentOpacityTextBox.Text);
        set => Notes_TransparentOpacityTextBox.Text = value.ToString();
    }

    public double Notes_OpaqueOpacity
    {
        get => double.Parse(Notes_OpaqueOpacityTextBox.Text);
        set => Notes_OpaqueOpacityTextBox.Text = value.ToString();
    }

    // Editor
    // - General
    public bool Editor_UseMonoFont
    {
        get => Editor_UseMonoFontCheckBox.IsChecked ?? false;
        set => Editor_UseMonoFontCheckBox.IsChecked = value;
    }

    public string Editor_MonoFontFamily
    {
        get => Editor_MonoFontFamilyTextBox.Text;
        set => Editor_MonoFontFamilyTextBox.Text = value;
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
    public ToolStates Tool_Base64State
    {
        get => (ToolStates)Tool_Base64StateComboBox.SelectedValue;
        set => Tool_Base64StateComboBox.SelectedValue = value;
    }

    public ToolStates Tool_BracketState
    {
        get => (ToolStates)Tool_BracketStateComboBox.SelectedValue;
        set => Tool_BracketStateComboBox.SelectedValue = value;
    }

    public ToolStates Tool_CaseState
    {
        get => (ToolStates)Tool_CaseStateComboBox.SelectedValue;
        set => Tool_CaseStateComboBox.SelectedValue = value;
    }

    public ToolStates Tool_DateTimeState
    {
        get => (ToolStates)Tool_DateTimeStateComboBox.SelectedValue;
        set => Tool_DateTimeStateComboBox.SelectedValue = value;
    }

    public ToolStates Tool_GibberishState
    {
        get => (ToolStates)Tool_GibberishStateComboBox.SelectedValue;
        set => Tool_GibberishStateComboBox.SelectedValue = value;
    }

    public ToolStates Tool_HashState
    {
        get => (ToolStates)Tool_HashStateComboBox.SelectedValue;
        set => Tool_HashStateComboBox.SelectedValue = value;
    }

    public ToolStates Tool_HtmlEntityState
    {
        get => (ToolStates)Tool_HtmlEntityStateComboBox.SelectedValue;
        set => Tool_HtmlEntityStateComboBox.SelectedValue = value;
    }

    public ToolStates Tool_IndentState
    {
        get => (ToolStates)Tool_IndentStateComboBox.SelectedValue;
        set => Tool_IndentStateComboBox.SelectedValue = value;
    }

    public ToolStates Tool_JoinState
    {
        get => (ToolStates)Tool_JoinStateComboBox.SelectedValue;
        set => Tool_JoinStateComboBox.SelectedValue = value;
    }

    public ToolStates Tool_JsonState
    {
        get => (ToolStates)Tool_JsonStateComboBox.SelectedValue;
        set => Tool_JsonStateComboBox.SelectedValue = value;
    }

    public ToolStates Tool_ListState
    {
        get => (ToolStates)Tool_ListStateComboBox.SelectedValue;
        set => Tool_ListStateComboBox.SelectedValue = value;
    }

    public ToolStates Tool_QuoteState
    {
        get => (ToolStates)Tool_QuoteStateComboBox.SelectedValue;
        set => Tool_QuoteStateComboBox.SelectedValue = value;
    }

    public ToolStates Tool_RemoveState
    {
        get => (ToolStates)Tool_RemoveStateComboBox.SelectedValue;
        set => Tool_RemoveStateComboBox.SelectedValue = value;
    }

    public ToolStates Tool_SlashState
    {
        get => (ToolStates)Tool_SlashStateComboBox.SelectedValue;
        set => Tool_SlashStateComboBox.SelectedValue = value;
    }

    public ToolStates Tool_SortState
    {
        get => (ToolStates)Tool_SortStateComboBox.SelectedValue;
        set => Tool_SortStateComboBox.SelectedValue = value;
    }

    public ToolStates Tool_SplitState
    {
        get => (ToolStates)Tool_SplitStateComboBox.SelectedValue;
        set => Tool_SplitStateComboBox.SelectedValue = value;
    }

    public ToolStates Tool_TrimState
    {
        get => (ToolStates)Tool_TrimStateComboBox.SelectedValue;
        set => Tool_TrimStateComboBox.SelectedValue = value;
    }

    public event EventHandler? WindowClosing;
    public event EventHandler? OkClicked;
    public event EventHandler? CancelClicked;
    public event EventHandler? ApplyClicked;

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
}
