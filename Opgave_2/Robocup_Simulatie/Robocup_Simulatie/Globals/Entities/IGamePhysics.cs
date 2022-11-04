using Globals.Entities;
using System.Windows.Media.Media3D;

namespace LogicLayer;
public interface IGamePhysics
{
    Task CollisionBallandPlayer(Players player, Ball ball, TimeSpan interval);
    Task HandleBallCollisionZ(Ball ball, int x);

    Task HandleBallCollisionNegatiefX(Ball ball, int x);
    Task HandleBallCollisionNegatiefZ(Ball ball, int x);

    void HandlePlayerCollisionX(Players player);
    void HandlePlayerCollisionZ(Players player);
    Task HandleBallCollisionX(Ball ball, int x);
    void CollisionPlayerandPlayer(Players playerOne, Players playerTwo, TimeSpan interval);
    Task MoveObject(Ball ball, Players player, TimeSpan interval);
}