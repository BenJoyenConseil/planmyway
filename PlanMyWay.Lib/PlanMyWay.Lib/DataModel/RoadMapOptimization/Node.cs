using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanMyWay.Lib.DataModel.RoadMapOptimization
{
    public class Node
    {
        List<Node> _children = new List<Node>();
        Node _parent = null;
        Meeting _meetingRef;

        public Node Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        public Meeting MeetingRef
        {
            get { return _meetingRef; }
            set { _meetingRef = value; }
        }

        public List<Node> Children
        {
            get { return _children; }
            set { _children = value; }
        }
    }
}
