using System.Windows;

namespace PinnyNotes.WpfUi.Messages;

public record OpenSettingsWindowMessage(
    Window? Owner = null
);
