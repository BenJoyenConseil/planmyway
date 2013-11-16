using Microsoft.Phone.Controls.Maps.Platform;
using PlanMyWay.Lib.RouteService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanMyWay.Lib.DataModel
{
    public class RoadMap
    {
        MeetingCollection _meetings = new MeetingCollection();
        List<Trip> _trips = new List<Trip>();

        public MeetingCollection Meetings { get { return _meetings; } }
        public List<Trip> Trips { get { return _trips; } }
        
    }
}
