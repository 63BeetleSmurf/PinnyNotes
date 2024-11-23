using System;
using System.Windows;

namespace PinnyNotes.WpfUi.Tools;

public class ToolMenuAction(string name, RoutedEventHandler? eventHandler = null)
{
    public string Name { get; set; } = name;
    public RoutedEventHandler? EventHandler { get; set; } = eventHandler;
}
