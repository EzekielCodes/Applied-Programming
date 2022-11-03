using Globals.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace LogicLayer;
public class GamePhysics : IGamePhysics
{


    public Point3D MoveObject(Ball ball, Players player, TimeSpan interval)
    {
        Point3D position = ball.Position;
        Vector3D direction = player.Direction;
        direction.Normalize();
        position -= direction * ball.Speed / 100 * interval.TotalMilliseconds;
        ball.Speed -= ball.RollingResistanceCoeffienct * interval.TotalMilliseconds;
        if (ball.Speed < 0) ball.Speed = 0;
        return position;

    }



    public void CollisionBallandPlayer(Players player, Ball ball, TimeSpan interval)
    {
        player.Speed = 0;
        ball.Direction = player.Direction;
        ball.Speed = ball.MaxSpeed;
        ball.Position = MoveObject(ball, player, interval);
    }

    public void HandlePlayerCollisionX(Players player)
    {
        Vector3D direction = player.Direction;
        direction.X = -direction.X;
        player.Direction = direction;
    }

    public void HandlePlayerCollisionZ(Players player)
    {
        Vector3D direction = player.Direction;
        direction.Z = -direction.Z;
        player.Direction = direction;
    }

    public void HandleBallCollisionZ(Ball ball)
    {
        Vector3D direction = ball.Direction;
        direction.Z = -direction.Z;
        ball.Direction = direction;
    }

    public void HandleBallCollisionX(Ball ball)
    {
        Vector3D direction = ball.Direction;
        direction.X = -direction.X;
        ball.Direction = direction;
    }

}
