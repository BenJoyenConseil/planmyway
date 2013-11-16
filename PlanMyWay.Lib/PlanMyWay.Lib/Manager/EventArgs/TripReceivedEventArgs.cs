using PlanMyWay.Lib.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanMyWay.Lib.Manager.EventArgs
{
    public class TripReceivedEventArgs : System.EventArgs
    {
        public List<Trip> Trips { get; set; }
        public bool Error { get; set; }
        public RouteService.RouteResult RouteResult { get; set; }

        public TripReceivedEventArgs()
        {
            Trips = new List<Trip>();
            Error = false;
        }

        public TripReceivedEventArgs(List<Trip> trips)
        {
            Trips = trips;
            Error = false;
        }

        public TripReceivedEventArgs(List<Trip> trips, RouteService.RouteResult routeResult)
        {
            Trips = trips;
            Error = false;
            RouteResult = routeResult;
        }
    }
}
