namespace PinnyNotes.WpfUi.Messages;

public record SettingChangedMessage(
    string SettingName,
    object NewValue
);
