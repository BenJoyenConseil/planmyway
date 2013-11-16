using Microsoft.Phone.Controls.Maps.Platform;
using PlanMyWay.Lib.RouteService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanMyWay.Lib.DataModel
{
    public class MeetingCollection : List<Meeting>
    {
        public Meeting GetByLocation(Location location)
        {
            foreach (var item in this)
            {
                if (item.Location.Equals(location))
                    return item;
            }
            throw new Exception("Meeting not found");
        }

        public List<Location> Locations
        {
            get
            {
                List<Location> locations = new List<Location>();
                foreach (var item in this)
                {
                    if (!item.IsLocationFail)
                        locations.Add(item.Location);
                }
                return locations;
            }
        }
    }
}
