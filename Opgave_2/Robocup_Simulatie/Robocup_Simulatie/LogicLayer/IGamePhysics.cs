using Globals.Entities;
using System.Windows.Media.Media3D;

namespace LogicLayer;
public interface IGamePhysics
{
    void CollisionBallandPlayer(Players player, Ball ball);
    Point3D MoveObject(Ball ball, TimeSpan interval);
}