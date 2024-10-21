using System.Windows;
using Pinny_Notes.ViewModels;

namespace Pinny_Notes.Views;

public partial class SettingsWindow : Window
{
    public SettingsWindow()
    {
        DataContext = new SettingsViewModel();
        InitializeComponent();
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
