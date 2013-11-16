using PlanMyWay.Lib.DataModel;
using System;
using Microsoft.Phone.UserData;
using PlanMyWay.Lib.Manager.EventArgs;

namespace PlanMyWay.Lib.Manager
{
    public interface IMeetingManager
    {
        EventHandler<LocationCheckedEventArgs> LocationChecked { get; set; }
        EventHandler<SearchStringCompletedEventArgs> SearchStringCompleted { get; set; }
        EventHandler<AllMeetingsReceivedEventArgs> AllMeetingsRetreived { get; set; }
        bool IsCanceled { get; set; }
        void CheckLocationAsync(Appointment appointment);
        void SearchStringAsync(string address, object userData = null);
        void GetAllMeetingsAsync(DateTime date);
    }
}
