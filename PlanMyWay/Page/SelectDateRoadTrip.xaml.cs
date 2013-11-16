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
using System.IO.IsolatedStorage;

namespace PlanMyWay.Page
{
    public partial class SelectDateRoadTrip : PhoneApplicationPage
    {
        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
        public SelectDateRoadTrip()
        {
            InitializeComponent();
        }

        private void btn_Ok_Click(object sender, RoutedEventArgs e)
        {
            settings["Date"] = (DateTime) Dp_DateDebut.Value;
            NavigationService.Navigate(new Uri("/Page/PivotRoadTrip.xaml", UriKind.Relative));
        }
    }
}