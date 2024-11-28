using System.Windows;
using WBStool11.Models;
using WBStool11.ViewModels;

namespace WBStool11;

public partial class MainWindow : Window
{
    private ProjectViewModel ProjectViewModel => DataContext as ProjectViewModel;

    public MainWindow(ProjectViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        => ProjectViewModel.SelectedElement = e.NewValue as Element;
}
