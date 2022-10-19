﻿using Globals.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Globals.Entities;
public record class Parallelogram : IItem3D
{
    public Point3D Position { get; set; }

    public double Scale { get; set; }

    public double YRotation { get; set; } = 0;

    public Vector3D Side1 { get; }
    public Vector3D Side2 { get; }

    public Parallelogram(Point3D origin, Vector3D side1, Vector3D side2, double scale = 1)
    {
        Position = origin;
        Side1 = side1;
        Side2 = side2;
        Scale = scale;
    }
}
