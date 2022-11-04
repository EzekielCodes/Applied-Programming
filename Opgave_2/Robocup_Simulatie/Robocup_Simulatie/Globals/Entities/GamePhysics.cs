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


    public async Task  MoveObject(Ball ball, Players player, TimeSpan interval)
    {
        /* Point3D position = ball.Position;
         Vector3D direction = player.Direction;
         direction.Normalize();
         ball.Position += direction * ball.Speed   * interval.TotalMilliseconds;
         ball.Speed = 0.1/9.81 * interval.TotalMilliseconds;
         //if (ball.Speed < 0.4) ball.Speed = 0.4;
         if (ball.Speed < 0) ball.Speed = 0;
         //return position;*/

        ball.Position += ball.Velocity * interval.TotalSeconds;
        Vector3D decelaration = -ball.Velocity;
        decelaration.Normalize();
        decelaration *= 0.1 * 9.81;
        if(ball.Velocity.Length < 0.981)
        {
            ball.Velocity = new Vector3D(0,0,0);
        }
        else
        {
            ball.Velocity += decelaration * interval.TotalSeconds;
        }

    }


    public async Task CollisionBallandPlayer(Players player, Ball ball, TimeSpan interval)
    {
        //player.Speed = 0;
        Vector3D newBallVelocity = player.Velocity;
        newBallVelocity.Normalize();
        ball.Velocity = newBallVelocity * 3;
        
        _ = MoveObject(ball, player, interval);
    }

    public void CollisionPlayerandPlayer(Players playerOne, Players playerTwo, TimeSpan interval)
    {
        
       /* double Speed = playerOne.Speed - playerTwo.Speed;
        
        Vector3D tempDirection = playerOne.Position - playerTwo.Position;
        tempDirection.Y = 0;
        playerOne.Direction = tempDirection;
        playerOne.Speed -= Speed;
        playerTwo.Direction = -tempDirection;
        playerTwo.Speed += Speed;*/
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

    public async Task HandleBallCollisionNegatiefZ(Ball ball, int x)
    {
        Debug.WriteLine("ball speed before" + ball.Speed);
        Vector3D sleep = ball.Direction;
        sleep.Z = ball.Direction.Z;
        sleep.Y = 0;
        sleep.X = -ball.Direction.X;
        Debug.WriteLine("sleeep" + sleep);

        ball.Direction = sleep;
        /*Vector3D sleep = ball.Direction;
        Debug.WriteLine("sleeep" + sleep);
        Vector3D normal = (Vector3D)new Point3D(ball.Position.Z, 10, x);
        normal.Normalize();
        Debug.WriteLine("ball -Z normal " + normal);
        Debug.WriteLine("ball -Z direction " + ball.Direction);
        //ball.Direction = new Vector3D(normal.X, normal.Y, normal.Z);
        ball.Direction += normal;*/
        Debug.WriteLine("ball speed after" + ball.Speed);
        Debug.WriteLine("ball-Z " + ball.Direction);
        Debug.WriteLine("ballX " + ball.Position);
    }

    public async Task HandleBallCollisionZ(Ball ball, int x)
    {
        Debug.WriteLine("ball speed before" + ball.Speed);
        Vector3D sleep = ball.Direction;
        sleep.Z = ball.Direction.Z;
        sleep.Y = 0;
        sleep.X = -ball.Direction.X;
        Debug.WriteLine("sleeep" + sleep);

        ball.Direction = sleep;
        /*Vector3D sleep = ball.Direction;
        Debug.WriteLine("sleeep" + sleep);
        Vector3D normal = (Vector3D)new Point3D(ball.Position.Z, 10, x);
        normal.Normalize();
        Debug.WriteLine("ball Z normal " + normal);
        Debug.WriteLine("ball Z direction " + ball.Direction);
        //ball.Direction = new Vector3D(normal.X, normal.Y, normal.Z);
        ball.Direction +=  normal;*/
        Debug.WriteLine("ball speed after" + ball.Speed);
        Debug.WriteLine("ball Z " + ball.Direction);
        Debug.WriteLine("ballX " + ball.Position);
    }

    public async Task HandleBallCollisionNegatiefX(Ball ball, int x)
    {
        Debug.WriteLine("ball speed before" + ball.Speed);
        Vector3D sleep = ball.Direction;
        sleep.Z = -ball.Direction.Z;
        sleep.Y = 0;
        sleep.X = ball.Direction.X;
        Debug.WriteLine("sleeep" + sleep);

        ball.Direction = sleep;
        /* Vector3D normal = (Vector3D)new Point3D(ball.Position.Z, 10, x);
         normal.Normalize();
         Debug.WriteLine("ball-X normal " + normal);
         Debug.WriteLine("ball -X direction " + ball.Direction);
         //ball.Direction = new Vector3D(normal.X, normal.Y, normal.Z);*/
        Debug.WriteLine("ball speed after" + ball.Speed);
        Debug.WriteLine("ball-X " + ball.Direction);
        Debug.WriteLine("ball - X " + ball.Position);
    }

    public async Task HandleBallCollisionX(Ball ball, int x)
    {
        Debug.WriteLine("ball speed before" + ball.Speed);
        Vector3D sleep = ball.Direction;
        sleep.Z = -ball.Direction.Z;
        sleep.Y = 0;
        sleep.X = ball.Direction.X;
        Debug.WriteLine("sleeep" + sleep);
        
        ball.Direction = sleep;
        /* Vector3D sleep = ball.Direction;
         Debug.WriteLine("sleeep" + sleep);
         Vector3D normal = (Vector3D)new Point3D(ball.Position.Z, 10, x);
         normal.Normalize();
         Debug.WriteLine("ball X nmormal " + normal);
         Debug.WriteLine("ball X direction " + ball.Direction);
         //ball.Direction = new Vector3D(normal.X, normal.Y,normal.Z);
         ball.Direction += normal;*/
        Debug.WriteLine("ball speed after" + ball.Speed);
        Debug.WriteLine("ballX " + ball.Direction);
        Debug.WriteLine("ballX " + ball.Position);
    }

    public void HandeleCollisionWithWall(Ball ball)
    {
        double x = ball.Position.X;
        double z = ball.Position.Z;

    }

}
