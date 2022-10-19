using System.Windows.Media.Media3D;

namespace Globals.Interfaces;
public interface IWorld
{
    (Point3D p1, Point3D p2) Bounds { get; }
    Point3D Origin { get; }
    List<IItem3D> Items { get; }
}