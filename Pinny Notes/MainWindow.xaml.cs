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

        public MainWindow(double left, double top)
        {
            InitializeComponent();

            this.Left = left;
            this.Top = top;
        }

        private void MainWindow_MouseDown(object sender, RoutedEventArgs e)
        {
            DragMove();
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            int gravityLeft;
            int gravityTop;
            if (this.Left > SystemParameters.PrimaryScreenWidth / 2)
            {
                gravityLeft = -1;
            }
            else
            {
                gravityLeft = 1;
            }
            if (this.Top > SystemParameters.PrimaryScreenHeight / 2)
            {
                gravityTop = -1;
            }
            else
            {
                gravityTop = 5; // Leave extra room to keep title bar visible
            }
            new MainWindow(this.Left + (10 * gravityLeft), this.Top + (10 * gravityTop)).Show();
        }

        private void TopButton_Click(object sender, RoutedEventArgs e)
        {
            Topmost = !Topmost;
            if (Topmost)
            {
                PinImage.Visibility = Visibility.Visible;
                Pin45Image.Visibility = Visibility.Hidden;
            }
            else
            {
                PinImage.Visibility = Visibility.Hidden;
                Pin45Image.Visibility = Visibility.Visible;
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
