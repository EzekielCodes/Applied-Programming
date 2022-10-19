
using System.Windows;

namespace Globals.Entities;
public interface IBall
{
    Point Position { get; set; }
    double RollingResistanceCoeffienct { get; set; }

    Vector Speed { get; set; }
}