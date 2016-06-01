using LiftManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LiftManager.ViewModels
{
    /// <summary>
    /// Viewmodel that manipulates the Floor and Lift model data and fires events a View can listen to.
    /// </summary>
    public class LiftSimulation
    {
        public event EventHandler<FloorEventArgs> OnFloorUpdate;
        public event EventHandler<LiftEventArgs> OnLiftUpdate;

        // (Mock) Data access

        public List<Floor> Floors
        {
            get { return DataManager.Instance.floors; }
        }

        public List<Lift> Lifts
        {
            get { return DataManager.Instance.lifts; }
        }

        public Floor GetFloor(int floorNumber)
        {
            foreach (Floor floor in Floors)
            {
                if (floor.floorNumber == floorNumber)
                    return floor;
            }

            return null;
        }

        // Model Manipulation

        public void CreateTravelParty (PartyEventArgs args)
        {
            if (args.destination == args.floor.floorNumber || args.passengers == 0) return;

            args.floor.travellers = args.passengers;
            args.floor.travelDestination = args.destination;
            args.floor.occupants -= args.passengers;

            OnFloorUpdate(this, new FloorEventArgs(args.floor));
        }

        void BoardLift (Floor floor, Lift lift)
        {

            lift.destinationLevel = floor.travelDestination;

            if (floor.travellers > lift.AvailableCapacity)
            {
                // Split the party (!)
                floor.travellers -= lift.AvailableCapacity;
                lift.passengers += lift.AvailableCapacity;                
            } else
            {
                lift.passengers += floor.travellers;
                floor.travellers = 0;
                floor.travelDestination = 0;
            }

            OnFloorUpdate(this, new FloorEventArgs(floor));
            OnLiftUpdate(this, new LiftEventArgs(lift));
        }

        void DisembarkFromLift (Lift lift, Floor floor)
        {
            floor.occupants += lift.passengers;

            lift.passengers = 0;
            lift.destinationLevel = 0;

            OnFloorUpdate(this, new FloorEventArgs(floor));
            OnLiftUpdate(this, new LiftEventArgs(lift));
        }

        // Simulation

        public bool StepSimulation()
        {
            ProcessLiftRequests();
            ProcessLiftMovement();

            return true;
        }

        void ProcessLiftRequests ()
        {
            foreach (Floor floor in Floors)
            {
                if (floor.travellers == 0) continue;

                if (floor.travelDestination > 0)
                {
                    if (LiftDispatchedToFloor(floor)) continue;

                    Lift nearestLift = LiftNearestToFloor(floor);

                    if (nearestLift != null)
                    {
                        nearestLift.destinationLevel = floor.floorNumber;
                    }
                }
            }
        }

        void ProcessLiftMovement ()
        {
            foreach (Lift lift in Lifts)
            {
                if (lift.destinationLevel == 0) continue;

                if (lift.currentLevel == lift.destinationLevel)
                {
                    Floor currentFloor = GetFloor(lift.currentLevel);

                    if (currentFloor != null)
                    {
                        if (lift.passengers > 0)
                        {
                            DisembarkFromLift(lift, currentFloor);
                        }
                        else if (currentFloor.travellers > 0)
                        {
                            BoardLift(currentFloor, lift);
                        }
                        else if (lift.passengers == 0 && currentFloor.travellers == 0)
                        {
                            lift.destinationLevel = 0;
                        } 
                    }

                } else
                {
                    lift.currentLevel = (lift.destinationLevel > lift.currentLevel) ? lift.currentLevel + 1 : lift.currentLevel - 1;
                }

                OnLiftUpdate(this, new LiftEventArgs(lift));
            }
        }

        int CalculateLiftDistanceFromFloor (Lift lift, Floor floor)
        {
            return Math.Abs(floor.floorNumber - lift.currentLevel);
        }

        Lift LiftNearestToFloor(Floor floor)
        {
            Lift nearestLift = null;

            foreach (Lift lift in Lifts)
            {
                if (lift.Available)
                {
                    if (nearestLift == null || CalculateLiftDistanceFromFloor(lift, floor) < CalculateLiftDistanceFromFloor(nearestLift, floor))
                        nearestLift = lift;
                }                
            }

            return nearestLift;
        }

        bool LiftDispatchedToFloor(Floor floor)
        {
            foreach (Lift lift in Lifts)
            {
                if (lift.destinationLevel == floor.floorNumber && lift.passengers == 0) return true;
            }

            return false;
        }
    }
}
