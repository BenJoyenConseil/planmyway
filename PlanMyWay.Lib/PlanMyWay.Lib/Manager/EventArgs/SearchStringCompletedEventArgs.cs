using Microsoft.Phone.Controls.Maps.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanMyWay.Lib.Manager.EventArgs
{
    public class SearchStringCompletedEventArgs : System.EventArgs
    {
        Location _location = new Location();
        String _city = string.Empty;
        String _address = string.Empty;

        bool _error = false;
        object _userData;

        public String Address
        {
            get { return _address; }
            set { _address = value; }
        }
        public String City
        {
            get { return _city; }
            set { _city = value; }
        }
        public object UserData
        {
            get { return _userData; }
            set { _userData = value; }
        }

        public SearchStringCompletedEventArgs(Location location, string city = "", string address = "", object userData = null)
        {
            _location = location;
            _city = city;
            _address = address;
        }

        public bool Error
        {
            get { return _error; }
            set { _error = value; }
        }

        public Location Location
        {
            get { return _location; }
            set { _location = value; }
        }
    }
}
