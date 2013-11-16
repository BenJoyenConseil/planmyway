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
using PlanMyWay.Lib.Test.Tests;
using PlanMyWay.Lib.Manager;
using System.Diagnostics;
using PlanMyWay.Lib.DataModel;
using Microsoft.Phone.Controls.Maps.Platform;
using PlanMyWay.Lib.Test.Converter;
using Microsoft.Phone.Controls.Maps;
using PlanMyWay.Lib.DataModel.RoadMapOptimization;
using Microsoft.Phone.Marketplace;

namespace PlanMyWay.Lib.Test
{
    public partial class MainPage : PhoneApplicationPage
    {
        RoadMap roadmap = null;
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            //MeetingManagerTest.SearchStringAsyncTEST();
            //MeetingManagerTest.GetAllMeetingsAsyncTEST();
            //TripManagerTest.GetTripAsyncTEST();
            //TripManagerTest.GetAllTripsAsyncTEST();
            //RoadMapManagerTest.GetRoadMapAsyncTEST();
            //RoadMapManagerTest.GetAllRoadMapsAsyncTEST();
            //HtmlTest.HtmlConstructTEST();
            //return;

            ///TRIAL
            //Microsoft.Phone.Tasks.MarketplaceDetailTask market = new Microsoft.Phone.Tasks.MarketplaceDetailTask();
            //LicenseInformation licence = new LicenseInformation();
            //licence.IsTrial();

            IRoadMapManager manager = new RoadMapManager();
            this.DataContext = new Route();
            (DataContext as Route).Credential = BingMapCredential.CREDENTIAL;
            manager.RoadMapReceived += OnRoadMapReceived;

            //Meeting de départ
            ReferenceMeeting start = new ReferenceMeeting(new DateTime(2013, 1, 2, 8, 30, 0), new Location()
            {
                Latitude = 48.85693,
                Longitude = 2.3412
            }) { 
                City = "Paris", 
                Subject = "Départ coucou lol" 
            };
            //Meeting d'arrivé
            ReferenceMeeting end = new ReferenceMeeting(new DateTime(2013, 1, 2, 18, 30, 0), new Location()
            {
                Latitude = 48.85693,
                Longitude = 2.3412
            }) { 
                City = "Paris", 
                Subject = "Arrivée coucou lol" 
            };
            //Appel de la récupération des données
            manager.GetRoadMapAsync(new DateTime(2013, 1, 3), start, end);
            
        }

        private void OnRoadMapReceived(object o, Manager.EventArgs.RoadMapReceivedEventArgs e)
        {
            if (e.Error)
            {
                Debug.WriteLine("Aucun rendez-vous n'existe pour le jour sélectionné");
                MessageBox.Show("Aucun rendez-vous n'existe pour le jour sélectionné");
                return;
            }
            roadmap = e.RoadMap;
            map.SetView(e.RoadMap.RouteResult.Summary.BoundingRectangle);
            var strokeThickness = e.RoadMap.Trips.Count * 2 + 1;
            foreach (var portion in e.RoadMap.Trips)
            {
                Random rand = new Random();
                byte r = (byte)rand.Next(0, 255);
                byte g = (byte)rand.Next(0, 255);
                byte b = (byte)rand.Next(0, 255);
                MapPolyline polyline = new MapPolyline();
                polyline.StrokeThickness = strokeThickness;
                polyline.Opacity = 1;
                polyline.Locations = new LocationCollection();
                foreach (var point in portion.RoutePath)
                    polyline.Locations.Add(point);
                map.Children.Add(polyline);
                polyline.Stroke = new SolidColorBrush(Color.FromArgb(255, 27, 161, 226));
                if (portion.IsTripFail)
                {
                    polyline.StrokeThickness += 2;
                    polyline.Stroke = new SolidColorBrush(Colors.Red);
                }
                if (portion.End.Subject == "End")
                    polyline.Stroke = new SolidColorBrush(Colors.Black);

                strokeThickness -= 2;
            }

            foreach (var rdv in e.RoadMap.Meetings)
            {
                if (rdv.IsLocationFail)
                    continue;
                Pushpin p = new Pushpin();
                if (rdv.Subject == string.Empty)
                {
                    p.Content = "Start";
                }
                else
                {
                    p.Content = rdv.Subject;
                }
                p.Background = new SolidColorBrush(Colors.Black);
                p.Location = rdv.Location;
                map.Children.Add(p);
            }
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (roadmap == null)
            {
                MessageBox.Show("Aucune RoadMap chargée..");
                return;
            }
            MeetingCollection bestorder = roadmap.OptimizedMeetingOrder;
            string message = "";
            foreach(var m in  bestorder)
                message+= m.Subject+" - ";
            MessageBox.Show(message);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }
    }
}