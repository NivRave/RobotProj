using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ClassLibrary1.Coords
{
    public class Coordinate
    {
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }

        public Coordinate()
        {
            SetZero();
        }

        public Coordinate(Coordinate c)
        {
            this.x = c.x;
            this.y = c.y;
            this.z = c.z;
        }

        public void SetZero()
        {
            x= 0;
            y= 0;
            z= 0;
        }
    }
}
