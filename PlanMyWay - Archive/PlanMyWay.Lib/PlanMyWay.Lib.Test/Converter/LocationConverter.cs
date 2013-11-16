using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Controls.Maps.Platform;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Device.Location;


namespace PlanMyWay.Lib.Test.Converter
{
    public class LocationConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
        { 
            if (value is Location) 
            { 
                return (value as Location).ToCoordinate(); 
            } 
            else if (value is IEnumerable<Location>) 
            { 
                return (value as IEnumerable<Location>).ToCoordinates(); 
            } 
            else 
            { 
                return null; 
            } 
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public static class GeoConverter
    {
        public static GeoCoordinate ToCoordinate(this Location routeLocation)
        {
            return new GeoCoordinate(routeLocation.Latitude, routeLocation.Longitude);
        }

        public static LocationCollection ToCoordinates(this IEnumerable<Location> points)
        {
            var locations = new LocationCollection();

            if (points != null)
            {
                foreach (var point in points)
                {
                    locations.Add(point.ToCoordinate());
                }
            }

            return locations;
        }
    }
}
