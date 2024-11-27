using System.Windows;
using WBStool11.ViewModels;

namespace WBStool11;

public partial class MainWindow : Window
{
    public MainWindow(ProjectViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
