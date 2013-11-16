using PlanMyWay.Lib.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanMyWay.Lib.Manager.EventArgs
{
    public class AllRoadMapReceivedEventArgs : System.EventArgs
    {
        List<RoadMap> _roadMaps = new List<RoadMap>();

        public List<RoadMap> RoadMaps
        {
            get { return _roadMaps; }
            set { _roadMaps = value; }
        }
        public bool Error { get; set; }
        public string MessageError { get; set; }
    }
}
