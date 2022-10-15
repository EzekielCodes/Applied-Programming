using Globals.Interfaces;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Numerics;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Wpf3dTools.Interfaces;

namespace PresentationLayer.ViewModels;
public class MainViewModel : ObservableObject
{
    private readonly IWorld _world;
    private readonly ISphericalCameraController _cameraController;
    public ProjectionCamera Camera => _cameraController.Camera;

    private string _title = "WpfApp (MVVM)";

    // binding properties

    public string Title
    {
        get => _title;
        private set => SetProperty<string>(ref _title, value);
    }

    private readonly Model3DGroup _model3dGroup = new();
    public Model3D Visual3dContent => _model3dGroup;

    public string Time => DateTime.Now.ToLongTimeString();

    public IRelayCommand<MouseWheelEventArgs> ZoomCommand { get; private set; }
    public IRelayCommand<Vector> ControlByMouseCommand { get; private set; }

    public IRelayCommand UpdateCommand { get; }

    public MainViewModel(IWorld logic, ISphericalCameraController cameraController)
    {
        _world = logic;
        _cameraController = cameraController;
        ZoomCommand = new RelayCommand<MouseWheelEventArgs>(ZoomByMouse);
        ControlByMouseCommand = new RelayCommand<Vector>(ControlByMouse);

        Init3DPresentation();
        UpdateCommand = new RelayCommand(NotifyTimeChanged);
    }

    private void NotifyTimeChanged()
    {
        OnPropertyChanged(nameof(Time));
    }
    private void ZoomByMouse(MouseWheelEventArgs? args)
    {
        _cameraController.Zoom(args!.Delta);
    }

    private void Init3DPresentation()
    {
        SetupCamera();
        SetUpLights();
    }

    private void SetupCamera()
    {
        double l1 = (_world.Bounds.p1 - _world.Origin).Length;
        double l2 = (_world.Bounds.p2 - _world.Origin).Length;
        double radius = 2.3 * Math.Max(l1, l2);
        _cameraController.PositionCamera(radius, Math.PI / 10, 2.0 * Math.PI / 5);
    }

    private void SetUpLights()
    {
        _model3dGroup.Children.Add(new AmbientLight(Colors.Gray));
        var direction = new Vector3D(1.5, -3, -5);
        _model3dGroup.Children.Add(new DirectionalLight(Colors.Gray, direction));
    }

    private void ControlByMouse(Vector vector)
    {
        _cameraController.ControlByMouse(vector);
    }


}
