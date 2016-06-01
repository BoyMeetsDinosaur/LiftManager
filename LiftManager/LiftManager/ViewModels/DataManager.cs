using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiftManager.Model;

namespace LiftManager.ViewModels
{
    class DataManager
    {
        public List<Floor> floors = new List<Floor>();
        public List<Lift> lifts = new List<Lift>();

        private static readonly System.Random random = new System.Random();

        public DataManager ()
        {
            CreateMockData(10, 4);
        }

        public void CreateMockData (int numberOfFloors, int numberOfLifts)
        {            
            floors.Clear();
            lifts.Clear();
            
            for (var i = 1; i <= numberOfFloors; i++)
            {
                Floor floor = new Floor(i);
                floor.occupants = RandomNumber(10, 25);
                floors.Add(floor);
            }

            for (var i = 1; i <= numberOfLifts; i++)
            {
                Lift lift = new Lift(i);
                lift.currentLevel = RandomNumber(1, numberOfFloors);
                lift.destinationLevel = 0;
                lifts.Add(lift);
            }
        }

        public static int RandomNumber(int min, int max)
        {
            return random.Next(min, max + 1); // inclusive min, inclusive max
        }

        // Primitive Singleton behaviour

        private static DataManager _instance;

        public static DataManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DataManager();

                return _instance;
            }
        }
    }
}
