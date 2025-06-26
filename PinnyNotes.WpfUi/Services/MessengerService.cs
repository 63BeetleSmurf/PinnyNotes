using System;

namespace PinnyNotes.WpfUi.Services;

public class MessengerService
{
    public event Action<string, object>? NotifySettingChanged;

    public void SendSettingChangedNotification(string settingName, object settingValue)
        => NotifySettingChanged?.Invoke(settingName, settingValue);
}
