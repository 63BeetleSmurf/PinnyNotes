﻿using System;
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

        _view.Closing += OnWindowClosing;

        _view.OkButton.Click += OnOkButtonClick;
        _view.CancelButton.Click += OnCancelButtonClick;
        _view.ApplyButton.Click += OnApplyButtonClick;

        _view.SettingsChanged += OnSettingsChanged;
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

    private void OnOkButtonClick(object? sender, EventArgs e)
    {
        SaveSettings();
        HideWindow();
    }

    private void OnCancelButtonClick(object? sender, EventArgs e)
    {
        HideWindow();
    }

    private void OnApplyButtonClick(object? sender, EventArgs e)
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
}
