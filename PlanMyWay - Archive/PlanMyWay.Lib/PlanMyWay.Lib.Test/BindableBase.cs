using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Controls.Maps.Platform;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PlanMyWay.Lib.Test
{

    public abstract class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string nomPropriete)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(nomPropriete));
        }

        public bool NotifyPropertyChanged<T>(ref T variable, T valeur, string nomPropriete = null)
        {
            if (object.Equals(variable, valeur)) return false;

            variable = valeur;
            NotifyPropertyChanged(nomPropriete);
            return true;
        }
    }

    public class Route : BindableBase
    {
        ObservableCollection<Location> _points = new ObservableCollection<Location>();

        public ObservableCollection<Location> RoutePoints
        {
            get
            {
                return _points;
            }
            set
            {
                NotifyPropertyChanged(ref _points, value);
            }
        }

        String _credential = string.Empty;

        public String Credential
        {
            get
            {
                return _credential;
            }
            set
            {
                NotifyPropertyChanged(ref _credential, value);
            }
        }

    }
}
