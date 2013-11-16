using PlanMyWay.Lib.DataModel;
using PlanMyWay.Lib.Manager.EventArgs;
using System;
using System.Collections;
using System.Collections.Generic;

namespace PlanMyWay.Lib.Manager
{
    public interface IRoadMapManager
    {
        EventHandler<RoadMapReceivedEventArgs> RoadMapReceived { get; set; }
        EventHandler<AllRoadMapReceivedEventArgs> AllRoadMapsReceived { get; set; }
        bool IsCanceled { get; set; }
        void GetRoadMapAsync(DateTime date, ReferenceMeeting startPosition, ReferenceMeeting endPosition);
        void GetAllRoadMapsAsync(DateTime startDate, DateTime endDate, ReferenceMeeting startPosition, ReferenceMeeting endPosition);
        void GetAllRoadMapsAsync(Dictionary<DateTime, KeyValuePair<ReferenceMeeting, ReferenceMeeting>> items);
    }
}
