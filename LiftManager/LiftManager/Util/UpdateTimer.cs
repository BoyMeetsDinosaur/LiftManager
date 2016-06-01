using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LiftManager.Util
{
    /// <summary>
    /// Simple Device.Timer utility to control timer instances.
    /// </summary>
    class UpdateTimer
    {
        public bool disposed = false;
        EventHandler<UpdateTimer> UpdateEvent;

        public UpdateTimer (int millisecondsBetweenUpdates, EventHandler<UpdateTimer> UpdateListener)
        {
            UpdateEvent = UpdateListener;
            StartUpdateLoop(millisecondsBetweenUpdates);
        }

        void StartUpdateLoop(int millisecondsBetweenUpdates)
        {
            Device.StartTimer(new TimeSpan(TimeSpan.TicksPerMillisecond * millisecondsBetweenUpdates), OnUpdate);
        }

        bool OnUpdate()
        {
            if (disposed)
            {
                UpdateEvent = null;
                return false;
            }

            UpdateEvent(this, this);

            return true;
        }
    }
}
