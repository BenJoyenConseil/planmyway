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
using Coding4Fun.Toolkit.Controls;
using Microsoft.Advertising.Mobile.UI;

namespace PlanMyWay.Page
{
    public partial class SelectDateRoadTrip : PhoneApplicationPage
    {
        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
        public SelectDateRoadTrip()
        {
            InitializeComponent();
            if ((Application.Current as App).IsTrial)
            {

                AdControl control = new AdControl("9ec83715-ac1e-41b5-9b0d-62b3cb52524a", "10063501", true);
                control.Width = 300;
                control.Height = 50;
                control.Margin = new Thickness(0, 50, 0, 0);
                mainPane.Children.Add(control);
            }
            if (settings.Contains("SelectDateRoadTrip_persistence"))
                this.Dp_DateDebut.Value = (DateTime)settings["SelectDateRoadTrip_persistence"];
        }

        private void btn_Ok_Click(object sender, RoutedEventArgs e)
        {
            if (!settings.Contains("SelectDateRoadTrip_persistence"))
                settings.Add("SelectDateRoadTrip_persistence", "");
            settings["SelectDateRoadTrip_persistence"] = (DateTime)Dp_DateDebut.Value;
            settings.Save();
            if (!App.IsConnected())
            {
                MessageBox.Show("Aucune connexion n'est détectée !");
                return;
            }
            else if ((double)settings["latiAdrDepart"] != 0.0 &&
                (double)settings["longiAdrDepart"] != 0.0 &&
                (double)settings["latiAdrArriver"] != 0.0 &&
                (double)settings["longiAdrArriver"] != 0.0)
            {
                settings["Date"] = (DateTime)Dp_DateDebut.Value;
                NavigationService.Navigate(new Uri("/Page/PivotRoadTrip.xaml", UriKind.Relative));
            }
            else
            {
                MessageBox.Show("Veuillez vérifier vos lieux de départ et d'arrivée dans les paramètres.");
                NavigationService.Navigate(new Uri("/Page/PivotParam.xaml", UriKind.Relative));
            }
        }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }
    }
}