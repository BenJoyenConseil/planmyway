using PlanMyWay.Lib.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanMyWay.Lib.Manager.EventArgs
{
    public class AllMeetingsReceivedEventArgs : System.EventArgs
    {
        public AllMeetingsReceivedEventArgs()
        {
            Meetings = new List<Meeting>();
            Error = false;
            MessageError = string.Empty;
        }

        public IEnumerable<Meeting> Meetings { get; set; }
        public bool Error { get; set; }
        public String MessageError { get; set; }
    }
}
