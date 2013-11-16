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
using Microsoft.Phone.Net.NetworkInformation;
using System.IO.IsolatedStorage;
using PlanMyWay.Lib.DataModel;

namespace PlanMyWay
{
    public partial class MenuPrin : PhoneApplicationPage
    {
        private bool _IsConnected = false;
        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
        bool firstStart;
        public MenuPrin()
        {
            InitializeComponent();
            if (App.IsConnected())
            {
                tb_PasDeConnection.Visibility = Visibility.Collapsed;
                _IsConnected = true;
            }
            else
            {
                tb_PasDeConnection.Visibility = Visibility.Visible;
                _IsConnected = false;
            }
            try
            {
                firstStart = (bool)settings["PremierLancement"];

            }
            catch
            {
                settings.Add("PremierLancement", (bool)false);
                settings.Add("Carburant", (int)1);
                settings.Add("ConsoCarburant", (double)6.5);
                settings.Add("AdrMail", (string)"linux@windows.mac");
                settings.Add("AdrDepart", (string)"Paris France");
                settings.Add("VilleDepart", (string)"Paris");
                settings.Add("longiAdrDepart", (double)0);
                settings.Add("latiAdrDepart", (double)0);
                settings.Add("AdrArriver", (string)"Paris France");
                settings.Add("VilleArriver", (string)"Paris");
                settings.Add("longiAdrArriver", (double)0);
                settings.Add("latiAdrArriver", (double)0);
                settings.Add("ActiveNotif", (bool)true);
                settings.Add("PrixCarburant", (double)1.45);
                settings.Add("Utilisateur", "Inconnue");
                settings.Add("LieuRef", "Paris");
                settings.Add("Date", (DateTime)DateTime.Today);
                settings.Add("DateFin", (DateTime)DateTime.Today);
                settings.Add("ListRoadeMap", (List<RoadMap>)null);
            }
        }


        private void btn_Parametre_Click(object sender, RoutedEventArgs e)
        {
            if(_IsConnected)
            NavigationService.Navigate(new Uri("/Page/PivotParam.xaml", UriKind.Relative));
        }

        private void btn_EditionCsv_Click(object sender, RoutedEventArgs e)
        {
            if (_IsConnected)
            NavigationService.Navigate(new Uri("/Page/SelectDate.xaml", UriKind.Relative));
        }

        private void btn_MyWay_Click(object sender, RoutedEventArgs e)
        {
            if (_IsConnected)
            NavigationService.Navigate(new Uri("/Page/SelectDateRoadTrip.xaml", UriKind.Relative));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Phone.Tasks.WebBrowserTask webBrowserTask = new Microsoft.Phone.Tasks.WebBrowserTask();
            webBrowserTask.Uri = new Uri("http://www.facebook.com/Planmyway",UriKind.Absolute);
            webBrowserTask.Show();
        }

        private void btn_Logo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.btn_Logo.Background = btn_MyWay.Background;
        }

        private void btn_Credits_Click(object sender, RoutedEventArgs e)
        {
            if (_IsConnected)
                NavigationService.Navigate(new Uri("/Page/Page1.xaml", UriKind.Relative));
        }
    }
}