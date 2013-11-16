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

        public double CalculateGeoCoordinatesDistanceBetween(int indexMeeting1, int indexMeeting2)
        {
            if (this.Count <= 0)
                return 0;
            if (indexMeeting1 < 0 || indexMeeting1 >= this.Count)
                return 0;
            if (indexMeeting2 < 0 || indexMeeting2 >= this.Count)
                return 0;
            if (indexMeeting1 == indexMeeting2)
                return 0;

            //On utilise le théorème de Pythagore pour calculer la taille du segment entre le point A et le point B
            double xA = 0, xB = 0, yA = 0, yB = 0;
            xA = this[indexMeeting1].Location.Latitude;
            yA = this[indexMeeting1].Location.Longitude;
            xB = this[indexMeeting2].Location.Latitude;
            yB = this[indexMeeting2].Location.Longitude;

            //Application du théorème : AB² = A²+B²
            return Math.Sqrt(Math.Pow((xB - xA), 2) + Math.Pow((yB - yA), 2));
        }
    }
}
