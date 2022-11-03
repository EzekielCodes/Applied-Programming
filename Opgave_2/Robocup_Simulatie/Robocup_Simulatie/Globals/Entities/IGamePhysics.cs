using Globals.Entities;
using System.Windows.Media.Media3D;

namespace LogicLayer;
public interface IGamePhysics
{
    void CollisionBallandPlayer(Players player, Ball ball, TimeSpan interval);
    void HandleBallCollisionZ(Ball ball);
    void HandlePlayerCollisionX(Players player);
    void HandlePlayerCollisionZ(Players player);
    void HandleBallCollisionX(Ball ball);
    Point3D MoveObject(Ball ball, Players player, TimeSpan interval);
}