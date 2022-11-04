using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Globals.Interfaces;
using LogicLayer;

namespace Globals.Entities;
public class GameLogic : ILogic
{
    private readonly IGamePhysics _gamePhysics;
    private const int _worldSize = 1000;
    private ILogic? _game;

    public Ball Ball { get; set; }
    private readonly PeriodicTimer _timer = new(TimeSpan.FromMilliseconds(10));
    private bool _playing = false;
    private int _aantalspelers = 1;
    private int _scoreTeamOne = 0;
    private int _scoreTeamTwo = 0;
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

    public int ScoreTeamOne 
    {
        get => _scoreTeamOne;
        set => _scoreTeamOne = value;
    }
    
    public int ScoreTeamTwo
    {
        get => _scoreTeamTwo;
        set => _scoreTeamTwo = value;
    }

    public GameLogic()
    {
        Bounds = (new Point3D(-_worldSize / 2, -_worldSize / 2, -_worldSize / 2),
                      new Point3D(_worldSize / 2, _worldSize / 2, _worldSize / 2));
        _gamePhysics = new GamePhysics();
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


    public void StartMoveAsync()
    {
        _tokenSource?.Dispose();
        _tokenSource = new CancellationTokenSource();
        token = _tokenSource.Token;
        Task.Run(() => ExecSimulatieLoop(), token);

    }

    public  void ExecSimulatieLoop()
    {
        DateTime previousTime = DateTime.Now;
        while (!token.IsCancellationRequested)
        {
            var ellapsedTime = DateTime.Now - previousTime;
            previousTime = DateTime.Now;
            _ballPosition = Ball.Position;
            _gamePhysics.MoveObject(Ball, null,ellapsedTime);
            for (int i = 0; i < TeamBlue.Count; i++)
            {
                _ = GoalScored(Ball);
                _ = CollisionWithBall(ellapsedTime);
                _ = CollisionWithWall();
                _ = CollisionWithPlayers(ellapsedTime);
                _ = TeamBlue[i].Updatepostion(_ballPosition, ellapsedTime);
                _ = TeamRed[i].Updatepostion(_ballPosition, ellapsedTime);
            }

           
        }
    }

    private async Task CollisionWithBall(TimeSpan interval)
    {
        for (int i = 0; i < TeamBlue.Count; i++)
        {
            Point3D ballPosition = Ball.Position;
            Vector3D afstandTeamA = TeamBlue[i].Position - ballPosition;
            Vector3D afstandTeamB = TeamRed[i].Position - ballPosition;
            if (afstandTeamA.Length <= (TeamBlue[0].Radius + Ball.Radius))
            {
                _ = _gamePhysics.CollisionBallandPlayer(TeamBlue[i], Ball, interval);

            }
            else if (afstandTeamB.Length <= (TeamRed[0].Radius + Ball.Radius))
            {
                _ = _gamePhysics.CollisionBallandPlayer(TeamRed[i], Ball, interval);
            }
        }
    }

    private async Task CollisionWithPlayers(TimeSpan interval)
    {
        for (int i = 0; i < TeamBlue.Count; i++)
        {
            var player = TeamBlue[i];
            for (int x = 0; x < TeamBlue.Count; x++)
            {
                if ((TeamBlue[i].Position - TeamRed[x].Position).Length <= 40)
                {
                    //_gamePhysics.CollisionPlayerandPlayer(TeamBlue[i], TeamRed[x], interval);
                }
                else if ((TeamBlue[i].Position - TeamBlue[x].Position).Length <= 40)
                {
                    if (player == TeamBlue[x]) continue;
                    //_gamePhysics.CollisionPlayerandPlayer(TeamBlue[i], TeamBlue[x], interval);
                }

                else if ((TeamRed[i].Position - TeamRed[x].Position).Length <= 40)
                {
                    if (TeamRed[i] == TeamRed[x]) continue;
                   // _gamePhysics.CollisionPlayerandPlayer(TeamRed[i], TeamRed[x], interval);
                }
            }
        }
    }
    public async Task CollisionWithWall()
    {

       /* if (Ball.Position.X > 440 && Ball.Position.Z > -290 && Ball.Position.Z < 295)
        {
            Ball.Speed = 0;
            Ball.Position = new Point3D(0, 10, 0);
        }
        else if (Ball.Position.X > -440 && Ball.Position.Z > -290 && Ball.Position.Z < 290)
        {
            Ball.Speed = 0;
            Ball.Position = new Point3D(0, 10, 0);
        }
        else
        {*/
            if (Ball.Position.X > 435)
            {
               _=  _gamePhysics.HandleBallCollisionX(Ball, 435);
            }
            else if (Ball.Position.X < -435)
            {
                _= _gamePhysics.HandleBallCollisionNegatiefX(Ball, 435);
            }
            else if (Ball.Position.Z < -285)
            {
                _ = _gamePhysics.HandleBallCollisionNegatiefZ(Ball, 285);
            }

            else if (Ball.Position.Z > 285)
            {
                _ = _gamePhysics.HandleBallCollisionZ(Ball, 285);
            }

        //}
        {
            for (int x = 0; x < TeamBlue.Count; x++)
            {
                if (TeamBlue[x].Position.X >= FieldLength / 2 || TeamBlue[x].Position.X <= -FieldLength / 2)
                {
                    //_gamePhysics.HandlePlayerCollisionX(TeamBlue[x]);
                }
                if (TeamRed[x].Position.Z >= FieldWidth / 2 || TeamRed[x].Position.Z <= -FieldWidth / 2)
                {
                    //_gamePhysics.HandlePlayerCollisionZ(TeamRed[x]);
                }
            }
        }
    }

    public async Task GoalScored(Ball ball)
    {
        //goalBrown
        if(ball.Position == new Point3D((FieldLength / 2), 10, GoalWidth / 2))
        {
            _scoreTeamOne += 1;
            ball.Position = new Point3D(0, 10, 0);
            _=GoalScored();
        }
        //goal blue
        else if(ball.Position == new Point3D((FieldLength / 2), 10, GoalWidth / 2))
        {
            _scoreTeamTwo += 1;
            ball.Position = new Point3D(0, 10, 0);
            _ = GoalScored();
        }
    }

    public async Task GoalScored()
    {
        _arrayCheckPoint = new Point3D[TeamBlue.Count];
        double speed = 0;
        double versnelling = 0.3;
        for (int i = 0; i < TeamBlue.Count; i++)
        {
            TeamRed[i].Position = GenerateRandomPoint(i, true);
            TeamBlue[i].Position = GenerateRandomPoint(i, false);
            TeamBlue[i].Speed = speed;
            TeamRed[i].Versnelling = versnelling;
            TeamBlue[i].Versnelling = versnelling;
            TeamRed[i].Speed = speed;

        }
    }
    public void StopMove()
    {
        _tokenSource?.Cancel();
        _game?.Stop();
        _playing = false;
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
