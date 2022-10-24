﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    private IWorld? _game;

    private Sphere? _ball;
    private readonly PeriodicTimer _timer = new(TimeSpan.FromMilliseconds(10));
    private bool _playing = false;
    private int _aantalspelers = 1;
    private readonly Random _random = new();
    private int[] _arrayCheck;
    public Point3D Origin => new();
    public (Point3D p1, Point3D p2) Bounds { get; private set; }

    public List<IItem3D> Items { get; } = new();

    public List<Cylinder> TeamBlue { get; } = new();
    public List<Cylinder> TeamRed { get; } = new();

    public int FieldLength => 900;
    public int FieldWidth => 600;

    public int GoalWidth => 150;


    public int AantalSpelers
    {
        get => _aantalspelers;
        set
        {
            if ( value > 0)
            {
                _aantalspelers = value;
            }
        }
    }
    public World()
    {
        Bounds = (new Point3D(-_worldSize / 2, -_worldSize / 2, -_worldSize / 2),
                      new Point3D(_worldSize / 2, _worldSize / 2, _worldSize / 2));

       /* Task.Run(async () =>
        {
            while (true)
            {
                await _timer.WaitForNextTickAsync();
                //MovePlayers();
            }
        });*/
        //CreateItems();
    }

    public void CreateItems()
    {
        CreatePlayers(_aantalspelers);
        _ball = new Sphere(position: new(0, 10, 0), radius: 10, Colors.Orange);
        Items.Add(_ball);
    }

    private void CreatePlayers(int aantal)
    {
        _arrayCheck = new int[aantal];
        for (int i = 0; i < aantal; i++)
        {
            TeamRed.Add(new Cylinder(position: new(RandomXPosition(FieldLength/2,i), 0, RandomZPosition(FieldWidth / 2,i)), radius: 20, axis: new(0, 20, 0), Colors.Red));
            
        }
        _arrayCheck = new int[aantal];
        for (int i = 0; i < aantal; i++)
        {
            TeamBlue.Add(new Cylinder(position: new(-RandomXPosition(FieldLength / 2,i), 0, RandomZPosition(FieldWidth / 2,i)), radius: 20, axis: new(0, 20, 0), Colors.Blue));
        }
    }

    private int RandomXPosition(int x, int i)
    {
        int Randomint = _random.Next(100,x-100);
        if (_arrayCheck.Contains(Randomint))
        {
            return RandomXPosition(x, i);
        }
        _arrayCheck[i] = Randomint;
        return Randomint;
    }

    private int RandomZPosition(int z,int i)
    {
        int Randomint = _random.Next(-(FieldWidth/2)+50, z - 50);
        if (_arrayCheck.Contains(Randomint))
        {
            return RandomZPosition(z, i);
        }
        _arrayCheck[i] = Randomint;
        return Randomint;
    }

    public void MovePlayers()
    {
        for (int i = 0; i < TeamBlue.Count; i++)
        {
            TeamRed[i].Position = new Point3D(TeamRed[i].Position.X - 0.5, 0, TeamRed[i].Position.Z - 0.5);
            TeamBlue[i].Position = new Point3D(TeamBlue[i].Position.X + 0.5, 0, TeamBlue[i].Position.Z + 0.5);

        }
    }

    public void Stop()
    {
        _game?.Stop();
        _playing = false;
    }

    public void Start()
    {
        if (_game == null) return;

        _game.Start();

        _playing = true;
    }
}
