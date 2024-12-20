using System.Collections.Generic;

namespace PinnyNotes.WpfUi.Models;

public class ManagementModel
{
    public IEnumerable<NoteModel> Notes { get; set; } = [];
}
