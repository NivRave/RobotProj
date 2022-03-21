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

        public void SetZero()
        {
            x= 0;
            y= 0;
            z= 0;
        }
    }
}
