
using System.Windows;
using System.Windows.Media.Media3D;

namespace Globals.Interfaces;
public interface IBall
{
    Point3D Position { get; set; }
    double RollingResistanceCoeffienct { get; set; }

    Vector Speed { get; set; }
}