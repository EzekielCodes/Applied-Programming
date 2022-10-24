using Globals.Entities;
using System.Windows.Media.Media3D;

namespace Globals.Interfaces;
public interface IWorld
{
    (Point3D p1, Point3D p2) Bounds { get; }
    Point3D Origin { get; }
    List<IItem3D> Items { get; }

    int FieldLength { get; }

    int FieldWidth { get; }
    int GoalWidth { get; }

    int AantalSpelers { get; set; }

    List<Cylinder> TeamRed { get; }
    List<Cylinder> TeamBlue { get; }
    void Start();

    void Stop();

    void MovePlayers();

    void CreateItems();
}