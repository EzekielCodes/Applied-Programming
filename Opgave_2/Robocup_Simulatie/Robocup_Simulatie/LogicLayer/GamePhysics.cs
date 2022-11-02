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


    public Point3D MoveObject(Ball ball, TimeSpan interval)
    {
        Point3D position = ball.Position;
        Vector3D direction = ball.Direction;
        direction.Normalize();
        position -= (direction * (ball.Speed) * interval.TotalMilliseconds);
        ball.Speed -= ((ball.RollingResistanceCoeffienct * interval.TotalMilliseconds));
        if (ball.Speed < 0) ball.Speed = 0;
        return position;
    }



    public void CollisionBallandPlayer(Players player, Ball ball)
    {
        player.Speed = 0;
        ball.Direction = player.Direction;
        ball.Speed = ball.MaxSpeed;
    }

}
