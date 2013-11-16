using Microsoft.Phone.Controls.Maps.Platform;
using PlanMyWay.Lib.DataModel;
using PlanMyWay.Lib.Manager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PlanMyWay.Lib.Test.Tests
{
    public class TripManagerTest
    {
        public static void GetTripAsyncTEST()
        {            
            Meeting startMeeting = new Meeting()
            {
                Location = new Location()
                {
                    Latitude = 49.47518,
                    Longitude = 1.091378
                }
            };
            Meeting endMeeting = new Meeting()
            {
                Location = new Location()
                {
                    Latitude = 49.4401397705078,
                    Longitude = 1.08940994739532
                }
            };

            ITripManager manager = new TripManager();

            manager.TripReceived += (o, e) =>
            {
                Debug.WriteLine("Coucou lol");
            };
            //manager.GetTripAsync(startMeeting, endMeeting);
        }


        public static void GetAllTripsAsyncTEST()
        {
            IMeetingManager meetingManager = new MeetingManager();
            //meetingManager.AllMeetingsRetreived += (o, e) =>
            //{
            //    ITripManager manager = new TripManager();
            //    manager.AllTripsReceived += (obj, eventTrips) =>
            //    {
                    
            //    };
            //    manager.GetAllTripsAsync(e.Results);
            //};
            //meetingManager.GetAllMeetingsAsync(DateTime.Today.AddDays(-1));
        }
    }
}
