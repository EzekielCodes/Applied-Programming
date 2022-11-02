using Globals.Entities;
using System.Windows.Media.Media3D;

namespace Globals.Interfaces;
public interface IWorld
{
    (Point3D p1, Point3D p2) Bounds { get; }
    Point3D Origin { get; }
    

    int FieldLength { get; }

    int FieldWidth { get; }
    int GoalWidth { get; }

    int AantalSpelers { get; set; }

    List<Players> TeamRed { get; }
    List<Players> TeamBlue { get; }

    public Ball Ball { get; set; }
    void Start();

    void Stop();

    

    void CreateItems();

    void CreateBall();

    void StartMove();
}