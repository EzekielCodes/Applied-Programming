using Globals.Entities;
using System.Windows.Media.Media3D;

namespace LogicLayer;
public interface IGamePhysics
{
    void CollisionBallandPlayer(Players player, Ball ball, TimeSpan interval);
    void HandleBallCollisionZ(Ball ball);

    void HandleBallCollisionNegatiefX(Ball ball);
    void HandleBallCollisionNegatiefZ(Ball ball);

    void HandlePlayerCollisionX(Players player);
    void HandlePlayerCollisionZ(Players player);
    void HandleBallCollisionX(Ball ball);
    void CollisionPlayerandPlayer(Players playerOne, Players playerTwo, TimeSpan interval);
    Task MoveObject(Ball ball,TimeSpan interval);
}