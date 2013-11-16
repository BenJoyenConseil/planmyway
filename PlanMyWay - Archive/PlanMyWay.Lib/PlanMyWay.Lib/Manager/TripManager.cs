using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Controls.Maps.Platform;
using PlanMyWay.Lib.DataModel;
using PlanMyWay.Lib.Manager.EventArgs;
using PlanMyWay.Lib.RouteService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Device;

namespace PlanMyWay.Lib.Manager
{
    public class TripManager : ITripManager
    {
        public EventHandler<TripReceivedEventArgs> TripReceived { get; set; }

        public bool IsCanceled { get; set; }

        public TripManager()
        {
            IsCanceled = false;
        }

        public void GetTripAsync(List<Meeting> meetings)
        {
            if (meetings.Count()<=1)
                return;

            //Initialisation
            var request = new RouteRequest();
            request.Credentials = new Credentials();
            request.Credentials.ApplicationId = BingMapCredential.CREDENTIAL;
            request.Waypoints = new ObservableCollection<Waypoint>();
            foreach (Meeting meeting in meetings)
            {
                if (IsCanceled)
                    return;

                if (meeting.IsLocationFail)
                    continue;
                request.Waypoints.Add(new Waypoint()
                {
                    Location = new Location()
                    {
                        Latitude = meeting.Location.Latitude,
                        Longitude = meeting.Location.Longitude
                    }
                });
            }
            request.Options = new RouteOptions();
            request.Options.RoutePathType = RoutePathType.Points;
            request.UserProfile = new UserProfile();
            request.UserProfile.DistanceUnit = DistanceUnit.Kilometer;
            var service = new RouteServiceClient("BasicHttpBinding_IRouteService");

            //Réponse
            service.CalculateRouteCompleted += (o, e) =>
            {
                if (IsCanceled)
                    return;

                if (e.Error != null || e.Result == null)
                {
                    try
                    {
                        Debug.WriteLine(e.Error.Message);
                    }
                    catch (Exception exception)
                    {
                    }
                    TripReceived(this, new TripReceivedEventArgs(){Error = true});
                    return;
                }
                List<Trip> trips = new List<Trip>();
                var legs = e.Result.Result.Legs;
                var countRoutePath = e.Result.Result.RoutePath.Points.Count;
                int lastIndex = 0;
                var count = legs.Count;

                for (int i = 0; i < count; i++)
                {
                    if (IsCanceled)
                        return;

                    var leg = legs[i];
                    Trip trip = new Trip();
                    meetings[i].Location = leg.ActualStart;
                    meetings[i + 1].Location = leg.ActualEnd;
                    trip.Start = meetings[i];
                    trip.End = meetings[i + 1];
                    trip.Duration = leg.Summary.TimeInSeconds / 60;
                    trip.Distance = leg.Summary.Distance;

                    while (lastIndex < countRoutePath)
                    {
                        if (IsCanceled)
                            return;
                        Location point = e.Result.Result.RoutePath.Points[lastIndex];
                        
                        if (Math.Round(trip.End.Location.Latitude, 5) == Math.Round(point.Latitude, 5) &&
                            Math.Round(trip.End.Location.Longitude, 5) == Math.Round(point.Longitude, 5))
                        {
                            break;
                        }
                        trip.RoutePath.Add(point);
                        lastIndex++;
                    }
                    trips.Add(trip);
                }

                TripReceived(this, new TripReceivedEventArgs(trips, e.Result.Result) );
            };

            //Appel
            service.CalculateRouteAsync(request);

            if (IsCanceled)
                return;
        }

    }
}
