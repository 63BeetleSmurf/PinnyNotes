using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.CodeDom.Compiler;

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

            AutoCopyMenuItem.IsChecked = Properties.Settings.Default.AutoCopy;
        }

        public MainWindow(double left, double top)
        {
            InitializeComponent();

            this.Left = left;
            this.Top = top;
        }

        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
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
            if (Properties.Settings.Default.AutoCopy && NoteTextBox.SelectionLength > 0)
            {
                Clipboard.SetText(NoteTextBox.SelectedText.Trim());
            }
        }

        #region Sort
        private void SortAscMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SortNoteText();
        }

        private void SortDecMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SortNoteText(true);
        }
        #endregion

        #region Indent
        private void Indent2SpacesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            IndentNoteText("  ");
        }
        private void Indent4SpacesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            IndentNoteText("    ");
        }

        private void IndentTabMenuItem_Click(object sender, RoutedEventArgs e)
        {
            IndentNoteText("\t");
        }

        private void IndentNoteText(string indentString)
        {
            string[] lines = NoteTextBox.Text.Split(Environment.NewLine);
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = indentString + lines[i];
            }
            NoteTextBox.Text = string.Join(Environment.NewLine, lines);
        }
        #endregion

        #region Trim
        private void TrimStartMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string[] lines = NoteTextBox.Text.Split(Environment.NewLine);
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].TrimStart(); 
            }
            NoteTextBox.Text = string.Join(Environment.NewLine, lines);
        }
        private void TrimEndMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string[] lines = NoteTextBox.Text.Split(Environment.NewLine);
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].TrimEnd();
            }
            NoteTextBox.Text = string.Join(Environment.NewLine, lines);
        }
        private void TrimBothMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string[] lines = NoteTextBox.Text.Split(Environment.NewLine);
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].Trim();
            }
            NoteTextBox.Text = string.Join(Environment.NewLine, lines);
        }
        #endregion

        #region Base64
        private void Base64EncodeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(NoteTextBox.Text);
            NoteTextBox.Text = System.Convert.ToBase64String(textBytes);
        }

        private void Base64DecodeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            byte[] base64Bytes = System.Convert.FromBase64String(NoteTextBox.Text);
            NoteTextBox.Text = System.Text.Encoding.UTF8.GetString(base64Bytes);
        }
        #endregion

        private void AutoCopyMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.AutoCopy = AutoCopyMenuItem.IsChecked;
            Properties.Settings.Default.Save();
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

        private void SortNoteText(bool reverse = false)
        {
            string[] lines = NoteTextBox.Text.Split(Environment.NewLine);
            Array.Sort(lines);
            if (reverse)
            {
                Array.Reverse(lines);
            }
            NoteTextBox.Text = string.Join(Environment.NewLine, lines);
        }
    }
}
