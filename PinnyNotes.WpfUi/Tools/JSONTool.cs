using System;
using System.Text.Json;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Helpers;

namespace PinnyNotes.WpfUi.Tools;

public static class JsonTool
{
    private static JsonSerializerOptions _jsonSerializerOptions = new()
    {
        WriteIndented = true
    };

    public const string Name = "JSON";

    public static MenuItem MenuItem
        => ToolHelper.GetToolMenuItem(
            Name,
            [
                new("Prettify", OnPrettifyMenuItemClick)
            ]
        );

    private static void OnPrettifyMenuItemClick(object sender, EventArgs e)
        => ToolHelper.ApplyFunctionToNoteText(ToolHelper.GetNoteTextBoxFromSender(sender), Prettify);

    private static string Prettify(string text, object? additionalParam)
    {
        try
        {
            object? jsonObject = JsonSerializer.Deserialize<object>(text);
            if (jsonObject != null)
                return JsonSerializer.Serialize(jsonObject, _jsonSerializerOptions);
        }
        catch
        {
        }

        return text;
    }
}
