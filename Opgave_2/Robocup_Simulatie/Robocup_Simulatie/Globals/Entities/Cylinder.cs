using Globals.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Globals.Entities;
public record class Cylinder : IItem3D
{
    public Point3D Position { get; set; }

    public double Scale => 1;

    public double YRotation => 0;

    public double Radius { get; init; }

    public Vector3D Axis { get; init; }

    public Color Color { get; }

    public Cylinder(Point3D position, double radius, Vector3D axis, Color colours)
    {
        Position = position;
        Radius = radius;
        Axis = axis;
        Color = colours;
    }
}
