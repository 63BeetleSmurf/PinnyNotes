﻿using System;
using System.Text.Json;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Commands;
using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Properties;

namespace PinnyNotes.WpfUi.Tools;

public class JsonTool : BaseTool, ITool
{
    private enum ToolActions
    {
        JsonPrettify
    }

    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        WriteIndented = true
    };

    public ToolStates State => (ToolStates)Settings.Default.JsonToolState;

    public JsonTool(TextBox noteTextBox) : base(noteTextBox)
    {
        InitializeMenuItem(
            "JSON",
            [
                new ToolMenuAction("Prettify", new RelayCommand(() => MenuAction(ToolActions.JsonPrettify)))
            ]
        );
    }

    private void MenuAction(ToolActions action)
    {
        ApplyFunctionToNoteText(ModifyTextCallback, action);
    }

    private string ModifyTextCallback(string text, Enum action)
    {
        try
        {
            switch (action)
            {
                case ToolActions.JsonPrettify:
                    object? jsonObject = JsonSerializer.Deserialize<object>(text);
                    if (jsonObject != null)
                        return JsonSerializer.Serialize<object>(jsonObject, _jsonSerializerOptions);
                    break;
            }
        }
        catch
        {
        }

        return text;
    }
}
