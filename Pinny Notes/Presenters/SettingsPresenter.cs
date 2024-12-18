using System;
using System.Windows;

using PinnyNotes.WpfUi.Helpers;
using PinnyNotes.WpfUi.Models;
using PinnyNotes.WpfUi.Views;

namespace PinnyNotes.WpfUi.Presenters;

public class SettingsPresenter
{
    private readonly ApplicationManager _applicationManager;
    private readonly SettingsModel _model;
    private readonly SettingsWindow _view;

    public SettingsPresenter(ApplicationManager applicationManager, SettingsModel model, SettingsWindow view)
    {
        _applicationManager = applicationManager;
        _model = model;
        _view = view;

        _view.WindowClosing += OnWindowClosing;

        _view.OkClicked += OnOkClicked;
        _view.CancelClicked += OnCancelClicked;
        _view.ApplyClicked += OnApplyClicked;

        AddChangeHandlers();
    }

    public event EventHandler? SettingsSaved;

    public void ShowWindow(Window? owner = null)
    {
        _view.Owner = owner;

        PositionWindow();

        LoadSettings();
        _view.ApplyButton.IsEnabled = false;

        if (_view.IsVisible)
            _view.Activate();
        else
            _view.Show();
    }

    private void OnWindowClosing(object? sender, EventArgs e)
    {
        HideWindow();
    }

    private void OnOkClicked(object? sender, EventArgs e)
    {
        SaveSettings();
        HideWindow();
    }

    private void OnCancelClicked(object? sender, EventArgs e)
    {
        HideWindow();
    }

    private void OnApplyClicked(object? sender, EventArgs e)
    {
        SaveSettings();
    }

    private void OnSettingsChanged(object? sender, EventArgs e)
    {
        _view.ApplyButton.IsEnabled = true;
    }

    private void PositionWindow()
    {
        if (_view.Owner == null)
        {
            _view.Left = SystemParameters.PrimaryScreenWidth / 2 - _view.Width / 2;
            _view.Top = SystemParameters.PrimaryScreenHeight / 2 - _view.Height / 2;
        }
        else
        {
            Point position = new(
                (_view.Owner.Left + _view.Owner.Width / 2) - _view.Width / 2,
                (_view.Owner.Top + _view.Owner.Height / 2) - _view.Height / 2
            );
            System.Drawing.Rectangle currentScreenBounds = ScreenHelper.GetCurrentScreenBounds(
                ScreenHelper.GetWindowHandle(_view.Owner)
            );

            if (position.X < currentScreenBounds.Left)
                position.X = currentScreenBounds.Left;
            else if (position.X + _view.Width > currentScreenBounds.Right)
                position.X = currentScreenBounds.Right - _view.Width;

            if (position.Y < currentScreenBounds.Top)
                position.Y = currentScreenBounds.Top;
            else if (position.Y + _view.Height > currentScreenBounds.Bottom)
                position.Y = currentScreenBounds.Bottom - _view.Height;

            _view.Left = position.X;
            _view.Top = position.Y;
        }
    }

    private void LoadSettings()
    {
        PropertiesHelper.CopyMatchingProperties(_model, _view);
    }

    private void SaveSettings()
    {
        _view.ApplyButton.IsEnabled = false;

        PropertiesHelper.CopyMatchingProperties(_view, _model);

        SettingsSaved?.Invoke(this, EventArgs.Empty);
    }

    private void HideWindow()
    {
        _view.Owner = null;
        _view.Hide();
    }

    private void AddChangeHandlers()
    {
        _view.Application_TrayIconCheckBox.Click += OnSettingsChanged;
        _view.Application_NotesInTaskbarCheckBox.Click += OnSettingsChanged;
        _view.Application_CheckForUpdatesCheckBox.Click += OnSettingsChanged;

        _view.Notes_DefaultWidthTextBox.TextChanged += OnSettingsChanged;
        _view.Notes_DefaultHeightTextBox.TextChanged += OnSettingsChanged;
        _view.Notes_StartupPositionComboBox.SelectionChanged += OnSettingsChanged;
        _view.Notes_MinimizeModeComboBox.SelectionChanged += OnSettingsChanged;
        _view.Notes_HideTitleBarCheckBox.Click += OnSettingsChanged;
        _view.Notes_DefaultThemeColorComboBox.SelectionChanged += OnSettingsChanged;
        _view.Notes_ColorModeComboBox.SelectionChanged += OnSettingsChanged;
        _view.Notes_TransparencyModeComboBox.SelectionChanged += OnSettingsChanged;
        _view.Notes_OpaqueWhenFocusedCheckBox.Click += OnSettingsChanged;
        _view.Notes_TransparentOpacityTextBox.TextChanged += OnSettingsChanged;
        _view.Notes_OpaqueOpacityTextBox.TextChanged += OnSettingsChanged;

        _view.Editor_UseMonoFontCheckBox.Click += OnSettingsChanged;
        _view.Editor_MonoFontFamilyTextBox.TextChanged += OnSettingsChanged;
        _view.Editor_SpellCheckCheckBox.Click += OnSettingsChanged;
        _view.Editor_AutoIndentCheckBox.Click += OnSettingsChanged;
        _view.Editor_NewLineAtEndCheckBox.Click += OnSettingsChanged;
        _view.Editor_KeepNewLineVisibleCheckBox.Click += OnSettingsChanged;
        _view.Editor_TabsToSpacesCheckBox.Click += OnSettingsChanged;
        _view.Editor_ConvertIndentationOnPasteCheckBox.Click += OnSettingsChanged;
        _view.Editor_TabWidthTextBox.TextChanged += OnSettingsChanged;
        _view.Editor_MiddleClickPasteCheckBox.Click += OnSettingsChanged;
        _view.Editor_TrimPastedTextCheckBox.Click += OnSettingsChanged;
        _view.Editor_TrimCopiedTextCheckBox.Click += OnSettingsChanged;
        _view.Editor_CopyHighlightedTextCheckBox.Click += OnSettingsChanged;

        _view.Tool_Base64StateComboBox.SelectionChanged += OnSettingsChanged;
        _view.Tool_BracketStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.Tool_CaseStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.Tool_DateTimeStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.Tool_GibberishStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.Tool_HashStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.Tool_HtmlEntityStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.Tool_IndentStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.Tool_JoinStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.Tool_JsonStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.Tool_ListStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.Tool_QuoteStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.Tool_RemoveStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.Tool_SlashStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.Tool_SortStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.Tool_SplitStateComboBox.SelectionChanged += OnSettingsChanged;
        _view.Tool_TrimStateComboBox.SelectionChanged += OnSettingsChanged;
    }
}
