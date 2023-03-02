using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace Pinny_Notes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_MouseDown(object sender, RoutedEventArgs e)
        {
            DragMove();
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
        }

        private void TopButton_Click(object sender, RoutedEventArgs e)
        {
            Topmost = !Topmost;
            if (Topmost)
            {
                TopButton.Content = "-";
            }
            else
            {
                TopButton.Content = "^";
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveNote();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (NoteTextBox.Text != "")
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Do you want to save this note?", "Pinny Notes", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if ((messageBoxResult == MessageBoxResult.Yes && SaveNote() == MessageBoxResult.Cancel) || messageBoxResult == MessageBoxResult.Cancel)
                {
                    return;
                }
            }
            Close();
        }

        private void NoteTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (NoteTextBox.SelectionLength > 0)
            {
                Clipboard.SetText(NoteTextBox.SelectedText.Trim());
            }
        }

        private MessageBoxResult SaveNote()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Documents (*.txt)|*.txt|All Files|*";
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, NoteTextBox.Text);
                return MessageBoxResult.OK;
            }
            return MessageBoxResult.Cancel;
        }
    }
}
