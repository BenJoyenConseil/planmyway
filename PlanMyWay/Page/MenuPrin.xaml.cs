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
using Microsoft.Phone.Tasks;
using Microsoft.Advertising.Mobile.UI;
using System.Diagnostics;

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
                settings.Add("AdrMail", (string)"");
                settings.Add("AdrDepart", (string)"");
                settings.Add("VilleDepart", (string)"");
                settings.Add("longiAdrDepart", (double)0);
                settings.Add("latiAdrDepart", (double)0);
                settings.Add("AdrArriver", (string)"");
                settings.Add("VilleArriver", (string)"");
                settings.Add("longiAdrArriver", (double)0);
                settings.Add("latiAdrArriver", (double)0);
                settings.Add("ActiveNotif", (bool)true);
                settings.Add("PrixCarburant", (double)1.45);
                settings.Add("Utilisateur", "Inconnue");
                settings.Add("LieuRef", "");
                settings.Add("Date", (DateTime)DateTime.Today);
                settings.Add("DateFin", (DateTime)DateTime.Today);
                settings.Add("ListRoadeMap", (List<RoadMap>)null);
            }

            if ((Application.Current as App).IsTrial)
            {

                AdControl control = new AdControl("9ec83715-ac1e-41b5-9b0d-62b3cb52524a", "10063501", true);
                control.Width = 300;
                control.Height = 50;
                control.ErrorOccurred += control_ErrorOccurred;
                pubPane.Children.Insert(0, control);

                ApplicationBar.IsVisible = true;
                panorama.Title = "Version Gratuite";
                btn_EditionCsv.IsEnabled = false;
                btn_EditionCsv.BorderThickness = new Thickness(5);
                btn_EditionCsv.Background = new SolidColorBrush(Colors.LightGray);
                //tbTrialStatus.Text = "Trial";
                
            }
            else
            {
                //mainPane.Header = "Version Pro";
                ApplicationBar.IsVisible = false;
                btn_EditionCsv.IsEnabled = true;
                //tbTrialStatus.Text = "Full";
            }
        }

        void control_ErrorOccurred(object sender, Microsoft.Advertising.AdErrorEventArgs e)
        {
            Debug.WriteLine(e.Error.Message);
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
            Microsoft.Phone.Tasks.ShareLinkTask shareTask = new Microsoft.Phone.Tasks.ShareLinkTask();
            shareTask.Title = "Découvrez la page PlanMyWay";
            shareTask.Message = "PlanMyWay est une application intelligente qui vous permettra d'optimiser vos trajets professionnels et personnels. "+
                                "Visualisez et optimisez vos trajets simplement et efficacement en quelques minutes seulement ! "+
                                "Economisez sur votre compta en générant des feuilles de routes pointues.";
            shareTask.LinkUri = new Uri("http://www.facebook.com/Planmyway", UriKind.Absolute);
            shareTask.Show();
            return;
            //Microsoft.Phone.Tasks.WebBrowserTask webBrowserTask = new Microsoft.Phone.Tasks.WebBrowserTask();
            //webBrowserTask.Uri = new Uri("http://www.facebook.com/Planmyway",UriKind.Absolute);
            //webBrowserTask.Show();
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

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            MarketplaceDetailTask _marketPlaceDetailTask = new MarketplaceDetailTask();
            _marketPlaceDetailTask.Show();
        }
    }
}