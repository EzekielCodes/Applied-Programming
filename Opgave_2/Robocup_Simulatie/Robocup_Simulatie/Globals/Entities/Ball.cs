using Globals.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Globals.Entities;
public record class Ball:IItem3D
{
    public Point3D Position { get; set; }

    public double Scale { get; set; }

    public double YRotation { get; set; } = 0;

    public double Radius { get; }

    public Vector3D Normal { get; }

    public Ball(Point3D center, double radius, Vector3D normal)
    {
        Position = center;
        Normal = normal;
        Scale = Radius = radius;
    }
}
