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
    private Vector3D _speed;

    public Vector3D Versnelling { get; private set; }

    public double Radius { get; init; }

    public Vector3D Axis { get; init; }

    public Color Color { get; }

    public Vector3D Speed
    {
        get => _speed;
        set => _speed = value;
    }

    public Players(Point3D position, double radius, Vector3D axis, Color colours)
    {
        Position = position;
        Radius = radius;
        Axis = axis;
        Color = colours;
    }

    public void Updatepostion(TimeSpan interval)
    {
        Position += Speed * interval.TotalSeconds;
        Speed += Versnelling * interval.TotalSeconds;




    }
}
