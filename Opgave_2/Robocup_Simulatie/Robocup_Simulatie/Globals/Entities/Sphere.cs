using Globals.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Globals.Entities;
public record class Sphere : IItem3D
{
    public Point3D Position { get; set; }
    public double Scale => Radius;
    public double YRotation => 0;
    public double Radius { get; init; }

    public Color Color { get; set; }



    public Sphere(Point3D position, double radius, Color colour)
    {
        Position = position;
        Radius = radius;
        Color = colour;
    }
}
