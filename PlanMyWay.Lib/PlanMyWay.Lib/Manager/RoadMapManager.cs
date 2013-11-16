using PlanMyWay.Lib.DataModel;
using PlanMyWay.Lib.Manager.EventArgs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PlanMyWay.Lib.Manager
{
    public class RoadMapManager : PlanMyWay.Lib.Manager.IRoadMapManager
    {
        public EventHandler<RoadMapReceivedEventArgs> RoadMapReceived { get; set; }
        public EventHandler<AllRoadMapReceivedEventArgs> AllRoadMapsReceived { get; set; }
        public bool IsCanceled { get; set; }

        public void GetRoadMapAsync(DateTime date, ReferenceMeeting startPosition, ReferenceMeeting endPosition)
        {
            IMeetingManager meetingManager;
            ITripManager tripManager;
            RoadMap roadMap = new RoadMap();
            meetingManager = new MeetingManager();
            tripManager = new TripManager();
            roadMap.Date = date;

            //Lorsque on reçoit tous les RDV géolocalisés
            meetingManager.AllMeetingsRetreived += (o, eventMeeting) =>
            {
                if (eventMeeting.Error)
                {
                    RoadMapReceived(this, new RoadMapReceivedEventArgs(null) { Error = true, MessageError = eventMeeting.MessageError});
                    meetingManager.AllMeetingsRetreived = null;
                    return;
                }

                startPosition.DateTime = eventMeeting.Meetings.FirstOrDefault().DateTime.Date;
                endPosition.DateTime = eventMeeting.Meetings.FirstOrDefault().DateTime.Date.AddDays(1);
                //Position de départ
                roadMap.Meetings.Add(startPosition);
                //Enregistrement des rendez-vous dans la roadmap
                roadMap.Meetings.AddRange(eventMeeting.Meetings);
                //Position d'arrivée
                roadMap.Meetings.Add(endPosition);

                //Lorsque on reçoit le chemin complet de la journée
                tripManager.TripReceived += (o2, eventTrip) =>
                {
                    if (eventTrip.Error)
                    {
                        RoadMapReceived(this, new RoadMapReceivedEventArgs(null) { Error = true, MessageError = "L'adresse de départ ou d'arrivée décrite dans les paramètres de l'application est incorrecte!" });
                        tripManager.TripReceived = null;
                        return;
                    }
                    roadMap.RouteResult = eventTrip.RouteResult;
                    roadMap.Trips.AddRange(eventTrip.Trips);
                    RoadMapReceived(this, new RoadMapReceivedEventArgs(roadMap)); 
                    tripManager.TripReceived = null;
                };
                if (roadMap.ValidMeetings.Count <= 0)
                {
                    RoadMapReceived(this, new RoadMapReceivedEventArgs(null) { Error = true });
                    return;
                }
                //Calcule du chemin complet de façon asynchrone
                tripManager.GetTripAsync(roadMap.ValidMeetings);
            };
            //Récupération et vérification des rendez-vous
            meetingManager.GetAllMeetingsAsync(date);
        }


        public void GetAllRoadMapsAsync(DateTime startDate, DateTime endDate, ReferenceMeeting startPosition, ReferenceMeeting endPosition)
        {
            if(startDate > endDate)
            {
                Debug.WriteLine("startDate doit être inférieure à endDate");
                AllRoadMapsReceived(this, new AllRoadMapReceivedEventArgs() { Error = true, MessageError = "La date de départ doit être strictement inférieure à la date d'arrivée" });
                return;
            }
            if (startDate == endDate)
            {
                endDate.Date.AddDays(1);
            }
            string messageError = string.Empty;

            List<DateTime> days = new List<DateTime>();
            List<RoadMap> roadMaps = new List<RoadMap>();
            int countSended = 0;
            int countReceived = 0;
            DateTime i = startDate;
            while (i <= endDate)
            {
                days.Add(i);
                i = i.AddDays(1);
            }

            RoadMapReceived += (o, e) =>
            {
                if (e.Error)
                {
                    messageError = e.MessageError;
                }
                countReceived++;
                if (!e.Error)
                    roadMaps.Add(e.RoadMap);
                if (countReceived >= days.Count)
                {
                    var arg = new AllRoadMapReceivedEventArgs();
                    if (messageError != string.Empty)
                    {
                        arg.MessageError = messageError;
                    }
                    if (roadMaps.Count <= 0)
                        arg.Error = true;

                    arg.RoadMaps.AddRange(roadMaps.OrderBy(obj=>obj.Date));
                    AllRoadMapsReceived(this, arg);
                    RoadMapReceived = null;
                }
            };

            foreach (var item in days)
            {
                GetRoadMapAsync(item, startPosition, endPosition);
                countSended++;
            }
        }



        public void GetAllRoadMapsAsync(Dictionary<DateTime, KeyValuePair<ReferenceMeeting, ReferenceMeeting>> items)
        {
            string messageError = string.Empty;

            List<RoadMap> roadMaps = new List<RoadMap>();
            int countSended = 0;
            int countReceived = 0;

            RoadMapReceived += (o, e) =>
            {
                if (e.Error)
                {
                    messageError = e.MessageError;
                }
                countReceived++;
                if (!e.Error)
                    roadMaps.Add(e.RoadMap);
                if (countReceived >= items.Count)
                {
                    var arg = new AllRoadMapReceivedEventArgs();
                    if (messageError != string.Empty)
                    {
                        arg.Error = true;
                        arg.MessageError = messageError;
                    }
                    arg.RoadMaps.AddRange(roadMaps.OrderBy(obj => obj.Date));
                    AllRoadMapsReceived(this, arg);
                    RoadMapReceived = null;
                }
            };

            for (int j = 0; j < items.Count; j++)
            {
                GetRoadMapAsync(items.Keys.ElementAt(j), items.Values.ElementAt(j).Key, items.Values.ElementAt(j).Value);
                countSended++;
            }
        }
    }
}
