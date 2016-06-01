using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LiftManager.Model
{
    public class Floor
    {
        public int occupants { get; set; }
        public int floorNumber { get; set; } // non-zero based

        public int travellers { get; set; }
        public int travelDestination { get; set; }
        
        public Floor (int floorNumber)
        {
            this.floorNumber = floorNumber;
        }
    }
}
