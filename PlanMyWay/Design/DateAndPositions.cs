using PlanMyWay.Lib.DataModel;
using PlanMyWay.Lib.Test;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace PlanMyWay.Lib.Util
{
    public class DateAndPositions : BindableBase
    {
        private Meeting _start = null;
        private Meeting _end = null;
        private DateTime _date;
        private int _id;
        private RoadMap _roadmap;
        private bool _isEdited;

        public bool IsEdited
        {
            get { return _isEdited; }
            set { _isEdited = value; }
        }

        public RoadMap Roadmap
        {
            get { return _roadmap; }
        }

        public int Id
        {
            get { return _id; } 
        }

        public String Date
        {
            get { return _date.ToShortDateString(); }
        }


        public Meeting Start
        {
            get { return _start; }
            set
            {
                NotifyPropertyChanged(ref _start, value);
                NotifyPropertyChanged("End");
            }
        }

        public Meeting End
        {
            get { return _end; }
            set
            {
                NotifyPropertyChanged(ref _end, value);
                NotifyPropertyChanged("Start");
            }
        }

        public DateAndPositions(int id, DateTime date, Meeting start, Meeting end)
        {
            _id = id;
            _start = start;
            _end = end;
            _date = date;
        }

        public DateAndPositions(int id, RoadMap roadmap) 
            : this(id, roadmap.Date, roadmap.Meetings.FirstOrDefault(), roadmap.Meetings.LastOrDefault())
        {
            _roadmap = roadmap;
        }
    }

    public class DateAndPositionsCollection : BindableBase
    {
        protected ObservableCollection<DateAndPositions> _items = new ObservableCollection<DateAndPositions>();

        public ObservableCollection<DateAndPositions> Items
        {
            get { return _items; }
            set {
                NotifyPropertyChanged(ref _items, value);
            }
        }

        public DateAndPositionsCollection()
        {   
        }
    }

    public class DateAndPositionsCollectionDesign : DateAndPositionsCollection
    {
        public DateAndPositionsCollectionDesign()
            : base()
        {
            var rfM = new ReferenceMeeting(DateTime.Now.Date, null) { City = "Paris" };
            var rfM2 = new ReferenceMeeting(DateTime.Now.AddDays(1).Date, null) { City = "Paris" };
            var rfMbis = new ReferenceMeeting(DateTime.Now.Date.AddDays(1), null) { City = "Rouen" };
            var rfM2bis = new ReferenceMeeting(DateTime.Now.AddDays(2).Date, null) { City = "Quincampoix les Miroufles" };
            this._items.Add(new DateAndPositions(0, DateTime.Now.Date, rfM, rfM2));
            this._items.Add(new DateAndPositions(1, DateTime.Now.Date.AddDays(1), rfMbis, rfM2bis));
        }
    }
}
