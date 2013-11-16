using Microsoft.Phone.Controls.Maps.Platform;
using PlanMyWay.Lib.DataModel;
using PlanMyWay.Lib.Manager.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanMyWay.Lib.Manager
{
    public interface ITripManager
    {
        EventHandler<TripReceivedEventArgs> TripReceived { get; set; }
        bool IsCanceled { get; set; }
        void GetTripAsync(List<Meeting> meetings);
    }
}
