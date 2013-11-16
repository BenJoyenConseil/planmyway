using PlanMyWay.Lib.DataModel;
using PlanMyWay.Lib.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanMyWay.Lib.Util
{
    public class DateAndPositions
    {
        private Meeting _start = null;
        private Meeting _end = null;
        private DateTime _date;

        public String Date
        {
            get { return _date.ToShortDateString(); }
        }


        public Meeting Start
        {
            get { return _start; }
            set { _start = value; }
        }

        public Meeting End
        {
            get { return _end; }
            set { _end = value; }
        }

        public DateAndPositions(DateTime date, Meeting start, Meeting end)
        {
            _start = start;
            _end = end;
            _date = date;
        }
    }

    public class DateAndPositionsCollection : BindableBase
    {
        List<DateAndPositions> _items = new List<DateAndPositions>();

        public List<DateAndPositions> Items
        {
            get { return _items; }
            set {
                NotifyPropertyChanged(ref _items, value);
            }
        }

        public void Add(DateAndPositions item)
        {
            this._items.Add(item);
            this.NotifyPropertyChanged("Items");
        }

        public DateAndPositionsCollection()
        {   
            var rfM = new ReferenceMeeting(DateTime.Now.Date, null) { City = "Paris" };
            var rfM2 = new ReferenceMeeting(DateTime.Now.AddDays(1).Date, null) { City = "Paris" };
            var rfMbis = new ReferenceMeeting(DateTime.Now.Date.AddDays(1), null) { City = "Rouen" };
            var rfM2bis = new ReferenceMeeting(DateTime.Now.AddDays(2).Date, null) { City = "Quincampoix les Miroufles" };
            this._items.Add(new DateAndPositions(DateTime.Now.Date, rfM, rfM2));
            this._items.Add(new DateAndPositions(DateTime.Now.Date.AddDays(1), rfMbis, rfM2bis));
        }
    }
}
