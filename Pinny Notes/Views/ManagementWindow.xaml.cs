using System.Collections.Generic;
using System.Windows;

using PinnyNotes.WpfUi.Models;

namespace PinnyNotes.WpfUi.Views;

public partial class ManagementWindow : Window
{
    public ManagementWindow()
    {
        InitializeComponent();
    }

    public void DisplayNotes(IEnumerable<NoteModel> notes)
    {
        NotesListView.ItemsSource = notes;
    }
}
