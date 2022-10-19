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

    private Sphere? _ball;

    public Point3D Origin => new();
    public (Point3D p1, Point3D p2) Bounds { get; private set; }

    public List<IItem3D> Items { get; } = new();

    public List<Cylinder> TeamBlue { get; } = new();
    public List<Cylinder> TeamRed { get; } = new();

    public int FieldLength => 900;
    public int FieldWidth => 600;

    public World()
    {
        Bounds = (new Point3D(-_worldSize / 2, -_worldSize / 2, -_worldSize / 2),
                      new Point3D(_worldSize / 2, _worldSize / 2, _worldSize / 2));
        CreateItems();
    }

    private void CreateItems()
    {
        CreatePlayers(2);
        _ball = new Sphere(position: new(0, 10, 0), radius: 10, Colors.Orange);
        Items.Add(_ball);
    }

    private void CreatePlayers(int aantal)
    {
        int teller = 20;
        for (int i = 0; i < aantal; i++)
        {
            TeamRed.Add(new Cylinder(position: new(300, 0, -100 + teller), radius: 20, axis: new(0, 20, 0), Colors.Red));
            teller += 100;
            
        }

        teller = 20;
        for (int i = 0; i < aantal; i++)
        {
            TeamBlue.Add(new Cylinder(position: new(-300, 0, -100 + teller), radius: 20, axis: new(0, 20, 0), Colors.Blue));
            teller += 100;
            
        }
    }
}
