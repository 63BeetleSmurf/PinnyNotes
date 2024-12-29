namespace PinnyNotes.WpfUi.Models;

public class GroupModel
{
    public void Initialize(SettingsModel settings, NoteModel? parent = null)
    {
        Settings = settings;
    }

    public int Id { get; set; }

    public int? SettingsId { get; set; }
    public SettingsModel Settings { get; set; } = null!; // Set in Initialize()

}
