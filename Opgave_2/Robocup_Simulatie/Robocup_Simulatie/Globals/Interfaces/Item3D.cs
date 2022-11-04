using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows;

namespace Globals.Interfaces;
public interface IItem3D
{
    public Point3D Position { get; }

    public Vector3D Velocity { get; set; }

    public Vector3D Acceleration { get; set; }

    public double Scale { get; }


    public Color Color { get; }
    double Speed { get; set; }

    Task Updatepostion(Point3D ball, TimeSpan interval);
}
