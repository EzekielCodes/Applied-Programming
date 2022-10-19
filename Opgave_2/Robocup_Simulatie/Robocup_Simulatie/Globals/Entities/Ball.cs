using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace Globals.Entities;
public record class Ball : IBall
{
    private const double _gravConstant = 9.81;
    private double _rollingDeceleration;
    private Vector _speed;

    public Point Position { get; set; }

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

}
