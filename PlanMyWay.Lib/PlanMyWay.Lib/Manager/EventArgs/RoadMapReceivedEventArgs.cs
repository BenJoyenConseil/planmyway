using PlanMyWay.Lib.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanMyWay.Lib.Manager.EventArgs
{
    public class RoadMapReceivedEventArgs : System.EventArgs
    {
        public RoadMap RoadMap { get; set; }
        public bool Error { get; set; }
        public string MessageError { get; set; }

        public RoadMapReceivedEventArgs(RoadMap roadMap)
        {
            RoadMap = roadMap;
            Error = false;
            MessageError = "";
        }
    }
}
