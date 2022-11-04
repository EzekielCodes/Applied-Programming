using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Globals.Interfaces;

namespace Globals.Entities;
public record class Ball : IBall
{
    private const double _gravConstant = 9.81;
    private double _rollingDeceleration;
    private double _speed;

    public Point3D Position { get; set; }

    public static double Radius = 10;

    public double RollingResistanceCoeffienct
    {
        get => _rollingDeceleration / _gravConstant;
        set => _rollingDeceleration = value;
        
    }

    public Double Speed
    {
        get => _speed;
        set => _speed = value;
    }

    public Vector3D Direction { get; set; }
    public Color Color { get; }

    public int Scale => 10;
    private Vector3D _accelaration;

    private Vector3D _velocity;

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

    //double IBall.MaxSpeed => throw new NotImplementedException();

    public double MaxSpeed { get; set; }
    public Ball(Point3D position, double radius, Color colour)
    {
        Position = position;
        Radius = radius;
        Color = colour;
        MaxSpeed = 0.4;
        //_rollingDeceleration = 0.1;
    }

}
