using PresentationLayer.ViewModels;
using System.Windows;

namespace PresentationLayer.Views;
public partial class MainWindow : Window
{
    private readonly MainViewModel _viewModel;

    public MainWindow(MainViewModel viewModel)
    {
        _viewModel = viewModel;
        DataContext = _viewModel;
        InitializeComponent();
    }
}
