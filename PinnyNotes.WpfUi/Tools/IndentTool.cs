using CommunityToolkit.Mvvm.Input;
using System;
using System.Windows.Controls;

using PinnyNotes.WpfUi.Properties;

namespace PinnyNotes.WpfUi.Tools;

public partial class IndentTool : BaseTool, ITool
{
    public bool IsEnabled => ToolSettings.Default.IndentToolEnabled;
    public bool IsFavourite => ToolSettings.Default.IndentToolFavourite;

    public enum ToolActions
    {
        Indent2Spaces,
        Indent4Spaces,
        IndentTab
    }

    public IndentTool(TextBox noteTextBox) : base(noteTextBox)
    {
        _name = "Indent";
        _menuActions.Add(new("2 Spaces", MenuActionCommand, ToolActions.Indent2Spaces));
        _menuActions.Add(new("4 Spaces", MenuActionCommand, ToolActions.Indent4Spaces));
        _menuActions.Add(new("Tab", MenuActionCommand, ToolActions.IndentTab));
    }

    [RelayCommand]
    private void MenuAction(ToolActions action)
    {
        ApplyFunctionToEachLine(ModifyLineCallback, action);
    }

    private string? ModifyLineCallback(string line, int index, Enum action)
    {
        switch (action)
        {
            case ToolActions.Indent2Spaces:
                return $"  {line}";
            case ToolActions.Indent4Spaces:
                return $"    {line}";
            case ToolActions.IndentTab:
                return $"\t{line}";
        }

        return line;
    }
}
