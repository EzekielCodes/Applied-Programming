using Globals.Entities;
using System.Windows.Media.Media3D;

namespace Globals.Interfaces;
public interface ILogic
{
    (Point3D p1, Point3D p2) Bounds { get; }
    Point3D Origin { get; }
    
    bool Playing { get; set; }
    int FieldLength { get; }

    int FieldWidth { get; }
    int GoalWidth { get; }

    int AantalSpelers { get; set; }

    int ScoreTeamOne { get; set; }
    int ScoreTeamTwo { get; set; }

    List<Players> TeamRed { get; }
    List<Players> TeamBlue { get; }

    public Ball Ball { get; set; }

    void StopMove();

    void CreateItems();

    void CreateBall();

    void StartMoveAsync();

    void  ResetPlayers();
}