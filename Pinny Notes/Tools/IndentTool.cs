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
        _menuActions.Add(new("2 Spaces", Indent2SpacesMenuAction));
        _menuActions.Add(new("4 Spaces", Indent4SpacesMenuAction));
        _menuActions.Add(new("Tab", IndentTabMenuAction));
    }

    private void Indent2SpacesMenuAction(object sender, EventArgs e) => ApplyFunctionToEachLine(ModifyLineCallback, ToolActions.Indent2Spaces);
    private void Indent4SpacesMenuAction(object sender, EventArgs e) => ApplyFunctionToEachLine(ModifyLineCallback, ToolActions.Indent4Spaces);
    private void IndentTabMenuAction(object sender, EventArgs e) => ApplyFunctionToEachLine(ModifyLineCallback, ToolActions.IndentTab);

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
