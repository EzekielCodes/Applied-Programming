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
    private Ball? _ball;

    private Parallelogram? _rectangle;
    private Beam? _goalpostOne;
    private Beam? _goalpostTwo;


    public Point3D Origin => new();
    public (Point3D p1, Point3D p2) Bounds { get; private set; }

    public List<IItem3D> Items { get; } = new();

    public World()
    {
        Bounds = (new Point3D(-_worldSize / 2, -_worldSize / 2, -_worldSize / 2),
                      new Point3D(_worldSize / 2, _worldSize / 2, _worldSize / 2));

        CreateItems();
    }

    private void CreateItems()
    {
        /*Items.Add(new Cube(position: new(100, 100, 0), size: 100));
        Items.Add(new Sphere(position: new(250, 100, 0), radius: 50));
        Items.Add(new Beam(position: new Point3D(350, 75, 50), xSize: 50, ySize: 80, zSize: 200));
        Items.Add(new Cylinder(position: new(500, 50, 50), radius: 30, axis: new(0, 150, -100)));
        Items.Add(new Cone(position: new(650, 50, -50), radius: 40, axis: new(0, 150, 100)));*/
       
        _rectangle = new Parallelogram(origin: new(-450, 0, -300), side1: new(900, 0, 0), new(0, 0, 600));
        Items.Add(_rectangle);
        
        _ball = new Ball(center: new Point3D(-250, 100, 0), radius: 50, normal: new(0, 1, 1));
        Items.Add(_ball);
    }
}
