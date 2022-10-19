using Globals.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Globals.Entities;
public record class Beam : IItem3D
{
    public Point3D Position { get; set; }
    public double Scale { get; set; }
    public double XSize { get; }
    public double YSize { get; }
    public double ZSize { get; }
    public double YRotation => 0;

    public Color Color { get; set; }

    public Beam(Point3D position, double xSize, double ySize, double zSize , Color color , double scale = 1)
    {
        Position = position;
        XSize = xSize;
        YSize = ySize;
        ZSize = zSize;
        Scale = scale;
        Color = color;
    }
}
