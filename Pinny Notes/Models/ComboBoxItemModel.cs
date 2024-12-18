namespace PinnyNotes.WpfUi.Models;

public class ComboBoxItemModel<TValue>(TValue value, string text)
{
    public TValue Value { get; set; } = value;
    public string Text { get; set; } = text;
}
