using Microsoft.Phone.UserData;
using PlanMyWay.Lib.Manager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PlanMyWay.Lib.Test.Tests
{
    public class MeetingManagerTest
    {

        public static void CheckLocationAsyncTEST()
        {
            IMeetingManager manager = new MeetingManager();
            manager.LocationChecked += (o, e) =>
            {
                Debug.WriteLine(e.Meeting.IsLocationFail);
            };
            manager.SearchStringAsync("4 rue Sakharov 76130 Mont Saint Aignan France");
        }

        public static void GetAllMeetingsAsyncTEST()
        {
            IMeetingManager manager = new MeetingManager();

            manager.AllMeetingsRetreived += (o, e) =>
            {
                Debug.WriteLine("Fin de la requete");

                foreach (var m in e.Meetings)
                {
                    Debug.WriteLine(m.DateTime);
                }
            };
            manager.GetAllMeetingsAsync(new DateTime(2013,1,2));
        }
    }
}
