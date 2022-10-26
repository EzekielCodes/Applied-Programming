using PresentationLayer.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace PresentationLayer.Views;
public partial class MainWindow : Window
{
    private readonly MainViewModel _viewModel;
    private Point _lastPoint;


    public MainWindow(MainViewModel viewModel)
    {
        _viewModel = viewModel;
        DataContext = _viewModel;
        InitializeComponent();
    }

    private void ViewPortMouseDown(object sender, MouseButtonEventArgs e)
    {
        _lastPoint = e.GetPosition(mainViewPort);
        _ = viewPortControl.CaptureMouse();
        PreviewKeyDown += WindowKeyDown;
        viewPortControl.MouseUp += ViewPortMouseUp;
        viewPortControl.PreviewMouseMove += ViewPortMouseMove;
        viewPortControl.PreviewMouseWheel += ViewPortPreviewMouseWheel;
    }

    private void ViewPortMouseMove(object sender, MouseEventArgs e)
    {
        var newPoint = e.GetPosition(mainViewPort);
        var vector = newPoint - _lastPoint;
        _viewModel.ControlByMouseCommand.Execute(vector);
        _lastPoint = newPoint;
    }

    private void WindowKeyDown(object sender, KeyEventArgs e)
    {
        _viewModel.ProcessKey(e.Key);
    }

    private void ViewPortPreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        _viewModel.Zoom(e.Delta);
    }

    private void ViewPortMouseUp(object sender, MouseButtonEventArgs e)
    {
        viewPortControl.ReleaseMouseCapture();
        viewPortControl.PreviewMouseMove -= ViewPortMouseMove;
        viewPortControl.MouseUp -= ViewPortMouseUp;
    }

    private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {


    }

    private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {

    }
}
