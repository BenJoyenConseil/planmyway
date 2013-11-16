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

namespace PlanMyWay
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
        bool firstStart;
        public MainPage()
        {
            InitializeComponent();
            try
            {
                firstStart = (bool)settings["PremierLancement"];
               
            }
            catch
            {
                settings.Add("PremierLancement", (bool)false);
                settings.Add("Carburant", (int)1);
                settings.Add("ConsoCarburant", (double)6.5);
                settings.Add("AdrMail", (string)"arnaud.lebris@viacesi.fr");
                settings.Add("AdrDepart", (string)"23 coteaux de repainville 76000 Rouen");
                settings.Add("VilleDepart", (string)"Rouen");
                settings.Add("longiAdrDepart", (double)0);
                settings.Add("latiAdrDepart", (double)0);
                settings.Add("AdrArriver", (string)"23 coteaux de repainville 76000 Rouen");
                settings.Add("VilleArriver", (string)"Rouen");
                settings.Add("longiAdrArriver", (double)0);
                settings.Add("latiAdrArriver", (double)0);
                settings.Add("ActiveNotif", (bool)true);
                settings.Add("PrixCarburant", (double)1.45);
                settings.Add("Utilisateur", "ZinZin");
                settings.Add("LieuRef", "Rouen");
                settings.Add("Date", (DateTime)DateTime.Today);
            }


        }

        private void btn_Menu_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Page/MenuPrin.xaml", UriKind.Relative));
        }
    }
}