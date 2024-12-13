﻿using System;
using System.Text.Json;

using PinnyNotes.WpfUi.Enums;
using PinnyNotes.WpfUi.Views.Controls;

namespace PinnyNotes.WpfUi.Tools;

public partial class JsonTool : BaseTool, ITool
{
    public ToolStates State { get; }

    public enum ToolActions
    {
        JsonPrettify
    }

    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        WriteIndented = true
    };

    public JsonTool(NoteTextBoxControl noteTextBox, ToolStates state) : base(noteTextBox)
    {
        State = state;
        _name = "JSON";
        _menuActions.Add(new("Prettify", JsonPrettifyMenuAction));
    }

    private void JsonPrettifyMenuAction(object sender, EventArgs e) => ApplyFunctionToNoteText(ModifyTextCallback, ToolActions.JsonPrettify);

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
