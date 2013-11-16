using Microsoft.Phone.Controls.Maps.Platform;
using PlanMyWay.Lib;
using PlanMyWay.Lib.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace PlanMyWay.Design
{
    public class TripFailDesign : BindableBase
    {
        Trip _trip;

        public TripFailDesign(Trip trip)
        {
            _trip = trip;
        }

        public string DateTimeStart{
            get{
                return _trip.Start.DateTime.ToShortTimeString();
            }
        }
        public string DateTimeEnd{
            get{
                return _trip.End.DateTime.ToShortTimeString();
            }
        }
        public Meeting End
        {
            get { 
                return _trip.End; 
            }
        }
        public Meeting Start
        {
          get { 
              return _trip.Start; 
          }
        }
        public Trip Trip
        {
          get { return _trip; }
          set { _trip = value; }
        }
    }

    public class TripFailCollectionDesign : BindableBase
    {
        ObservableCollection<TripFailDesign> _items = new ObservableCollection<TripFailDesign>();

        public TripFailCollectionDesign()
        {
            Trip t = new Trip(){
                Start = new ReferenceMeeting(DateTime.Now.Date.AddHours(8), null)
                {
                    Subject = "Company Meeting Microsoft 2013"
                },
                End = new ReferenceMeeting(DateTime.Now.Date.AddHours(12), null)
                {
                    Subject = "M. Dupont agence immo"
                }
            };
            Trip t2 = new Trip(){
                Start = new ReferenceMeeting(DateTime.Now.Date.AddHours(14), null)
                {
                    Subject = "RDV3 subject"
                },
                End = new ReferenceMeeting(DateTime.Now.Date.AddHours(15), null)
                {
                    Subject = "RDV4 blabla subject qjpd",
                    
                }
            };

            _items.Add(new TripFailDesign(t));
            _items.Add(new TripFailDesign(t2));
        }

        public ObservableCollection<TripFailDesign> Items
        {
            get { return _items; }
            set { _items = value; }
        }
    }
}
