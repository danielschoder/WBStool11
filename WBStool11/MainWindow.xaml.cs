using System.Windows;

namespace WBStool11;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    private void NewProject_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("New Project clicked!");
    }

    private void OpenProject_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Open Project clicked!");
    }

    private void SaveProject_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Save Project clicked!");
    }

    private void SaveAs_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Save As clicked!");
    }

    private void About_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("About clicked!");
    }
}
