using LiftManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftManager.ViewModels
{
    public class FloorEventArgs : EventArgs
    {
        public Floor floor { get; set; }
        public FloorEventArgs (Floor floor)
        {
            this.floor = floor;
        }
    }

    public class LiftEventArgs : EventArgs
    {
        public Lift lift { get; set; }
        public LiftEventArgs (Lift lift)
        {
            this.lift = lift;
        }
    }

    public class PartyEventArgs : EventArgs
    {
        public Floor floor { get; set; }
        public int passengers { get; set; }
        public int destination { get; set; }

        public PartyEventArgs (Floor floor, int passengers, int destination)
        {
            this.floor = floor;
            this.passengers = passengers;
            this.destination = destination;
        }
    }
}
