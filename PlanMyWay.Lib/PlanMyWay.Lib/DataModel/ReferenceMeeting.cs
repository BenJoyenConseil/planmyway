using Microsoft.Phone.Controls.Maps.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanMyWay.Lib.DataModel
{
    public class ReferenceMeeting : Meeting
    {
        /// <summary>
        /// Durée en minutes
        /// </summary>
        public double Duration { get { return 0; } set { } }
        public bool IsLocationFail { get { return false; } set { } }

        public ReferenceMeeting(DateTime time, Location location)
        {
            Location = location;
            DateTime = time;
        }

        public ReferenceMeeting() : base()
        {

        }
    }
}
