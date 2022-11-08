using Globals.Entities;
using Globals.Interfaces;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using Wpf3dTools.Interfaces;

namespace PresentationLayer.ViewModels;
public class MainViewModel : ObservableObject
{
    private readonly ILogic _logic;
    private readonly ISphericalCameraController _cameraController;
    private readonly IShapesFactory _shapesFactory;
    public ProjectionCamera Camera { get; private set; }
    private ProjectionCamera _projectitonCamera;
    private PerspectiveCamera _perspectiveCamera;

    private int _checker = 0;
    private readonly PeriodicTimer _timer = new(TimeSpan.FromMilliseconds(10));

    private string _title = "WpfApp (MVVM)";

    // binding properties
    private int _currentTime = 120;
    private DateTime _startTime;
    private DateTime _endTime;
    private TimeSpan _matchTime = new TimeSpan(0,2,0);
    private PeriodicTimer? _gametimer;

    public string Title
    {
        get => _title;
        private set => SetProperty<string>(ref _title, value);
    }

    private readonly Model3DGroup _model3dGroup = new();
    private readonly Model3DGroup _axesGroup = new();

    private readonly List<GeometryModel3D> _teamBlue = new();
    private readonly List<GeometryModel3D> _teamRed= new();

    private readonly Model3DGroup _teamBlueitems = new();
    private readonly Model3DGroup _teamReditems = new();
    public Model3DGroup Visual3dContent => _model3dGroup;
    private GeometryModel3D _ball = new();
    private bool _showAxes;

    public String CurrentTime { get; set; }
    public string Time => DateTime.Now.ToLongTimeString();
    private TimeSpan _pauzetime;
    public IRelayCommand<MouseWheelEventArgs> ZoomCommand { get; private set; }
    public IRelayCommand<Vector> ControlByMouseCommand { get; private set; }

    public IRelayCommand ChangeviewCommand { get; }

    public int AantalSpelers
    {
        get => _logic?.AantalSpelers ?? 0;
        set
        {
            if (_logic != null) _logic.AantalSpelers = value;
        }
    }

    public int ScoreTeamTwo 
    {
        get => _logic.ScoreTeamTwo;
        set
        {
            if(_logic != null) _logic.ScoreTeamTwo = value;
        }
    }
    public int ScoreTeamOne
    {
        get => _logic.ScoreTeamOne;
        set
        {
            if (_logic != null) _logic.ScoreTeamOne= value;
        }
    }

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

    public IRelayCommand PlayCommand { get; }
    public IRelayCommand PauseCommand { get; }
    public IRelayCommand ResetCommand { get; }
    public bool AantalSpelersisEnabled { get; set; }

    public MainViewModel(ILogic logic, ISphericalCameraController cameraController, IShapesFactory shapesFactory)
    {
        
        BeginFilters();
         _logic = logic;
        _cameraController = cameraController;
        _shapesFactory = shapesFactory;

        Cameraaanmaken();
        Camerachoice();
        Init3DPresentation();
        InitPresentation();
        //_ = Animate();



        ZoomCommand = new RelayCommand<MouseWheelEventArgs>(ZoomByMouse);
        ControlByMouseCommand = new RelayCommand<Vector>(ControlByMouse);
       

        PlayCommand = new RelayCommand(StartGame, () => !_logic.Playing);
        PauseCommand = new RelayCommand(PauseGame, () => _logic.Playing);
        ResetCommand = new RelayCommand(ResetGame);
        ChangeviewCommand = new RelayCommand(Camerachoice);

    }

    private void BeginFilters()
    {
        CurrentTime = String.Format("0{0}:{1}", 120 / 60, 120 % 60);
        AantalSpelersisEnabled = true;
        ScoreTeamOne = 0;
        ScoreTeamTwo = 0;

    }

    private async void StartGame()
    {
        _logic.Playing = true;
        AantalSpelersisEnabled = false;
        UpdateUiCommandsState();
        _gametimer = new(TimeSpan.FromMilliseconds(1));
        if (_pauzetime.Seconds == 0)
        {

            _startTime = DateTime.Now;
            _endTime = _startTime.Add(_matchTime);
            OnPropertyChanged(nameof(AantalSpelersisEnabled));
            _logic.CreateItems();
            InitItemGeometries();

        }
        else
        {
            _endTime = DateTime.Now.Add(_pauzetime);
        }
        _logic?.StartMoveAsync();

    }

    private void PauseGame()
    {
        if(_currentTime > 0)
        {
            _pauzetime = _endTime.Subtract(DateTime.Now);
        }
        else
        {
            _pauzetime = new TimeSpan(0);
            Debug.WriteLine(_pauzetime);
            _currentTime = 120;
            _logic.TeamBlue.Clear();
            _logic.TeamRed.Clear();
            _teamBlue.Clear();
            _teamRed.Clear();
            BeginFilters();
            OnPropertyChanged(nameof(AantalSpelersisEnabled));
            OnPropertyChanged(nameof(CurrentTime));
          
        }
        
        _gametimer?.Dispose();
        _logic?.StopMove();
        UpdateUiCommandsState();
    }

    private void StopGame()
    {
        _pauzetime = new TimeSpan(0);
        _currentTime = 120;
        _logic.TeamBlue.Clear();
        _logic.TeamRed.Clear();
        _teamBlue.Clear();
        _teamRed.Clear();
        BeginFilters();
        OnPropertyChanged(nameof(AantalSpelersisEnabled));
        OnPropertyChanged(nameof(CurrentTime));
        _gametimer?.Dispose();
        _logic?.StopMove();
        _model3dGroup.Children.Remove(_teamBlueitems);
        _model3dGroup.Children.Remove(_teamReditems);
        _teamBlueitems.Children.Clear();
        _teamBlueitems.Children.Clear();
        InitItemGeometries();
        UpdateUiCommandsState();
    }

    private void ResetGame()
    {
        _logic.ResetPlayers();

    }

    public void UpdateWorldDisplay()
    {
        if ((_logic != null) && (_currentTime <= 0)) StopGame();

        if (_logic.Playing)
        {
            UpdateUiCommandsState();
            if (DateTime.Now >= _endTime)
            {
                _currentTime = 0;
                _logic.StopMove();
                
            }
            CurrentTime = String.Format("{0:mm}:{0:ss}", _endTime - DateTime.Now);
            OnPropertyChanged(nameof(ScoreTeamTwo));
            OnPropertyChanged(nameof(ScoreTeamOne));
            OnPropertyChanged(nameof(CurrentTime));
            var itemTransformBall = new Transform3DGroup();
            itemTransformBall.Children.Add(new ScaleTransform3D(_logic.Ball.Scale, _logic.Ball.Scale, _logic.Ball.Scale));
            itemTransformBall.Children.Add(new TranslateTransform3D(_logic.Ball.Position - _logic.Origin));
            _ball.Transform = itemTransformBall;


            for (int i = 0; i < _teamBlue.Count; i++)
            {
                var itemTransform = new Transform3DGroup();
                itemTransform.Children.Add(new ScaleTransform3D(_logic.TeamBlue[i].Scale, _logic.TeamBlue[i].Scale, _logic.TeamBlue[i].Scale));
                itemTransform.Children.Add(new TranslateTransform3D(_logic.TeamBlue[i].Position - _logic.Origin));
                _teamBlue[i].Transform = itemTransform;
            };

            for (int i = 0; i < _teamRed.Count; i++)
            {
                var itemTransform = new Transform3DGroup();
                itemTransform.Children.Add(new ScaleTransform3D(_logic.TeamRed[i].Scale, _logic.TeamRed[i].Scale, _logic.TeamRed[i].Scale));
                itemTransform.Children.Add(new TranslateTransform3D(_logic.TeamRed[i].Position - _logic.Origin));
                _teamRed[i].Transform = itemTransform;
            };

        }


    }

    private void UpdateUiCommandsState()
    {
        PlayCommand.NotifyCanExecuteChanged();
        PauseCommand.NotifyCanExecuteChanged();
    }


    private void Init3DPresentation()
    {
        SetupCamera();
        SetUpLights();
        CreateAxesGroup();
        ShowAxes = true;

    }

    private void Cameraaanmaken()
    {
        _projectitonCamera = _cameraController.Camera;
        _perspectiveCamera = new PerspectiveCamera(new Point3D(_logic.Origin.X, 500, _logic.Origin.Z), new Vector3D(0, -90, -1), new Vector3D(0, 1, 0), 100);
    }

    /// <summary>
    /// wordt gebruik om een keuze te maken tussen beide cameras
    /// </summary>
    private void Camerachoice()
    {
        if (_checker == 0)
        {
            Camera = _projectitonCamera;
            _checker = 1;

            OnPropertyChanged(nameof(Camera));
        }
        else
        {
            _checker = 0;
            Camera = _perspectiveCamera;
            OnPropertyChanged(nameof(Camera));
        }
    }

    private void SetupCamera()
    {
        double l1 = (_logic.Bounds.p1 - _logic.Origin).Length;
        double l2 = (_logic.Bounds.p2 - _logic.Origin).Length;
        double radius = 2.3 * Math.Max(l1, l2);
        _cameraController.PositionCamera(radius, Math.PI / 10, 2.0 * Math.PI / 5);
    }

    private void ZoomByMouse(MouseWheelEventArgs? args)
    {
        _cameraController.Zoom(args!.Delta);
    }

    public void ProcessKey(Key key)
    {
        _cameraController.ControlByKey(key);
    }
    public void Zoom(int delta)
    {
        _cameraController.Zoom(delta);
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
        foreach (var item in _logic.TeamBlue)
        {
            var x = _shapesFactory.CreateCylinder(item.Radius, item.Direction, GetMaterial(Colors.Blue));
            _teamBlue.Add(x);
            _teamBlueitems.Children.Add(x);
            _model3dGroup.Children.Add(_teamBlueitems);
        }

        foreach (var item in _logic.TeamRed)
        {

            var x = _shapesFactory.CreateCylinder(item.Radius, item.Direction, GetMaterial(Colors.Red));
            _teamRed.Add(x);
            _teamReditems.Children.Add(x);
            _model3dGroup.Children.Add(_teamReditems);
        }
    }


    private void InitPresentation()
    {
        _logic.CreateBall();
        //terrain aanmaken
        var terrain = _shapesFactory.CreateParallelogram
            (
                side1: new Vector3D(0, 0, _logic.FieldWidth),
                side2: new Vector3D(_logic.FieldLength, 0, 0),
                materials: GetMaterial(Colors.LightGreen),
                backMaterials: GetMaterial(Colors.Green)

            );

        terrain.Transform = new TranslateTransform3D(new Point3D(- _logic.FieldLength / 2, 0, -_logic.FieldWidth/2) -new Point3D());
        Visual3dContent.Children.Add(terrain);

        //create goals
        var goalBlue = _shapesFactory.CreateBeam(-20, 100, _logic.GoalWidth, GetMaterial(Colors.Brown));
        goalBlue.Transform = new TranslateTransform3D(new Point3D((-_logic.FieldLength / 2) , 0, _logic.GoalWidth/2) - new Point3D());
        Visual3dContent.Children.Add(goalBlue);

        var goalRed = _shapesFactory.CreateBeam(20, 100, _logic.GoalWidth, GetMaterial(Colors.Blue));
        goalRed.Transform = new TranslateTransform3D(new Point3D((_logic.FieldLength / 2) , 0, _logic.GoalWidth/2) - new Point3D());
        Visual3dContent.Children.Add(goalRed);

        //create walls
        var wallMesh = new MeshGeometry3D();
        _shapesFactory.AddParalellogramToMesh(wallMesh, new Point3D(- _logic.FieldLength / 2, 0, -_logic.FieldWidth/ 2), new Vector3D(0, 20, 0), new Vector3D(_logic.FieldLength, 0, 0));
        _shapesFactory.AddParalellogramToMesh(wallMesh, new Point3D(- _logic.FieldLength / 2, 0, _logic.FieldWidth / 2), new Vector3D(0, 20, 0), new Vector3D(0, 0, - _logic.FieldWidth));
        _shapesFactory.AddParalellogramToMesh(wallMesh, new Point3D(-_logic.FieldLength / 2, 0, _logic.FieldWidth / 2), new Vector3D(0, 20, 0), new Vector3D(_logic.FieldLength, 0, 0));
        _shapesFactory.AddParalellogramToMesh(wallMesh, new Point3D(_logic.FieldLength / 2, 0, - _logic.FieldWidth / 2), new Vector3D(0, 20, 0), new Vector3D(0, 0,  _logic.FieldWidth));

        var walls = new GeometryModel3D(wallMesh, GetMaterial(Colors.Gold));
        walls.BackMaterial = GetMaterial(Colors.Gold);
        Visual3dContent.Children.Add(walls);

        _ball = _shapesFactory.CreateSphere(GetMaterial(Colors.Orange));
        Visual3dContent.Children.Add(_ball);

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
        double xLength = Math.Abs(_logic.Bounds.p2.X - _logic.Bounds.p1.X) / 2;
        double yLength = Math.Abs(_logic.Bounds.p2.Y - _logic.Bounds.p1.Y) / 2;
        double zLength = Math.Abs(_logic.Bounds.p2.Z - _logic.Bounds.p1.Z) / 2;
        double thickness = (_logic.Bounds.p2 - _logic.Bounds.p1).Length / 500;
        _axesGroup.Children.Add(_shapesFactory.CreateLine(new Point3D(xLength, 0, 0), new Point3D(0, 0, 0), thickness, Brushes.Red));
        _axesGroup.Children.Add(_shapesFactory.CreateLine(new Point3D(0, yLength, 0), new Point3D(0, 0, 0), thickness, Brushes.Green));
        _axesGroup.Children.Add(_shapesFactory.CreateLine(new Point3D(0, 0, zLength), new Point3D(0, 0, 0), thickness, Brushes.Blue));
        _axesGroup.Freeze();
    }}
