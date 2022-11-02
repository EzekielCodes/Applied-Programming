using Globals.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows;

namespace Globals.Entities;
public record class Players : IItem3D
{
    public Point3D Position { get; set; }

    public double Scale => 1;
    private double _speed;

    public double Versnelling { get; set; }
    

    public double Radius { get; init; }

    public Vector3D Axis { get; set; }

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
        Axis = axis;
        Color = colours;
    }

    public void Updatepostion(Point3D ball, TimeSpan interval)
    {
        Vector3D direction = this.Position - ball;
        direction.Normalize();
        this.Position -= (direction * 1 * interval.TotalSeconds);
        Speed = this.Versnelling + interval.TotalSeconds;
        if (Speed < 1) Speed = 1;
        this.Speed = Speed;
       /* Position += Speed * interval.TotalSeconds;
        Speed += Versnelling * interval.TotalSeconds;
        
        Versnelling = Speed / interval.TotalSeconds;*/
        
        
    }
}
