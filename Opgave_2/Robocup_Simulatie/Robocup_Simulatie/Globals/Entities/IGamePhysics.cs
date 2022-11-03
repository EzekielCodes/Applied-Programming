using Globals.Entities;
using System.Windows.Media.Media3D;

namespace LogicLayer;
public interface IGamePhysics
{
    void CollisionBallandPlayer(Players player, Ball ball, TimeSpan interval);
    void HandleBallCollisionZ(Ball ball, int x);

    void HandleBallCollisionNegatiefX(Ball ball, int x);
    void HandleBallCollisionNegatiefZ(Ball ball, int x);

    void HandlePlayerCollisionX(Players player);
    void HandlePlayerCollisionZ(Players player);
    void HandleBallCollisionX(Ball ball, int x);
    void CollisionPlayerandPlayer(Players playerOne, Players playerTwo, TimeSpan interval);
    Point3D MoveObject(Ball ball, Players player, TimeSpan interval);
}