using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Globals.Interfaces;
public interface IItem3D
{
        public Point3D Position { get; }

        public double Scale { get; }

        public double YRotation { get; }

        public Color Color { get; }
}
