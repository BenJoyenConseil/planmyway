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
    public class RoadMapManagerTest
    {
        
        public static void GetRoadMapAsyncTEST()
        {
            IRoadMapManager manager = new RoadMapManager();

            manager.RoadMapReceived += (o, e) =>
            {
                if (e.Error)
                {
                    Debug.WriteLine("Aucun rendez-vous n'existe pour le jour sélectionné");
                    return;
                }
                Debug.WriteLine("YopYop Bling on a récupéré la roadmap!!");
            };
            ReferenceMeeting start = new ReferenceMeeting(new DateTime(2013, 1, 2, 8, 30, 0), new Location() { Latitude = 48.85693,
                                                                     Longitude = 2.3412});
            ReferenceMeeting end = new ReferenceMeeting(new DateTime(2013, 1, 2, 18, 30, 0), new Location() { Latitude = 48.85693,
                                                                     Longitude = 2.3412});

            manager.GetRoadMapAsync(new DateTime(2013, 1, 2), start, end);
        }

        public static void GetAllRoadMapsAsyncTEST()
        {
            IRoadMapManager manager = new RoadMapManager();

            manager.AllRoadMapsReceived += (o, e) =>
            {
                if (e.RoadMaps.Count  == 0)
                {
                    Debug.WriteLine("Aucun rendez-vous n'existe pour les jours sélectionnés");
                    return;
                }
                Debug.WriteLine("YopYop Bling on a récupéré les roadmaps!!");
            };
            ReferenceMeeting start = new ReferenceMeeting(new DateTime(2013, 1, 2, 8, 30, 0), new Location()
            {
                Latitude = 48.85693,
                Longitude = 2.3412
            }) { City = "Paris", Subject = "Start" };
            ReferenceMeeting end = start;
            end.Subject = "End";

            manager.GetAllRoadMapsAsync(new DateTime(2013, 1, 2), new DateTime(2013, 1, 4), start, end);
        }
    }
}
