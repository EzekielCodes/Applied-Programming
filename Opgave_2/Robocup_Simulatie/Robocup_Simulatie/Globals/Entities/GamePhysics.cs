using Globals.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        ball.Position -= direction * ball.Speed   * interval.TotalMilliseconds;
        ball.Speed -= 0.1/9.81 * interval.TotalMilliseconds;
        if (ball.Speed < 0) ball.Speed = 0;
        return position;

    }


    public void CollisionBallandPlayer(Players player, Ball ball, TimeSpan interval)
    {
        //player.Speed = 0;
        ball.Direction = player.Direction;
        ball.Speed = ball.MaxSpeed;
        ball.Position = MoveObject(ball, player, interval);
    }

    public void CollisionPlayerandPlayer(Players playerOne, Players playerTwo, TimeSpan interval)
    {
        
        double Speed = playerOne.Speed - playerTwo.Speed;
        
        Vector3D tempDirection = playerOne.Position - playerTwo.Position;
        tempDirection.Y = 0;
        playerOne.Direction = tempDirection;
        playerOne.Speed -= Speed;
        playerTwo.Direction = -tempDirection;
        playerTwo.Speed += Speed;
        //Debug.WriteLine(playerOne.Direction + " 2 " + playerTwo.Direction);
        //MovePlayers(playerOne, playerTwo, tempDirection, interval);
    }

    public void MovePlayers(Players playerOne, Players playerTwo, Vector3D direction, TimeSpan interval)
    {
        //direction.Normalize();
        /* playerTwo.Direction -= direction;
         playerOne.Direction = direction;
         playerTwo.Position = new Point3D(playerTwo.Position.X - 10 , 0, playerTwo.Position.Z - 10);
         playerOne.Position = new Point3D(playerOne.Position.X - 10, 0, playerOne.Position.Z - 10);*/

        playerOne.Position -= (direction * playerOne.Speed) / 100 * interval.TotalMilliseconds;
        //playerOne.Speed = playerOne.Versnelling * interval.TotalMilliseconds;
        if (playerOne.Speed > 1) playerOne.Speed = 0.01;

        playerTwo.Position -= (direction * playerTwo.Speed) / 100 * interval.TotalMilliseconds;
        //playerTwo.Speed = playerTwo.Versnelling * interval.TotalMilliseconds;
        if (playerTwo.Speed > 1) playerTwo.Speed = 0.01;

    }

    public void HandlePlayerCollisionX(Players player)
    {
        player.Speed = 0;
        Vector3D direction = player.Direction;
        direction.X = -direction.X;
        player.Direction = direction;
    }

    public void HandlePlayerCollisionZ(Players player)
    {
        player.Speed = 0;
        Vector3D direction = player.Direction;
        direction.Z = -direction.Z;
        player.Direction = direction;
    }

    public void HandleBallCollisionNegatiefZ(Ball ball, int x)
    {   
        Vector3D normal = (Vector3D)new Point3D(-x, 10, ball.Position.Z);
        normal.Normalize();
        ball.Direction += normal;
        //ball.Direction += normal;
        Debug.WriteLine("ball-Z " +ball.Position);
        Debug.WriteLine("ballX " + ball.Position);
    }

    public void HandleBallCollisionZ(Ball ball, int x)
    {
        Vector3D normal = (Vector3D)new Point3D(-x, 10, ball.Position.Z);
        normal.Normalize();
        ball.Direction += normal;
        //ball.Direction +=  normal;
        Debug.WriteLine("ball Z " + ball.Position);
        Debug.WriteLine("ballX " + ball.Position);
    }

    public void HandleBallCollisionNegatiefX(Ball ball, int x)
    {
        Vector3D normal = (Vector3D)new Point3D(-x, 10, ball.Position.Z);
        normal.Normalize();
        ball.Direction += normal;
        //ball.Direction += normal;
        Debug.WriteLine("ball-X " + ball.Position);
        Debug.WriteLine("ballX " + ball.Position);
    }

    public void HandleBallCollisionX(Ball ball, int x)
    {
        Vector3D normal = (Vector3D)new Point3D(-x, 10, ball.Position.Z);
        normal.Normalize();
        ball.Direction += normal;
        //ball.Direction += normal;
        Debug.WriteLine("ballX " + ball.Position);
        Debug.WriteLine("ballX " + ball.Position);
    }

    public void HandeleCollisionWithWall(Ball ball)
    {
        double x = ball.Position.X;
        double z = ball.Position.Z;

    }

}
