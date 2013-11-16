using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.UserData;
using Microsoft.Phone.Shell;
using System.ComponentModel;

namespace PlanMyWay.Page
{
    public partial class DetailsAppointement : PhoneApplicationPage, INotifyPropertyChanged
    {
        private Appointment _AppointementChoose;
        private string _location, _starTime, _endTime, _Details, _Status, _subject;
        public event PropertyChangedEventHandler PropertyChanged;

        public string Subject
        {
            get { return _subject; }
            set
            {
                if (value == Subject)
                    return;
                _subject = value;
                NotifyPropertyChanged("Subject");                
            }
        }

        public string Status
        {
            get { return _Status; }
            set
            {
                _Status = value;
                NotifyPropertyChanged("Status");
            }
        }

        public string Details
        {
            get { return _Details; }
            set
            {
                _Details = value;
                NotifyPropertyChanged("Details");
            }
        }

        public string EndTime
        {
            get { return _endTime; }
            set
            {
                _endTime = value;
                NotifyPropertyChanged("EndTime");
            }
        }

        public string StarTime
        {
            get { return _starTime; }
            set 
            {
                _starTime = value;
                NotifyPropertyChanged("StarTime");
            }
        }

        public string Location
        {
            get { return _location; }
            set
            {
                _location = value;
                NotifyPropertyChanged("Location");
            }
        }
        public DetailsAppointement()
        {
            InitializeComponent();
            DataContext = this;
            _AppointementChoose = (Appointment)PhoneApplicationService.Current.State["Appointements"];
            this.Subject = _AppointementChoose.Subject;
            this.Location = _AppointementChoose.Location;
            this.StarTime = _AppointementChoose.StartTime.ToString();
            this.EndTime= _AppointementChoose.EndTime.ToString();
            this.Details = _AppointementChoose.Details;
            this.Status = _AppointementChoose.Status.ToString();

        }
        public void NotifyPropertyChanged(string nomPropriete)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(nomPropriete));
        }
    }
}