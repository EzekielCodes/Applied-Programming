using Globals.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows;
using System.Diagnostics;

namespace Globals.Entities;
public record class Players : IItem3D
{
    public Point3D Position { get; set; }

    public double Scale => 1;
    private double _speed;

    public double Versnelling { get; set; }
    

    public double Radius { get; init; }

    public Vector3D Direction { get; set; }

    public Color Color { get; }

    public double Speed
    {
        get => _speed;
        set => _speed = value;
    }

    public Players(Point3D position, double radius, double speed, double versnelling, Vector3D axis, Color colours)
    {
        Position = position;
        Speed = speed;
        Versnelling = versnelling;
        Radius = radius;
        Direction = axis;
        Color = colours;
    }

    public void Updatepostion(Point3D ball, TimeSpan interval)
    {
        Vector3D direction = this.Position - ball;
        direction.Y = 0;
        this.Direction = direction;
        direction.Normalize();

        this.Position -= (direction * this.Speed/100 * interval.TotalMilliseconds);
        double x = this.Position.X;
        double z = this.Position.Z;
        this.Speed += this.Versnelling *  interval.TotalMilliseconds;
        if (this.Speed > 1) this.Speed = 0.01;
        Debug.WriteLine(this.Position);
        this.Speed = Speed;        
        
    }

    public void MoveObject(Ball ball , TimeSpan interval)
    {
        Point3D pos = ball.Position;
        //Vector3D direction = ball.
    }

    
}
