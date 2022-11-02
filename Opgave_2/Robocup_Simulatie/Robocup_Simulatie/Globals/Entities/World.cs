using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Globals.Interfaces;
using LogicLayer;

namespace Globals.Entities;
public class World : IWorld
{
    private readonly IGamePhysics _gamePhysics;
    private const int _worldSize = 1000;
    private IWorld? _game;

    public Ball Ball { get; set; }
    private readonly PeriodicTimer _timer = new(TimeSpan.FromMilliseconds(10));
    private bool _playing = false;
    private int _aantalspelers = 1;
    private readonly Random _random = new();

    private Point3D[] _arrayCheckPoint;
    public Point3D Origin => new();
    public (Point3D p1, Point3D p2) Bounds { get; private set; }
    public List<Players> TeamBlue { get; } = new();
    public List<Players> TeamRed { get; } = new();

    public int FieldLength => 900;
    public int FieldWidth => 600;

    public int GoalWidth => 150;
    private readonly int _playerRadius = 20;

    private Point3D _ballPosition;

    private CancellationTokenSource? _tokenSource;
    private CancellationToken token;


    public int AantalSpelers
    {
        get => _aantalspelers;
        set
        {
            if ( value > 0 && value < 4)
            {
                _aantalspelers = value;
            }
        }
    }

    //Ball IWorld.Ball => throw new NotImplementedException();

    public World()
    {
        Bounds = (new Point3D(-_worldSize / 2, -_worldSize / 2, -_worldSize / 2),
                      new Point3D(_worldSize / 2, _worldSize / 2, _worldSize / 2));
    }

    public void CreateItems()
    {
        CreatePlayers(_aantalspelers);
       
       
    }

    public void CreateBall()
    {
        Ball = new Ball(position: new(0, 10, 0), radius: 10, Colors.Orange);
    }

    private void CreatePlayers(int aantal)
    {
        _arrayCheckPoint = new Point3D[aantal];
        double speed = 0;
        double versnelling = 0.3;
        for (int i = 0; i < aantal; i++)
        {
            TeamRed.Add(new Players(GenerateRandomPoint(i,true), _playerRadius, speed, versnelling, axis: new(0, 20, 0), Colors.Red));
            
        }
        for (int i = 0; i < aantal; i++)
        {
            TeamBlue.Add(new Players(GenerateRandomPoint(i, false), _playerRadius, speed, versnelling, axis: new(0, 20, 0), Colors.Blue));
        }
    }

    private Point3D GenerateRandomPoint(int i, bool inverse)
    {
        Point3D point = new Point3D();
        if (inverse)
        {
            point.X = _random.Next(-_playerRadius, (FieldLength / 2)- _playerRadius);
        }
        else
        {
            point.X = _random.Next((-FieldLength / 2) + _playerRadius, _playerRadius);
        }
        
        point.Z = _random.Next((-FieldWidth/2)+ _playerRadius, (FieldWidth/2)- _playerRadius);
       
        
        if (!_arrayCheckPoint.Contains(point))
        {
            _arrayCheckPoint[i] = point;
        }
        else
        {
            GenerateRandomPoint(i,inverse);
        }


        return point;
    }


    public void StartMove()
    {
        _tokenSource?.Dispose();
        _tokenSource = new CancellationTokenSource();
        token = _tokenSource.Token;
        Task.Run(() => ExecSimulatieLoop(), token);

    }

    public void ExecSimulatieLoop()
    {
        DateTime previousTime = DateTime.Now;
        while (!token.IsCancellationRequested)
        {
            var ellapsedTime = DateTime.Now - previousTime;
            _ballPosition = Ball.Position;
            for (int i = 0; i < TeamBlue.Count; i++)
            {
                TeamBlue[i].Updatepostion(_ballPosition, ellapsedTime);
                CollisionWithBall(ellapsedTime);
                TeamRed[i].Updatepostion(_ballPosition, ellapsedTime);
            }

           
        }
    }

    public void CollisionWithBall(TimeSpan interval)
    {
        for (int i = 0; i < TeamBlue.Count; i++)
        {
            Point3D ballPosition = Ball.Position;
            Vector3D afstandTeamA = TeamBlue[i].Position - ballPosition;
            Vector3D afstandTeamB = TeamRed[i].Position - ballPosition;

            if (afstandTeamA.Length <= 30)
            {
                Debug.WriteLine("touch");
                CollisionBallandPlayer(TeamBlue[i], Ball, interval);

            }
            else if (afstandTeamB.Length <= 30)
            {
                CollisionBallandPlayer(TeamRed[i], Ball, interval);
            }
        }
    }

    public void CollisionBallandPlayer(Players player, Ball ball, TimeSpan interval)
    {
        player.Speed = 0;
        ball.Direction = player.Direction;
        ball.Speed = ball.MaxSpeed;
        ball.Position = MoveObject(ball,player, interval);
    }

    public Point3D MoveObject(Ball ball, Players player, TimeSpan interval)
    {
        Point3D position = ball.Position;
        Vector3D direction = player.Direction;
        direction.Normalize();
        
        position -= direction *  ball.Speed/1000 * interval.TotalMilliseconds;
        ball.Speed -= ball.RollingResistanceCoeffienct * interval.TotalMilliseconds;
        if (ball.Speed < 0) ball.Speed = 0;
        return position;
        
    }

    public void StopMove()
    {
        _tokenSource?.Cancel();
    }

    public void Stop()
    {
        _game?.Stop();
        _playing = false;
    }

    public void Start()
    {
        if (_game == null) return;

        _game.Start();

        _playing = true;
    }
}
