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
    

    private Parallelogram? _terrein;
    private Parallelogram? _rand;
    private Beam? _goalpostOne;
    private Beam? _goalpostTwo;
    private Sphere? _ball;


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
        int teller = 20;
        for (int i = 0; i < 3; i++)
        {
            Beam player;
            player = new Beam(position: new Point3D(300, 0, -70 + teller), xSize: 20, ySize: 20, zSize: 50);
            teller = teller + 100;
            Items.Add(player);
        }

        teller = 20;
        for (int i = 0; i < 3; i++)
        {
            Beam player;
            player = new Beam(position: new Point3D(-300, 0, -70 + teller), xSize: 20, ySize: 20, zSize: 50);
            teller = teller + 100;
            Items.Add(player);
        }
        _goalpostOne = new Beam(position: new Point3D(400, 0, 70), xSize: 50, ySize: 80, zSize: 150);
        _goalpostTwo = new Beam(position: new Point3D(-450, 0, 70), xSize: 50, ySize: 80, zSize: 150);
       
        Items.Add(_goalpostTwo);
        Items.Add(_goalpostOne);
        _terrein = new Parallelogram(origin: new(-450, 0, -300), side1: new(900, 0, 0), new(0, 0, 600));
        Items.Add(_terrein);
        /*_rand = new Parallelogram(origin: new(-450, 10, -300), side1: new(920, 0, 0), new(0, 0, 620));
        Items.Add(_rand);*/


        _ball = new Sphere(position: new(250, 10, 0), radius: 10);
        Items.Add(_ball);
    }
}
