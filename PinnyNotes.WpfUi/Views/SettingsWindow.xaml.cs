using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using PinnyNotes.Core.Enums;
using PinnyNotes.WpfUi.Controls.TitleBar;
using PinnyNotes.WpfUi.Helpers;
using PinnyNotes.WpfUi.Messages;
using PinnyNotes.WpfUi.Services;
using PinnyNotes.WpfUi.ViewModels;

namespace PinnyNotes.WpfUi.Views;

public partial class SettingsWindow : Window
{
    private readonly MessengerService _messengerService;

    private readonly SettingsViewModel _viewModel;

    private Window _lastOwner;

    private readonly Panel[] _allowedPanels;

    private class DragData(FrameworkElement item, Panel parentPanel)
    {
        public FrameworkElement Item { get; set; } = item;
        public Panel ParentPanel { get; set; } = parentPanel;
    }

    public SettingsWindow(MessengerService messengerService, SettingsViewModel viewModel)
    {
        _messengerService = messengerService;
        _messengerService.Subscribe<WindowActionMessage>(OnWindowActionMessage);

        _viewModel = viewModel;
        DataContext = _viewModel;

        InitializeComponent();
        _allowedPanels = [SelectedTitleBarItemsPanel, AvailableTitleBarItemsPanel];

        Activated += Window_Activated;

        _lastOwner = Owner;

        InitializeNoteTitleBarItemsPanels();
    }

    private void InitializeNoteTitleBarItemsPanels()
    {
        SolidColorBrush iconBrush = new(Color.FromRgb(0x46, 0x46, 0x46));

        SelectedTitleBarItemsPanel.AllowDrop = true;
        SelectedTitleBarItemsPanel.DragOver += TitleBarItemsPanel_DragOver;
        SelectedTitleBarItemsPanel.Drop += TitleBarItemsPanel_Drop;

        AvailableTitleBarItemsPanel.AllowDrop = true;
        AvailableTitleBarItemsPanel.DragOver += TitleBarItemsPanel_DragOver;
        AvailableTitleBarItemsPanel.Drop += TitleBarItemsPanel_Drop;

        BaseTitleBarSpacerItem.PreviewMouseLeftButtonDown += TitleBarItem_PreviewMouseDown;

        foreach (NoteTitleBarItem titleBarItem in _viewModel.NoteSettings.TitleBarItems)
        {
            FrameworkElement itemElement;
            switch (titleBarItem)
            {
                case NoteTitleBarItem.Spacer:
                    itemElement = new TitleBarBorderSpacer();
                    break;
                case NoteTitleBarItem.NewNoteButton:
                    itemElement = new NewNoteButton()
                    {
                        IconStroke = iconBrush,
                        Margin = new Thickness(5,10,5,10)
                    };
                    break;
                case NoteTitleBarItem.PinButton:
                    itemElement = new PinButton()
                    {
                        IconFill = iconBrush,
                        Margin = new Thickness(5, 10, 5, 10)
                    };
                    break;
                case NoteTitleBarItem.CloseButton:
                    itemElement = new CloseButton()
                    {
                        IconStroke = iconBrush,
                        Margin = new Thickness(5, 10, 5, 10)
                    };
                    break;
                default:
                    continue;
            }
            itemElement.PreviewMouseLeftButtonDown += TitleBarItem_PreviewMouseDown;

            SelectedTitleBarItemsPanel.Children.Add(itemElement);
        }
    }

    private void Window_Activated(object? sender, EventArgs e)
    {
        if (Owner is null)
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
            Rect currentScreenBounds = ScreenHelper.GetCurrentScreenBounds(
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

    private void OnWindowActionMessage(WindowActionMessage message)
    {
        if (message.Action == WindowAction.Activate)
        {
            WindowState = WindowState.Normal;
            Activate();
        }
    }

    private void TitleBarItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is not FrameworkElement draggedItem || draggedItem.Parent is not Panel parentPanel)
        {
            return;
        }

        DragData dragData = new(draggedItem, parentPanel);

        DragDrop.DoDragDrop(draggedItem, dragData, DragDropEffects.Move);
    }

    private void TitleBarItemsPanel_DragOver(object sender, DragEventArgs e)
    {
        if (e.Data.GetData(typeof(DragData)) is not DragData dragData || sender is not Panel currentPanel || !_allowedPanels.Contains(currentPanel))
        {
            return;
        }

        Point mousePos = e.GetPosition(currentPanel);
        int targetIndex = GetInsertionIndex(currentPanel, mousePos);

        if (currentPanel != dragData.ParentPanel)
        {
            if (dragData.Item is TitleBarBorderSpacer spacerItem && spacerItem == BaseTitleBarSpacerItem)
            {
                TitleBarBorderSpacer newSpacerItem = new();
                newSpacerItem.PreviewMouseLeftButtonDown += TitleBarItem_PreviewMouseDown;

                currentPanel.Children.Insert(targetIndex, newSpacerItem);
                dragData.Item = newSpacerItem;
            }
            else
            {
                dragData.ParentPanel.Children.Remove(dragData.Item);
                currentPanel.Children.Insert(targetIndex, dragData.Item);
            }

            dragData.ParentPanel = currentPanel;
        }
        else if (currentPanel == dragData.ParentPanel)
        {
            int currentIndex = currentPanel.Children.IndexOf(dragData.Item);

            if (currentIndex >= 0 && currentIndex != targetIndex)
            {
                currentPanel.Children.Remove(dragData.Item);

                targetIndex = Math.Clamp(targetIndex, 0, currentPanel.Children.Count);

                currentPanel.Children.Insert(targetIndex, dragData.Item);
            }
        }
        else
        {
            return;
        }

        e.Effects = DragDropEffects.Move;
        e.Handled = true;
    }

    private void TitleBarItemsPanel_Drop(object sender, DragEventArgs e)
    {
        if (e.Data.GetData(typeof(DragData)) is not DragData dragData)
        {
            return;
        }

        if (dragData.Item is TitleBarBorderSpacer spacerItem && spacerItem != BaseTitleBarSpacerItem && spacerItem.Parent == BaseTitleBarSpacerItem.Parent)
        {
            dragData.ParentPanel.Children.Remove(spacerItem);
        }

        _viewModel.NoteSettings.TitleBarItems = GetSelectedTitleBarItems();

        e.Handled = true;
    }

    private static int GetInsertionIndex(Panel panel, Point mousePoint)
    {
        for (int i = 0; i < panel.Children.Count; i++)
        {
            if (panel.Children[i] is not FrameworkElement childItem)
            {
                continue;
            }

            Point position = childItem.TranslatePoint(new Point(0, 0), panel);

            if (mousePoint.X <= position.X + childItem.Width)
            {
                return i;
            }
        }

        return panel.Children.Count;
    }

    private NoteTitleBarItem[] GetSelectedTitleBarItems()
    {
        List<NoteTitleBarItem> selectedTitleBarItems = [];

        foreach (UIElement item in SelectedTitleBarItemsPanel.Children)
        {
            NoteTitleBarItem? titleBarItem = item switch
            {
                TitleBarBorderSpacer => NoteTitleBarItem.Spacer,
                NewNoteButton => NoteTitleBarItem.NewNoteButton,
                PinButton => NoteTitleBarItem.PinButton,
                CloseButton => NoteTitleBarItem.CloseButton,
                _ => null
            };

            if (titleBarItem is not null)
            {
                selectedTitleBarItems.Add(titleBarItem.Value);
            }
        }

        return [.. selectedTitleBarItems];
    }
}
