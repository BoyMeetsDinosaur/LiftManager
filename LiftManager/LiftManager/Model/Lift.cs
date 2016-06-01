using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LiftManager.Model
{
    public class Lift
    {
        public int destinationLevel;
        public int currentLevel;
        public int liftNumber; // non-zero based
        public int passengers;

        public Lift (int liftNumber)
        {
            this.liftNumber = liftNumber;
        }

        public String Name
        {
            get
            {
                char letter = (char)('a' + (liftNumber-1));
                return String.Format("Lift {0}", letter.ToString().ToUpper());
            }
        }

        public bool Available
        {
            get
            {
                return (destinationLevel == 0 && passengers == 0);
            }
        }

        public int AvailableCapacity
        {
            get
            {
                return (20 - passengers);
            }
        }

        public Status CurrentStatus
        {
            get
            {
                if (currentLevel == destinationLevel || destinationLevel == 0)
                    return Status.Idle;

                return (currentLevel < destinationLevel ? Status.Ascending : Status.Descending);
            }
        }

        public enum Status
        {
            Ascending,
            Descending,
            Idle
        }
    }
}
