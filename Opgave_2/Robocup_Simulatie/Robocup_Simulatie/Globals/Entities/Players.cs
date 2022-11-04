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
    private  Vector3D _accelaration;

    private Vector3D _velocity;
    public double Radius { get; init; }

    public Vector3D Direction { get; set; }

    public Color Color { get; }

    public double Speed
    {
        get => _speed;
        set => _speed = value;
    }

    public Vector3D Velocity
    {
        get => _velocity;
        set => _velocity = value;
    }

    public Vector3D Acceleration
    {
        get => _accelaration;
        set => _accelaration = value;
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

    public async Task Updatepostion(Point3D ball, TimeSpan interval)
    {
        this.Position += this.Velocity * interval.TotalSeconds;
        Velocity += this.Acceleration * interval.TotalSeconds;
        if(Velocity.Length > 100) Velocity.Normalize();

        //acceleration
        Vector3D direction = ball - this.Position;
        direction.Y = 0;
        direction.Normalize();
        Acceleration = direction * 40;
        
    }
    


    
}
