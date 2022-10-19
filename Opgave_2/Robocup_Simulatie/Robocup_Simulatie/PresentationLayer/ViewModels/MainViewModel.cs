using Globals.Entities;
using Globals.Interfaces;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Wpf3dTools.Interfaces;

namespace PresentationLayer.ViewModels;
public class MainViewModel : ObservableObject
{
    private readonly IWorld _world;
    private readonly ISphericalCameraController _cameraController;
    private readonly IShapesFactory _shapesFactory;
    public ProjectionCamera Camera => _cameraController.Camera;
    private readonly PeriodicTimer _timer = new(TimeSpan.FromMilliseconds(10));

    private string _title = "WpfApp (MVVM)";

    // binding properties

    public string Title
    {
        get => _title;
        private set => SetProperty<string>(ref _title, value);
    }

    private readonly Model3DGroup _model3dGroup = new();
    private readonly Model3DGroup _itemsGroup = new();
    private readonly Model3DGroup _axesGroup = new();

    private readonly List<GeometryModel3D> _teamBlue = new();
    private readonly List<GeometryModel3D> _teamRed= new();

    private readonly Model3DGroup _teamBlueitems = new();
    private readonly Model3DGroup _teamReditems = new();
    public Model3DGroup Visual3dContent => _model3dGroup;
    private readonly List<GeometryModel3D> _itemsList = new();
    private bool _showAxes;

    public string Time => DateTime.Now.ToLongTimeString();

    public IRelayCommand<MouseWheelEventArgs> ZoomCommand { get; private set; }
    public IRelayCommand<Vector> ControlByMouseCommand { get; private set; }

    public IRelayCommand UpdateCommand { get; }

    public bool? ShowAxes
    {
        get => _showAxes;
        set
        {
            if (value == _showAxes) return;
            _showAxes = value ?? false;
            if (_showAxes)
            {
                _model3dGroup.Children.Add(_axesGroup);
            }
            else
            {
                _model3dGroup.Children.Remove(_axesGroup);
            }
        }
    }

    public MainViewModel(IWorld logic, ISphericalCameraController cameraController, IShapesFactory shapesFactory)
    {
        _world = logic;
        _cameraController = cameraController;
        _shapesFactory = shapesFactory;

        ZoomCommand = new RelayCommand<MouseWheelEventArgs>(ZoomByMouse);
        ControlByMouseCommand = new RelayCommand<Vector>(ControlByMouse);

        Init3DPresentation();
        
        InitItemGeometries();
        InitPresentation();
        //InitPlayers();
        _ = Animate();
        UpdateCommand = new RelayCommand(NotifyTimeChanged);
    }

    public async Task Animate()
    {
        while (true)
        {
            UpdateWorldDisplay();
            await _timer.WaitForNextTickAsync();
        }
    }

    private void UpdateWorldDisplay()
    {
        for (int i = 0; i < _itemsList.Count; i++)
        {
            var itemTransform = new Transform3DGroup();
            itemTransform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), _world.Items[i].YRotation)));
            itemTransform.Children.Add(new ScaleTransform3D(_world.Items[i].Scale, _world.Items[i].Scale, _world.Items[i].Scale));
            itemTransform.Children.Add(new TranslateTransform3D(_world.Items[i].Position - _world.Origin));
            _itemsList[i].Transform = itemTransform;
        };

        for (int i = 0; i < _teamBlue.Count; i++)
        {
            var itemTransform = new Transform3DGroup();
            itemTransform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), _world.TeamBlue[i].YRotation)));
            itemTransform.Children.Add(new ScaleTransform3D(_world.TeamBlue[i].Scale, _world.TeamBlue[i].Scale, _world.TeamBlue[i].Scale));
            itemTransform.Children.Add(new TranslateTransform3D(_world.TeamBlue[i].Position - _world.Origin));
            _teamBlue[i].Transform = itemTransform;
        };

        for (int i = 0; i < _teamRed.Count; i++)
        {
            var itemTransform = new Transform3DGroup();
            itemTransform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), _world.TeamRed[i].YRotation)));
            itemTransform.Children.Add(new ScaleTransform3D(_world.TeamRed[i].Scale, _world.TeamRed[i].Scale, _world.TeamRed[i].Scale));
            itemTransform.Children.Add(new TranslateTransform3D(_world.TeamRed[i].Position - _world.Origin));
            _teamRed[i].Transform = itemTransform;
        };
    }

    private void NotifyTimeChanged()
    {
        OnPropertyChanged(nameof(Time));
    }
    

    private void Init3DPresentation()
    {
        SetupCamera();
        SetUpLights();
        CreateAxesGroup();
        ShowAxes = true;

    }

    private void SetupCamera()
    {
        double l1 = (_world.Bounds.p1 - _world.Origin).Length;
        double l2 = (_world.Bounds.p2 - _world.Origin).Length;
        double radius = 2.3 * Math.Max(l1, l2);
        _cameraController.PositionCamera(radius, Math.PI / 10, 2.0 * Math.PI / 5);
    }

    private void ZoomByMouse(MouseWheelEventArgs? args)
    {
        _cameraController.Zoom(args!.Delta);
    }
    private void ControlByMouse(Vector vector)
    {
        _cameraController.ControlByMouse(vector);
    }

    private void SetUpLights()
    {
        _model3dGroup.Children.Add(new AmbientLight(Colors.Gray));
        var direction = new Vector3D(1.5, -3, -5);
        _model3dGroup.Children.Add(new DirectionalLight(Colors.Gray, direction));
    }

   

    private void InitItemGeometries()
    {
        foreach (var item in _world.Items)
        {
            var geometry = item switch
            {
                Sphere sphere  => _shapesFactory.CreateSphere(GetMaterial(sphere.Color)),
                // show circles without backface culling (by providing a backMaterials parameter).
                _ => throw new ArgumentException("Unknown type of a item"),
            };
            _itemsList.Add(geometry);
            _itemsGroup.Children.Add(geometry);
            _model3dGroup.Children.Add(_itemsGroup);
        }

        foreach (var item in _world.TeamBlue)
        {
            var x = item switch
            {
                Cylinder cyl => _shapesFactory.CreateCylinder(cyl.Radius, cyl.Axis, GetMaterial(cyl.Color)),
                _ => throw new ArgumentException("Unknown type of a item"),
            };
            _teamBlue.Add(x);
            _teamBlueitems.Children.Add(x);
            _model3dGroup.Children.Add(_teamBlueitems);
        }

        foreach (var item in _world.TeamRed)
        {
            var x = item switch
            {
                Cylinder cyl => _shapesFactory.CreateCylinder(cyl.Radius, cyl.Axis, GetMaterial(cyl.Color)),
                _ => throw new ArgumentException("Unknown type of a item"),
            };
            _teamRed.Add(x);
            _teamReditems.Children.Add(x);
            _model3dGroup.Children.Add(_teamReditems);
        }

        //InitPlayers();
        
       
    }

    private void InitPresentation()
    {
        //terrain aanmaken
        var terrain = _shapesFactory.CreateParallelogram
            (
                side1: new Vector3D(0, 0, 600),
                side2: new Vector3D(_world.FieldLength, 0, 0),
                materials: GetMaterial(Colors.LightGreen),
                backMaterials: GetMaterial(Colors.Green)

            );

        terrain.Transform = new TranslateTransform3D(new Point3D(- _world.FieldLength / 2, 0, -_world.FieldWidth/2) -new Point3D());
        Visual3dContent.Children.Add(terrain);

        //create goals
        var goalBlue = _shapesFactory.CreateBeam(50, 80, 150, GetMaterial(Colors.Brown));
        goalBlue.Transform = new TranslateTransform3D(new Point3D(-_world.FieldLength / 2, 0, 70) - new Point3D());
        Visual3dContent.Children.Add(goalBlue);

        var goalRed = _shapesFactory.CreateBeam(-50, 80, 150, GetMaterial(Colors.Blue));
        goalRed.Transform = new TranslateTransform3D(new Point3D(_world.FieldLength / 2, 0, 70) - new Point3D());
        Visual3dContent.Children.Add(goalRed);

        //create walls
        var wallMesh = new MeshGeometry3D();
        _shapesFactory.AddParalellogramToMesh(wallMesh, new Point3D(- _world.FieldLength / 2, 0, -_world.FieldWidth/ 2), new Vector3D(0, 10, _world.FieldWidth), new Vector3D(_world.FieldLength, 10, 0));
        _shapesFactory.AddParalellogramToMesh(wallMesh, new Point3D(- _world.FieldLength / 2, 0, -_world.FieldWidth / 2), new Vector3D(0, 10, _world.FieldWidth), new Vector3D(_world.FieldLength, 10, 0));
        _shapesFactory.AddParalellogramToMesh(wallMesh, new Point3D(- _world.FieldLength / 2, 0, -_world.FieldWidth / 2), new Vector3D(0, 10, _world.FieldWidth), new Vector3D(_world.FieldLength, 10, 0));
        _shapesFactory.AddParalellogramToMesh(wallMesh, new Point3D(- _world.FieldLength / 2, 0, -_world.FieldWidth / 2), new Vector3D(0, 10, _world.FieldWidth), new Vector3D(_world.FieldLength, 10, 0));

        var walls = new GeometryModel3D(wallMesh, GetMaterial(Colors.Gold));
        Visual3dContent.Children.Add(walls);
    }

    private static MaterialGroup GetMaterial(Color color)
    {
        var brush = new SolidColorBrush(color);
        var matGroup = new MaterialGroup();
        matGroup.Children.Add(new DiffuseMaterial(brush));
        matGroup.Children.Add(new SpecularMaterial(brush, 100));
        return matGroup;
    }

    private void CreateAxesGroup()
    {
        double xLength = Math.Abs(_world.Bounds.p2.X - _world.Bounds.p1.X) / 2;
        double yLength = Math.Abs(_world.Bounds.p2.Y - _world.Bounds.p1.Y) / 2;
        double zLength = Math.Abs(_world.Bounds.p2.Z - _world.Bounds.p1.Z) / 2;
        double thickness = (_world.Bounds.p2 - _world.Bounds.p1).Length / 500;
        _axesGroup.Children.Add(_shapesFactory.CreateLine(new Point3D(xLength, 0, 0), new Point3D(0, 0, 0), thickness, Brushes.Red));
        _axesGroup.Children.Add(_shapesFactory.CreateLine(new Point3D(0, yLength, 0), new Point3D(0, 0, 0), thickness, Brushes.Green));
        _axesGroup.Children.Add(_shapesFactory.CreateLine(new Point3D(0, 0, zLength), new Point3D(0, 0, 0), thickness, Brushes.Blue));
        _axesGroup.Freeze();
    }}
