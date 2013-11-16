using Microsoft.Phone.Controls.Maps.Platform;
using System;

namespace PlanMyWay.Lib.DataModel
{
    public class Meeting
    {
        protected string _subject;
        protected string _address;

        
        protected String _city;
        protected Location _location;
        protected DateTime _datetime;
        protected double _duration;
        protected bool _isLocationFail;


        public string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }
        public Location Location
        {
            get { return _location; }
            set { _location = value; }
        }
        public DateTime DateTime
        {
            get { return _datetime; }
            set { _datetime = value; }
        }
        public String City
        {
            get { return _city; }
            set { _city = value; }
        }

        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }
        /// <summary>
        /// Durée en minutes
        /// </summary>
        public double Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }
        public bool IsLocationFail
        {
            get { return _isLocationFail; }
            set { _isLocationFail = value; }
        }

        public Meeting()
        {
            Location = new Location();
            DateTime = DateTime.Now;
            Duration = 0;
            IsLocationFail = false;
            City = String.Empty;
            Address = String.Empty;
        }
    }
}
