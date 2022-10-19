using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
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
        CreatePlayers();
        _goalpostOne = new Beam(position: new Point3D(400, 0, 70), xSize: 50, ySize: 80, zSize: 150, Colors.Blue);
        _goalpostTwo = new Beam(position: new Point3D(-450, 0, 70), xSize: 50, ySize: 80, zSize: 150, Colors.Brown);
       
        Items.Add(_goalpostTwo);
        Items.Add(_goalpostOne);
        _terrein = new Parallelogram(origin: new(-450, 0, -300), side1: new(900, 0, 0), new(0, 0, 600), Colors.Green);
        Items.Add(_terrein);

        _ball = new Sphere(position: new(50, 10, 0), radius: 10, Colors.Orange);
        Items.Add(_ball);
    }

    private void CreatePlayers()
    {
        int teller = 20;
        for (int i = 0; i < 3; i++)
        {
            Items.Add(new Cylinder(position: new(300, 0, -100 + teller), radius: 20, axis: new(0, 20, 0), Colors.Red));
            teller = teller + 100;
            
        }

        teller = 20;
        for (int i = 0; i < 3; i++)
        {
            Items.Add(new Cylinder(position: new(-300, 0, -100 + teller), radius: 20, axis: new(0, 20, 0), Colors.Blue));
            teller = teller + 100;
            
        }
    }
}
