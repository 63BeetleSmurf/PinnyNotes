using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Animation;

using Pinny_Notes.Enums;
using Pinny_Notes.Helpers;
using Pinny_Notes.Properties;
using Pinny_Notes.Tools;
using Pinny_Notes.ViewModels;

namespace Pinny_Notes.Views;

public partial class NoteWindow : Window
{
    private NoteViewModel _viewModel { get; }

    private RelayCommand _copyCommand = null!;
    private RelayCommand _cutCommand = null!;
    private RelayCommand _pasteCommand = null!;

    private RelayCommand _clearCommand = null!;
    private RelayCommand _saveCommand = null!;

    private RelayCommand _resetSizeCommand = null!;

    private IEnumerable<ITool> _tools = [];

    #region NoteWindow

    public NoteWindow() : this(null) { }
    public NoteWindow(NoteViewModel? parentViewModel = null)
    {
        DataContext = _viewModel = new NoteViewModel(parentViewModel);

        InitializeComponent();

        _tools = [
            new Base64Tool(NoteTextBox),
            new BracketTool(NoteTextBox),
            new CaseTool(NoteTextBox),
            new DateTimeTool(NoteTextBox),
            new GibberishTool(NoteTextBox),
            new HashTool(NoteTextBox),
            new HtmlEntityTool(NoteTextBox),
            new IndentTool(NoteTextBox),
            new JoinTool(NoteTextBox),
            new JsonTool(NoteTextBox),
            new ListTool(NoteTextBox),
            new QuoteTool(NoteTextBox),
            new RemoveTool(NoteTextBox),
            new SlashTool(NoteTextBox),
            new SortTool(NoteTextBox),
            new SplitTool(NoteTextBox),
            new TrimTool(NoteTextBox)
        ];

        NoteTextBox.ContextMenu = new();

        _copyCommand = new(NoteTextBox.Copy);
        NoteTextBox.InputBindings.Add(new InputBinding(_copyCommand, new KeyGesture(Key.C, ModifierKeys.Control)));
        _cutCommand = new(NoteTextBox.Cut);
        NoteTextBox.InputBindings.Add(new InputBinding(_cutCommand, new KeyGesture(Key.X, ModifierKeys.Control)));
        _pasteCommand = new(NoteTextBox.Paste);
        NoteTextBox.InputBindings.Add(new InputBinding(_pasteCommand, new KeyGesture(Key.V, ModifierKeys.Control)));

        _clearCommand = new(NoteTextBox.Clear);
        ClearMenuItem.Command = _clearCommand;
        _saveCommand = new(SaveCommandExecute);
        SaveMenuItem.Command = _saveCommand;
        _resetSizeCommand = new(ResetSizeCommandExecute);
        ResetSizeMenuItem.Command = _resetSizeCommand;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        _viewModel.WindowHandel = ScreenHelper.GetWindowHandle(this);
    }

    private void NoteWindow_MouseDown(object sender, MouseButtonEventArgs e)
    {
        // Check mouse button is pressed as a missed click of a button
        // can cause issues with DragMove().
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            DragMove();

            // Reset gravity depending what position the note was moved to.
            // This does not effect the saved start up setting, only what
            // direction new child notes will go towards.
            _viewModel.X = Left;
            _viewModel.Y = Top;

            Rectangle screenBounds = ScreenHelper.GetCurrentScreenBounds(_viewModel.WindowHandel);
            _viewModel.GravityX = (Left - screenBounds.X < screenBounds.Width / 2) ? 1 : -1;
            _viewModel.GravityY = (Top - screenBounds.Y < screenBounds.Height / 2) ? 1 : -1;
        }
    }

    private void NoteWindow_StateChanged(object sender, EventArgs e)
    {
        MinimizeModes minimizeMode = (MinimizeModes)Settings.Default.MinimizeMode;

        if (WindowState == WindowState.Minimized
            && (
                minimizeMode == MinimizeModes.Prevent 
                || (minimizeMode == MinimizeModes.PreventIfPinned && Topmost)
            )
        )
            WindowState = WindowState.Normal;
    }

    private void Window_MouseEnter(object sender, MouseEventArgs e)
    {
        ShowTitleBar();
    }

    private void Window_MouseLeave(object sender, MouseEventArgs e)
    {
        if (!IsActive)
            HideTitleBar();
    }

    private void Window_Activated(object sender, EventArgs e)
    {
        Topmost = true;
        _viewModel.IsFocused = true;
        _viewModel.UpdateOpacity();
        ShowTitleBar();
    }

    private void Window_Deactivated(object sender, EventArgs e)
    {
        Topmost = _viewModel.IsPinned;
        _viewModel.IsFocused = false;
        _viewModel.UpdateOpacity();
        HideTitleBar();
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
        if (!_viewModel.IsSaved && NoteTextBox.Text != "")
        {
            MessageBoxResult messageBoxResult = MessageBox.Show(
                this,
                "Do you want to save this note?",
                "Pinny Notes",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question
            );
            // If the user presses cancel on the message box or 
            // save dialog, do not close.
            if (
                (messageBoxResult == MessageBoxResult.Yes && SaveNote() == MessageBoxResult.Cancel)
                || messageBoxResult == MessageBoxResult.Cancel
            )
                e.Cancel = true;
        }
    }

    #endregion

    #region Commands

    public void SaveCommandExecute()
    {
        SaveNote();
    }

    public void ResetSizeCommandExecute()
    {
        Width = NoteViewModel.DefaultWidth;
        Height = NoteViewModel.DefaultHeight;
    }

    #endregion

    #region MiscFunctions

    private MessageBoxResult SaveNote()
    {
        SaveFileDialog saveFileDialog = new()
        {
            Filter = "Text Documents (*.txt)|*.txt|All Files|*"
        };
        if (saveFileDialog.ShowDialog(this) == true)
        {
            File.WriteAllText(saveFileDialog.FileName, NoteTextBox.Text);
            _viewModel.IsSaved = true;
            return MessageBoxResult.OK;
        }
        return MessageBoxResult.Cancel;
    }

    private void HideTitleBar()
    {
        if (Settings.Default.HideTitleBar)
            BeginStoryboard("HideTitleBarAnimation");
    }

    private void ShowTitleBar()
    {
        BeginStoryboard("ShowTitleBarAnimation");
    }

    private void BeginStoryboard(string resourceKey)
    {
        Storyboard hideTitleBar = (Storyboard)FindResource(resourceKey);
        hideTitleBar.Begin();
    }

    #endregion

    #region TitleBar

    private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount >= 2)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }
    }

    private void NewButton_Click(object sender, RoutedEventArgs e)
    {
        new NoteWindow(_viewModel).Show();
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void SettingsMenuItem_Click(object sender, RoutedEventArgs e)
    {
        ((App)Application.Current).ShowSettingsWindow(this);
    }

    #endregion

    #region TextBox

    private void NoteTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        _viewModel.IsSaved = false;
    }

    #region ContextMenu

    private ContextMenu GetNoteTextBoxContextMenu()
    {
        ContextMenu menu = new();

        int caretIndex = NoteTextBox.CaretIndex;
        SpellingError spellingError = NoteTextBox.GetSpellingError(caretIndex);
        if (spellingError != null)
        {
            if (!spellingError.Suggestions.Any())
                menu.Items.Add(
                        new MenuItem()
                        {
                            Header = "(no spelling suggestions)",
                            IsEnabled = false
                        }
                    );
            else
                foreach (string spellingSuggestion in spellingError.Suggestions)
                {
                    menu.Items.Add(
                        new MenuItem()
                        {
                            Header = spellingSuggestion,
                            FontWeight = FontWeights.Bold,
                            Command = EditingCommands.CorrectSpellingError,
                            CommandParameter = spellingSuggestion,
                            CommandTarget = NoteTextBox
                        }
                    );
                }
            menu.Items.Add(new Separator());
        }

        menu.Items.Add(
            new MenuItem()
            {
                Header = "Copy",
                Command = _copyCommand,
                InputGestureText = "Ctrl+C",
                IsEnabled = (NoteTextBox.SelectionLength > 0)
            }
        );
        menu.Items.Add(
            new MenuItem()
            {
                Header = "Cut",
                Command = _cutCommand,
                InputGestureText = "Ctrl+X",
                IsEnabled = (NoteTextBox.SelectionLength > 0)
            }
        );
        menu.Items.Add(
            new MenuItem()
            {
                Header = "Paste",
                Command = _pasteCommand,
                InputGestureText = "Ctrl+V",
                IsEnabled = Clipboard.ContainsText()
            }
        );
            
        menu.Items.Add(new Separator());

        menu.Items.Add(
            new MenuItem()
            {
                Header = "Select All",
                Command = new RelayCommand(NoteTextBox.SelectAll),
                IsEnabled = (NoteTextBox.Text.Length > 0)
            }
        );
        menu.Items.Add(
            new MenuItem()
            {
                Header = "Clear",
                Command = _clearCommand,
                IsEnabled = (NoteTextBox.Text.Length > 0)
            }
        );
        menu.Items.Add(
            new MenuItem()
            {
                Header = "Save",
                Command = _saveCommand,
                IsEnabled = (NoteTextBox.Text.Length > 0)
            }
        );

        menu.Items.Add(new Separator());

        MenuItem countsMenuItem = new()
        {
            Header = "Counts"
        };
        countsMenuItem.Items.Add(
            new MenuItem()
            {
                Header = $"Lines: {NoteTextBox.LineCount()}",
                IsEnabled = false
            }
        );
        countsMenuItem.Items.Add(
            new MenuItem()
            {
                Header = $"Words: {NoteTextBox.WordCount()}",
                IsEnabled = false
            }
        );
        countsMenuItem.Items.Add(
            new MenuItem()
            {
                Header = $"Chars: {NoteTextBox.CharCount()}",
                IsEnabled = false
            }
        );
        menu.Items.Add(countsMenuItem);

        AddToolContextMenus(menu.Items);

        return menu;
    }

    private void AddToolContextMenus(ItemCollection menuItems)
    {
        IEnumerable<ITool> favouriteTools = _tools.Where(t => t.IsEnabled && t.IsFavourite);
        bool hasFavouriteTools = favouriteTools.Any();
        IEnumerable<ITool> standardTools = _tools.Where(t => t.IsEnabled && !t.IsFavourite);
        bool hasStandardTools = standardTools.Any();

        if (hasFavouriteTools || hasStandardTools)
            menuItems.Add(new Separator());

        foreach (ITool tool in favouriteTools)
        {
            if (tool.IsEnabled)
                menuItems.Add(
                    tool.GetMenuItem()
                );
        }

        if (hasStandardTools)
        {
            MenuItem toolsMenu = new()
            {
                Header = "Tools"
            };
            foreach (ITool tool in standardTools)
            {
                toolsMenu.Items.Add(
                    tool.GetMenuItem()
                );
            }
            menuItems.Add(toolsMenu);
        }
    }

    private void NoteTextBox_ContextMenuOpening(object sender, ContextMenuEventArgs e)
    {
        NoteTextBox.ContextMenu = GetNoteTextBoxContextMenu();
    }

    #endregion

    #endregion
}
