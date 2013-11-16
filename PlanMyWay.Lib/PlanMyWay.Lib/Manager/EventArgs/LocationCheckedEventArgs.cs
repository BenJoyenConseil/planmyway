using Microsoft.Phone.UserData;
using PlanMyWay.Lib.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanMyWay.Lib.Manager.EventArgs
{
    public class LocationCheckedEventArgs : System.EventArgs
    {
        public Meeting Meeting { get; set; }

        public LocationCheckedEventArgs()
        {
            Meeting = new Meeting();

        }
    }
}
