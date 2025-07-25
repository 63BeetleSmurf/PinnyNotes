namespace PinnyNotes.WpfUi.Models;

public class SettingsModel(
    ApplicationSettingsModel applicationSettings,
    NoteSettingsModel noteSettings,
    EditorSettingsModel editorSettings,
    ToolSettingsModel toolSettings
)
{
    public ApplicationSettingsModel ApplicationSettings { get; set; } = applicationSettings;
    public NoteSettingsModel NoteSettings { get; set; } = noteSettings;
    public EditorSettingsModel EditorSettings { get; set; } = editorSettings;
    public ToolSettingsModel ToolSettings { get; set; } = toolSettings;
}
