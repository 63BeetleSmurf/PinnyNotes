using PinnyNotes.WpfUi.ViewModels;
using PinnyNotes.WpfUi.Views;

namespace PinnyNotes.WpfUi.Services;

public static class DialogueService
{
    public static DateTime? ShowReminderDialogue(NoteWindow owner, DateTime? reminderTrigger = null)
    {
        ReminderDialogueViewModel viewModel = new(reminderTrigger);

        ReminderDialogue dialogue = new()
        {
            DataContext = viewModel,
            Owner = owner
        };

        viewModel.CloseAction = (result) => { dialogue.DialogResult = result; };

        bool? result = dialogue.ShowDialog();

        return (result == true) ? viewModel.ReminderTrigger : null;
    }
}
