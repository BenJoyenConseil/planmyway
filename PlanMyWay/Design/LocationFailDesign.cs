using PlanMyWay.Lib.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace PlanMyWay.Design
{
    public class LocationFailDesign
    {
        Meeting _meeting;

        public LocationFailDesign(Meeting meeting)
        {
            _meeting = meeting;
        }

        public string DateTime
        {
            get
            {
                return _meeting.DateTime.ToShortTimeString();
            }
        }
        public Meeting Meeting
        {
            get { return _meeting; }
            set { _meeting = value; }
        }
    }

    public class LocationFailCollectionDesign
    {
        ObservableCollection<LocationFailDesign> _items = new ObservableCollection<LocationFailDesign>();

        public LocationFailCollectionDesign()
        {
            Meeting m = new Meeting() { IsLocationFail = true, Subject = "Company Meeting Microsoft 2013", DateTime = DateTime.Now };
            Meeting m2 = new Meeting() { IsLocationFail = true, Subject = " Dupont agence immo", DateTime = DateTime.Now.Date.AddHours(12) };
            _items.Add(new LocationFailDesign(m));
            _items.Add(new LocationFailDesign(m2));
        }

        public ObservableCollection<LocationFailDesign> Items
        {
            get { return _items; }
            set { _items = value; }
        }
    }
}
