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
    private Vector _speed;

    public Point3D Position { get; set; }

    public static double Radius = 0.1;

    public double RollingResistanceCoeffienct
    {
        get => _rollingDeceleration / _gravConstant;
        set => _rollingDeceleration = value;
        
    }

    public Vector Speed
    {
        get => _speed;
        set => _speed = value;
    }
    public Color Color { get; }

    public int Scale => 10;

    public Ball(Point3D position, double radius, Color colour)
    {
        Position = position;
        Radius = radius;
        Color = colour;
    }

}
