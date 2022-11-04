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

    /// <summary>
    /// Deze methode wordt gebruik om de ball altijd te bewegen en vertragen
    /// </summary>
    /// <param name="ball"></param>
    /// <param name="interval"></param>
    /// <returns></returns>
    public async Task  MoveObject(Ball ball, TimeSpan interval)
    {
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
            Debug.WriteLine(ball.Velocity.Length);
            ball.Velocity += decelaration * interval.TotalSeconds;
        }

    }

    /// <summary>
    /// Deze methode wordt opgeroepen als een collision is tussen de ball en speler
    /// </summary>
    /// <param name="player"></param>
    /// <param name="ball"></param>
    /// <param name="interval"></param>
    public void CollisionBallandPlayer(Players player, Ball ball, TimeSpan interval)
    {
        Vector3D newBallVelocity = player.Velocity;
        newBallVelocity.Normalize();
        ball.Velocity = newBallVelocity * 40;
        player.Velocity = newBallVelocity * 0;
    }

    /// <summary>
    /// De methode wordt opgeroepen als een botsing tussen 2 spelers is
    /// </summary>
    /// <param name="playerOne"></param>
    /// <param name="playerTwo"></param>
    /// <param name="interval"></param>
    public void CollisionPlayerandPlayer(Players playerOne, Players playerTwo, TimeSpan interval)
    {
        double speed = playerOne.Velocity.Length - playerTwo.Velocity.Length;
        Vector3D temp = playerOne.Velocity;
        Vector3D tempTwo = playerTwo.Velocity;
        playerOne.Velocity = new Vector3D(-temp.X,0,temp.Z) ;
        playerTwo.Velocity = new Vector3D(-tempTwo.X, 0, tempTwo.Z);

    }

    /// <summary>
    /// Deze methode handelt een collision tussen de spelers en Wall
    /// </summary>
    /// <param name="player"></param>
    public void HandlePlayerCollisionX(Players player)
    {
        Players temp = player;
        player.Velocity = new Vector3D(temp.Velocity.X, 0, -temp.Velocity.Z);
    }

    /// <summary>
    /// Deze methode handelt een collision tussen de spelers en Wall
    /// </summary>
    /// <param name="player"></param>
    public void HandlePlayerCollisionZ(Players player)
    {
        Players temp = player;
        player.Velocity = new Vector3D(-temp.Velocity.X, 0, temp.Velocity.Z);
    }

    /// <summary>
    /// Deze methode handelt een collision tussen de spelers en Wall
    /// </summary>
    /// <param name="player"></param>
    public void HandleBallCollisionNegatiefZ(Ball ball)
    {
        Vector3D change = ball.Velocity;
        ball.Velocity = new Vector3D(-change.X, 0, change.Z);
    }
    /// <summary>
    /// Deze methode handelt een collision tussen de spelers en Wall
    /// </summary>
    /// <param name="player"></param>
    public void HandleBallCollisionZ(Ball ball)
    {
        Vector3D change = ball.Velocity;
        ball.Velocity = new Vector3D(-change.X, 0, change.Z);
    }

    /// <summary>
    /// Deze methode handelt een collision tussen de ball en Wall
    /// </summary>
    /// <param name="player"></param>
    public void HandleBallCollisionNegatiefX(Ball ball)
    {
        Vector3D change = ball.Velocity;
        ball.Velocity = new Vector3D(change.X, 0, -change.Z);
    }

    /// <summary>
    /// Deze methode handelt een collision tussen de ball en Wall
    /// </summary>
    /// <param name="player"></param
    public void HandleBallCollisionX(Ball ball)
    {
        Vector3D change = ball.Velocity;
        ball.Velocity = new Vector3D(change.X, 0, -change.Z);
    }

}
