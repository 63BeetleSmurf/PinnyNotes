﻿using System.Windows;

using PinnyNotes.WpfUi.Helpers;
using PinnyNotes.WpfUi.Services;
using PinnyNotes.WpfUi.ViewModels;

namespace PinnyNotes.WpfUi.Views;

public partial class SettingsWindow : Window
{
    private Window _lastOwner;

    public SettingsWindow(MessengerService messenger)
    {
        DataContext = new SettingsViewModel(messenger);
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
}
