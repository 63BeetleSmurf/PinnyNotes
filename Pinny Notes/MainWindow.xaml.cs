using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Newtonsoft.Json;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Threading.Tasks;
using Octokit;
using System.Reflection;
using System.Windows.Media;
using System.Linq;
using System.Windows.Controls;

namespace Pinny_Notes
{
    public partial class MainWindow : Window
    {
        Dictionary<string,(string, string, string)> NOTE_COLOURS = new Dictionary<string, (string, string, string)>{
            {"Yellow", ("#fef7b1", "#fffcdd", "#feea00")},
            {"Orange", ("#ffd179", "#fee8b9", "#ffab00")},
            {"Red",    ("#ff7c81", "#ffc4c6", "#e33036")},
            {"Pink",   ("#d986cc", "#ebbfe3", "#a72995")},
            {"Purple", ("#9d9add", "#d0cef3", "#625bb8")},
            {"Blue",   ("#7ac3e6", "#b3d9ec", "#1195dd")},
            {"Aqua",   ("#97cfc6", "#c0e2e1", "#16b098")},
            {"Green",  ("#c6d67d", "#e3ebc6", "#aacc04")}
        };

        string? NOTE_COLOUR = null;
        Tuple<bool, bool>? NOTE_GRAVITY = null;

        public MainWindow()
        {
            InitializeComponent();
            LoadSettings();
            PositionNote();
#pragma warning disable CS4014
            CheckForNewVersion();
#pragma warning restore CS4014
        }

        public MainWindow(double parentLeft, double parentTop, string? parentColour, Tuple<bool, bool>? parentGravity)
        {
            InitializeComponent();
            LoadSettings(parentColour, parentGravity);
            PositionNote(parentLeft, parentTop);
        }

        private void MainWindow_LocationChanged(object sender, EventArgs e)
        {
            if (!this.IsLoaded)
                return;

            bool gravityLeft = true;
            bool gravityTop = true;
            if (Left > SystemParameters.PrimaryScreenWidth / 2)
                gravityLeft = false;
            if (Top > SystemParameters.PrimaryScreenHeight / 2)
                gravityTop = false;
            NOTE_GRAVITY = new Tuple<bool, bool>(
                gravityLeft,
                gravityTop
            );
        }

        private void LoadSettings(string? parentColour = null, Tuple<bool, bool>? parentGravity = null)
        {
            AutoCopyMenuItem.IsChecked = Properties.Settings.Default.AutoCopy;
            SpellCheckMenuItem.IsChecked = Properties.Settings.Default.SpellCheck;
            NoteTextBox.SpellCheck.IsEnabled = SpellCheckMenuItem.IsChecked;
            NewLineMenuItem.IsChecked = Properties.Settings.Default.NewLine;
            DisableUpdateCheckMenuItem.IsChecked = Properties.Settings.Default.DisableUpdateCheck;
            ColourCycleMenuItem.IsChecked = Properties.Settings.Default.CycleColours;
            SetColour(parentColour: parentColour);
            if (parentGravity == null)
                NOTE_GRAVITY = new Tuple<bool, bool>(
                    Properties.Settings.Default.StartupPositionLeft,
                    Properties.Settings.Default.StartupPositionTop
                );
            else
                NOTE_GRAVITY = parentGravity;

            if (Properties.Settings.Default.StartupPositionLeft)
            {
                if (Properties.Settings.Default.StartupPositionTop)
                    StartupPositionTopLeftMenuItem.IsChecked = true;
                else
                    StartupPositionBottomLeftMenuItem.IsChecked = true;
            }
            else
            {
                if (Properties.Settings.Default.StartupPositionTop)
                    StartupPositionTopRightMenuItem.IsChecked = true;
                else
                    StartupPositionBottomRightMenuItem.IsChecked = true;
            }

        }

        private void SetColour(string? colour = null, string? parentColour = null)
        {
            if (colour == null)
            {
                if (Properties.Settings.Default.CycleColours)
                {
                    string? nextColour = null;
                    int nextColourIndex = NOTE_COLOURS.Keys.ToList().IndexOf(Properties.Settings.Default.Colour) + 1;
                    while (nextColour == null)
                    {
                        nextColour = NOTE_COLOURS.Keys.ElementAtOrDefault(nextColourIndex);
                        if (nextColour == null)
                        {
                            nextColourIndex = 0;
                        }
                        else if (nextColour == parentColour)
                        {
                            nextColour = null;
                            nextColourIndex++;
                        }
                    }
                    colour = nextColour;
                }
                else
                {
                    colour = Properties.Settings.Default.Colour;
                }
            }

            BrushConverter brushConverter = new BrushConverter();
            object? titleBrush = brushConverter.ConvertFromString(NOTE_COLOURS[colour].Item1);
            object? bodyBrush = brushConverter.ConvertFromString(NOTE_COLOURS[colour].Item2);
            object? borderBrush = brushConverter.ConvertFromString(NOTE_COLOURS[colour].Item3);

#pragma warning disable CS8600
            TitleBarGrid.Background = (Brush)titleBrush;
            Background = (Brush)bodyBrush;
            BorderBrush = (Brush)borderBrush;
#pragma warning restore CS8600

            NOTE_COLOUR = colour;
            Properties.Settings.Default.Colour = colour;
            Properties.Settings.Default.Save();

            foreach (object childObject in ColoursMenuItem.Items)
            {
                if (childObject.GetType().Name != "MenuItem")
                    continue;

                MenuItem childMenuItem = (MenuItem)childObject;
                if (childMenuItem.Header.ToString() != "Cycle" && childMenuItem.Header.ToString() != colour)
                    childMenuItem.IsChecked = false;
                else if (childMenuItem.Header.ToString() == colour)
                    childMenuItem.IsChecked = true;
            }
        }

        private void PositionNote(double? parentLeft = null, double? parentTop = null)
        {
            double positionTop = 0;
            double positionLeft = 0;
#pragma warning disable CS8602
            if (parentLeft == null || parentTop == null)
            {
                int screenMargin = 78;
                if (NOTE_GRAVITY.Item1)
                    positionLeft = screenMargin + 10;
                else
                    positionLeft = (SystemParameters.PrimaryScreenWidth - screenMargin) - Width - 10;

                if (NOTE_GRAVITY.Item2)
                    positionTop = screenMargin + 50;
                else
                    positionTop = (SystemParameters.PrimaryScreenHeight - screenMargin) - Height - 50;
            }
            else
            {
                if (NOTE_GRAVITY.Item1)
                    positionLeft = (double)parentLeft + 10;
                else
                    positionLeft = (double)parentLeft - 10;

                if (NOTE_GRAVITY.Item2)
                    positionTop = (double)parentTop + 50;
                else
                    positionTop = (double)parentTop - 50;
            }
#pragma warning restore CS8602

            if (positionLeft < 0)
                Left = 0;
            else if (positionLeft + Width > SystemParameters.PrimaryScreenWidth)
                Left = SystemParameters.PrimaryScreenWidth - Width;
            else
                Left = positionLeft;

            if (positionTop < 0)
                Top = 0;
            else if (positionTop + Height > SystemParameters.PrimaryScreenHeight)
                Top = SystemParameters.PrimaryScreenHeight - Height;
            else
                Top = positionTop;
        }

        private async Task CheckForNewVersion()
        {
            DateTime lastUpdateCheck = Properties.Settings.Default.LastUpdateCheck;
            if (
                Properties.Settings.Default.DisableUpdateCheck 
                || DateTime.Now.Subtract(lastUpdateCheck).Days < 7
            )
                return;

            GitHubClient client = new GitHubClient(new ProductHeaderValue("pinny_notes"));
            IReadOnlyList<Release> releases = await client.Repository.Release.GetAll("63BeetleSmurf", "pinny_notes");

            Version latestVersion = new Version(releases[0].TagName.Replace("v", "") + ".0");
            Version? localVersion = Assembly.GetExecutingAssembly().GetName().Version;
            if (localVersion is null)
                return;

            if (localVersion.CompareTo(latestVersion) < 0)
                MessageBox.Show(
                    "A new version of Pinny Notes is available.\n\nGet the latest release from;\nhttps://github.com/63BeetleSmurf/pinny_notes/releases",
                    "Update Available",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

            Properties.Settings.Default.LastUpdateCheck = DateTime.Now;
            Properties.Settings.Default.Save();
        }

        private MessageBoxResult SaveNote()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Documents (*.txt)|*.txt|All Files|*";
            if (saveFileDialog.ShowDialog(this) == true)
            {
                File.WriteAllText(saveFileDialog.FileName, NoteTextBox.Text);
                return MessageBoxResult.OK;
            }
            return MessageBoxResult.Cancel;
        }

        #region TitleBar
        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow(
                Left,
                Top,
                NOTE_COLOUR,
                NOTE_GRAVITY
            ).Show();
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

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (NoteTextBox.Text != "")
            {
                MessageBoxResult messageBoxResult = MessageBox.Show(
                    this,
                    "Do you want to save this note?",
                    "Pinny Notes",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question
                );
                if (
                    (messageBoxResult == MessageBoxResult.Yes && SaveNote() == MessageBoxResult.Cancel)
                    || messageBoxResult == MessageBoxResult.Cancel
                )
                    return;
            }
            Close();
        }
        #endregion

        #region ContextMenu
        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveNote();
        }

        private void ClearMenuItem_Click(object sender, RoutedEventArgs e)
        {
            NoteTextBox.Clear();
        }

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
                lines[i] = indentString + lines[i];
            NoteTextBox.Text = string.Join(Environment.NewLine, lines);
        }
        #endregion

        #region Trim
        private void TrimStartMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string[] lines = NoteTextBox.Text.Split(Environment.NewLine);
            for (int i = 0; i < lines.Length; i++)
                lines[i] = lines[i].TrimStart(); 
            NoteTextBox.Text = string.Join(Environment.NewLine, lines);
        }
        private void TrimEndMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string[] lines = NoteTextBox.Text.Split(Environment.NewLine);
            for (int i = 0; i < lines.Length; i++)
                lines[i] = lines[i].TrimEnd();
            NoteTextBox.Text = string.Join(Environment.NewLine, lines);
        }
        private void TrimBothMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string[] lines = NoteTextBox.Text.Split(Environment.NewLine);
            for (int i = 0; i < lines.Length; i++)
                lines[i] = lines[i].Trim();
            NoteTextBox.Text = string.Join(Environment.NewLine, lines);
        }
        #endregion

        #region List
        private void ListEnumerateMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string[] lines = NoteTextBox.Text.Split(Environment.NewLine);
            for (int i = 0; i < lines.Length; i++)
                lines[i] = (i + 1).ToString() + ". " + lines[i];
            NoteTextBox.Text = string.Join(Environment.NewLine, lines);
        }

        private void ListSortAscMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SortNoteText();
        }

        private void ListSortDecMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SortNoteText(true);
        }

        private void SortNoteText(bool reverse = false)
        {
            string[] lines = NoteTextBox.Text.Split(Environment.NewLine);
            Array.Sort(lines);
            if (reverse)
                Array.Reverse(lines);
            NoteTextBox.Text = string.Join(Environment.NewLine, lines);
        }
        #endregion

        #region JSON
        private void JSONPrettifyMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string noteText = NoteTextBox.Text;
            try
            {
                NoteTextBox.Text = JsonConvert.SerializeObject(
                    JsonConvert.DeserializeObject(noteText),
                    Formatting.Indented
                );
            }
            catch
            {
                NoteTextBox.Text = noteText;
            }
        }
        #endregion

        #region Hash
        private void HashSHA512MenuItem_Click(object sender, RoutedEventArgs e)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                NoteTextBox.Text = BitConverter.ToString(
                    sha512.ComputeHash(Encoding.UTF8.GetBytes(NoteTextBox.Text))
                ).Replace("-", "");
            }
        }

        private void HashSHA384MenuItem_Click(object sender, RoutedEventArgs e)
        {
            using (SHA384 sha384 = SHA384.Create())
            {
                NoteTextBox.Text = BitConverter.ToString(
                    sha384.ComputeHash(Encoding.UTF8.GetBytes(NoteTextBox.Text))
                ).Replace("-", "");
            }
        }

        private void HashSHA256MenuItem_Click(object sender, RoutedEventArgs e)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                NoteTextBox.Text = BitConverter.ToString(
                    sha256.ComputeHash(Encoding.UTF8.GetBytes(NoteTextBox.Text))
                ).Replace("-", "");
            }
        }

        private void HashSHA1MenuItem_Click(object sender, RoutedEventArgs e)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                NoteTextBox.Text = BitConverter.ToString(
                    sha1.ComputeHash(Encoding.UTF8.GetBytes(NoteTextBox.Text))
                ).Replace("-", "");
            }
        }

        private void HashMD5MenuItem_Click(object sender, RoutedEventArgs e)
        {
            using (MD5 md5 = MD5.Create())
            {
                NoteTextBox.Text = BitConverter.ToString(
                    md5.ComputeHash(Encoding.UTF8.GetBytes(NoteTextBox.Text))
                ).Replace("-", "");
            }
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

        #region Settings

        #region Colours
        private void ColourCycleMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.CycleColours = ColourCycleMenuItem.IsChecked;
            Properties.Settings.Default.Save();
        }

        private void ColourMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            if (!menuItem.IsChecked)
            {
                menuItem.IsChecked = true;
                return;
            }

            SetColour(menuItem.Header.ToString());
        }
        #endregion

        private void StartupPositionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            if (!menuItem.IsChecked)
            {
                menuItem.IsChecked = true;
                return;
            }

            foreach (object childObject in StartupPositionMenuItem.Items)
            {
                MenuItem childMenuItem = (MenuItem)childObject;
                if (childMenuItem != menuItem)
                    childMenuItem.IsChecked = false;
            }

#pragma warning disable CS8602
            string[] position = menuItem.Header.ToString().Split(" ");
#pragma warning restore CS8602
            if (position[0] == "Top")
                Properties.Settings.Default.StartupPositionTop = true;
            else
                Properties.Settings.Default.StartupPositionTop = false;
            if (position[1] == "Left")
                Properties.Settings.Default.StartupPositionLeft = true;
            else
                Properties.Settings.Default.StartupPositionLeft = false;
            Properties.Settings.Default.Save();
        }

        private void AutoCopyMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.AutoCopy = AutoCopyMenuItem.IsChecked;
            Properties.Settings.Default.Save();
        }

        private void SpellCheckMenuItem_Click(object sender, RoutedEventArgs e)
        {
            NoteTextBox.SpellCheck.IsEnabled = SpellCheckMenuItem.IsChecked;
            Properties.Settings.Default.SpellCheck = SpellCheckMenuItem.IsChecked;
            Properties.Settings.Default.Save();
        }

        private void NewLineMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.NewLine = NewLineMenuItem.IsChecked;
            Properties.Settings.Default.Save();

            if (Properties.Settings.Default.NewLine)
                NoteTextBox_TextChanged(sender, e);
        }

        private void DisableUpdateCheckMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.DisableUpdateCheck = DisableUpdateCheckMenuItem.IsChecked;
            Properties.Settings.Default.Save();
        }
        #endregion

        #endregion

        #region TextBox
        private void NoteTextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.NewLine && !NoteTextBox.Text.EndsWith(Environment.NewLine))
            {
                // Preserving selection when adding new line
                int selectionStart = NoteTextBox.SelectionStart;
                int selectionLength = NoteTextBox.SelectionLength;

                NoteTextBox.Text += Environment.NewLine;
                
                NoteTextBox.SelectionStart = selectionStart;
                NoteTextBox.SelectionLength = selectionLength;
            }
        }

        private void NoteTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.AutoCopy && NoteTextBox.SelectionLength > 0)
                Clipboard.SetText(NoteTextBox.SelectedText.Trim());
        }

        private void NoteTextBox_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
            e.Handled = true;
        }

        private void NoteTextBox_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                NoteTextBox.Text = File.ReadAllText(
                    ((string[])e.Data.GetData(DataFormats.FileDrop))[0]
                );
            }
            else if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                NoteTextBox.Text = (string)e.Data.GetData(DataFormats.StringFormat);
            }
        }
        #endregion
    }
}
