using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using Globals.Interfaces;

namespace Globals.Entities;
public class World : IWorld
{
    private const int _worldSize = 1000;
    private readonly PeriodicTimer _timer = new(TimeSpan.FromMilliseconds(10));


    public Point3D Origin => new();
    public (Point3D p1, Point3D p2) Bounds { get; private set; }

    public World()
    {
        Bounds = (new Point3D(-_worldSize / 2, -_worldSize / 2, -_worldSize / 2),
                      new Point3D(_worldSize / 2, _worldSize / 2, _worldSize / 2));
    }
}
