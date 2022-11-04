
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Globals.Interfaces;
public interface IBall
{
    Point3D Position { get; set; }
    double RollingResistanceCoeffienct { get; set; }

    Double Speed { get; set; }
    public Vector3D Velocity { get; set; }

    public Vector3D Acceleration { get; set; }

    public int Scale { get; }

    public Color Color { get; }
    double MaxSpeed { get; }
}