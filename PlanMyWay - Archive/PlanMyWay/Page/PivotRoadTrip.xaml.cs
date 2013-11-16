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
using PlanMyWay.Lib.Manager;
using PlanMyWay.Lib.DataModel;
using Microsoft.Phone.Controls.Maps.Platform;
using PlanMyWay.Lib.Manager.EventArgs;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using PlanMyWay.UserControlPMW;
using System.Diagnostics;
using Microsoft.Phone.Controls.Maps;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;
using Coding4Fun.Phone.Controls;
using Coding4Fun.Toolkit.Controls;

namespace PlanMyWay.Page
{
    public partial class PivotRoadTrip : PhoneApplicationPage
    {
        RoadMapManager myRMM;
        RoadMap myRoadM;
        List<Meeting> m;
        List<Meeting> meetingLocationFail;
        List<Trip> tripTimeFail;
        MeetingManager myMM;
        Meeting MeetTmps;
        ProgressIndicator indicator;
        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
        FlecheBasInfo flbI_InfoRdv;
        ApplicationBarIconButton btn_setting;
        ApplicationBarIconButton btn_refresh;
        bool traitementEnd = false;


        public PivotRoadTrip()
        {
            InitializeComponent();
            btn_setting = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
            btn_refresh = ApplicationBar.Buttons[1] as ApplicationBarIconButton;
            btn_refresh.IsEnabled = false;
            btn_setting.IsEnabled = false;
            initRoadTrip();
           
        }

        private void initRoadTrip()
        {
            if (settings["latiAdrDepart"] != "" && settings["longiAdrDepart"] != "" && settings["latiAdrArriver"] != "" && settings["longiAdrArriver"] != "")
            {
                rt_ConsoTrajet.tb_Info1.Text = "Consomation";
                rt_CoutTot.tb_Info1.Text = "Coût total";
                rt_DistRdv.tb_Info1.Text = "Distance parcourue";
                rt_nbrRdv.tb_Info1.Text = "Nombres de RDVs";
                rt_TpsTrajetRdv.tb_Info1.Text = "Temps de trajet";
                gr_AnomalieLieu.Children.Clear();
                gr_AnomalieTemps.Children.Clear();
                gr_InfoRD.Children.Clear();
                gr_InfoRDOpti.Children.Clear();
                map_Rdv.Children.Clear();
                myRMM = new RoadMapManager();
                myMM = new MeetingManager();
                tripTimeFail = new List<Trip>();
                meetingLocationFail = new List<Meeting>();

                var myRefMeetDepart = new ReferenceMeeting();
                myRefMeetDepart.Address = (string)settings["AdrDepart"];
                myRefMeetDepart.City = (string)settings["VilleDepart"];
                myRefMeetDepart.Location = new Location() { Latitude = (double)settings["latiAdrDepart"], Longitude = (double)settings["longiAdrDepart"] };

                var myRefMeetArriver = new ReferenceMeeting();
                myRefMeetArriver.Address = (string)settings["AdrArriver"];
                myRefMeetArriver.City = (string)settings["VilleArriver"];
                myRefMeetArriver.Location = new Location() { Latitude = (double)settings["latiAdrArriver"], Longitude = (double)settings["longiAdrArriver"] };

                myRMM.RoadMapReceived += OnRoadMapReceived;
                myRMM.GetRoadMapAsync((DateTime)settings["Date"], myRefMeetDepart, myRefMeetArriver);

                indicator = new ProgressIndicator
                {
                    IsVisible = true,
                    IsIndeterminate = true,
                    Text = "Chargement..."
                };
                SystemTray.SetProgressIndicator(this, indicator);
            }
            else
            {
                MessageBox.Show("Veuillez vérifier vos lieux de départ et d'arriver dans les paramètres");
            }
        }

        private void InitGrid()
        {
            Trip monTrip;
            RowDefinition tmpRow;
            double tpsTot=0;
            if (m != null)
            {
                for (int i = 0; i < m.Count; i++)
                {
                    monTrip =myRoadM.Trips.Where(o => o.Start.Equals(m[i])).FirstOrDefault();
                    
                    if (monTrip != null  && !m[i].IsLocationFail)
                    {
                        if (i == 0)
                        {
                            tmpRow = new RowDefinition();
                            tmpRow.Height = GridLength.Auto;
                            gr_InfoRD.RowDefinitions.Add(tmpRow);
                            flbI_InfoRdv = new FlecheBasInfo();
                            flbI_InfoRdv.SujetRdv = "Départ";
                            flbI_InfoRdv.AdrRdv = m[i].City;
                            flbI_InfoRdv.Distance = monTrip.Distance + " km";
                            flbI_InfoRdv.TpsTrajet = monTrip.Duration + " min";
                            tpsTot = tpsTot + monTrip.Duration;
                            flbI_InfoRdv.ConSomation = Math.Round(((monTrip.Distance * (double)settings["ConsoCarburant"]) / 100), 2) + " L";
                            flbI_InfoRdv.Cout =Math.Round((((monTrip.Distance * (double)settings["ConsoCarburant"]) / 100) * (double)settings["PrixCarburant"]),2) + " €";
                            flbI_InfoRdv.IsTripFail1 = monTrip.IsTripFail;
                            flbI_InfoRdv.InitInfo();
                            flbI_InfoRdv.Margin = new Thickness(1, 1, 1, 1);
                            flbI_InfoRdv.SetValue(Grid.RowProperty, i);
                            gr_InfoRD.Children.Add(flbI_InfoRdv);
                            continue;
                        }
                        tmpRow = new RowDefinition();
                        tmpRow.Height = GridLength.Auto;
                        gr_InfoRD.RowDefinitions.Add(tmpRow);
                        flbI_InfoRdv = new FlecheBasInfo();
                        flbI_InfoRdv.SujetRdv = m[i].Subject+" ("+m[i].DateTime.TimeOfDay+")";
                        flbI_InfoRdv.AdrRdv = m[i].Address;
                        flbI_InfoRdv.Distance = monTrip.Distance + " km";
                        flbI_InfoRdv.TpsTrajet = monTrip.Duration + " min";
                        tpsTot = tpsTot + monTrip.Duration;
                        flbI_InfoRdv.IsTripFail1 = monTrip.IsTripFail;
                        flbI_InfoRdv.ConSomation = Math.Round(((monTrip.Distance * (double)settings["ConsoCarburant"])/100),2) + " L";
                        flbI_InfoRdv.Cout = Math.Round((((monTrip.Distance * (double)settings["ConsoCarburant"]) / 100) * (double)settings["PrixCarburant"]), 2) + " €";
                        flbI_InfoRdv.InitInfo();                        
                        flbI_InfoRdv.SetValue(Grid.RowProperty, i);
                        flbI_InfoRdv.Margin = new Thickness(1, 5, 1, 5);
                        gr_InfoRD.Children.Add(flbI_InfoRdv);

                    }
                    if (i == m.Count - 1)
                    {

                        var monTxt = new TextBlock();
                        tmpRow = new RowDefinition();
                        tmpRow.Height = GridLength.Auto;
                        gr_InfoRD.RowDefinitions.Add(tmpRow);
                        monTxt.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                        monTxt.SetValue(Grid.RowProperty, i);
                        monTxt.Text = "Arrivée";
                        monTxt.FontSize = 35;
                        System.Windows.Shapes.Rectangle monRect = new System.Windows.Shapes.Rectangle();
                        monRect.Height = 45;
                        monRect.Width = 340;
                        monRect.SetValue(Grid.RowProperty, i);
                        monRect.SetValue(System.Windows.Shapes.Rectangle.FillProperty, App.Current.Resources["PhoneAccentBrush"]);
                        gr_InfoRD.Children.Add(monRect);
                        gr_InfoRD.Children.Add(monTxt);
                        monTxt = new TextBlock();
                        tmpRow = new RowDefinition();
                        tmpRow.Height = GridLength.Auto;
                        gr_InfoRD.RowDefinitions.Add(tmpRow);
                        monTxt.SetValue(Grid.RowProperty, i+2);
                        monTxt.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                        monTxt.Text = m[i].City;
                        monTxt.FontSize = 20;
                        gr_InfoRD.Children.Add(monTxt);
                    }

                }
                rt_ConsoTrajet.tb_info2.Text = Math.Round(((myRoadM.Distance * (double)settings["ConsoCarburant"]) / 100), 2) + " L"; ;
                rt_CoutTot.tb_info2.Text = Math.Round((((myRoadM.Distance * (double)settings["ConsoCarburant"]) / 100) * (double)settings["PrixCarburant"]), 2) + " €"; ;
                rt_DistRdv.tb_info2.Text = myRoadM.Distance+"km";
                rt_nbrRdv.tb_info2.Text = myRoadM.Meetings.Count-2+"";
                rt_TpsTrajetRdv.tb_info2.Text =tpsTot+" min";
               

            }
            initRdvOpti();
            indicator.IsIndeterminate = false;
            indicator.IsVisible = false;
        }
        private void InitAnomlie()
        {
            RectAnomalie tmp;
            RowDefinition tmpRow;
            int compteurLigne = 1;
            for (int i = 0; i < tripTimeFail.Count; i++)
            {
                
                    try
                    {
                        tmpRow = new RowDefinition();
                        tmpRow.Height = GridLength.Auto;
                        gr_AnomalieTemps.RowDefinitions.Add(tmpRow);
                        tmp = new RectAnomalie();
                        tmp.NomRdv = tripTimeFail[i].Start.Subject + " => " + tripTimeFail[i].End.Subject;
                        tmp.Distance_AdrRdv = tripTimeFail[i].Distance + " km";
                        tmp.TpsPrevu = tripTimeFail[i].Duration + " min ";
                        tmp.InitInfo();
                        tmp.Height = 50;
                        tmp.Width = 400;
                        tmp.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                        tmp.Margin = new Thickness(1, 5, 1, 5);
                        tmp.SetValue(Grid.RowProperty, compteurLigne);
                        gr_AnomalieTemps.Children.Add(tmp);
                        compteurLigne++;
                    }
                    catch
                    {
                        MessageBox.Show("Erreur de recepetion de la map bing, Réesseyer");
                    }

                
            }
            for (int i = 0; i < meetingLocationFail.Count; i++)
            {
              
                    try
                    {
                        tmpRow = new RowDefinition();
                        tmpRow.Height = GridLength.Auto;
                        gr_AnomalieLieu.RowDefinitions.Add(tmpRow);
                        tmp = new RectAnomalie();
                        tmp.NomRdv = meetingLocationFail[i].Subject;
                        tmp.Distance_AdrRdv = meetingLocationFail[i].Address;
                        tmp.TpsPrevu = "=> Adresse invalide";
                        tmp.InitInfo();
                        tmp.Height = 50;
                        tmp.Width = 400;
                        tmp.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                        tmp.Margin = new Thickness(1, 5, 1, 5);
                        tmp.SetValue(Grid.RowProperty, compteurLigne);
                        gr_AnomalieLieu.Children.Add(tmp);
                        compteurLigne++;
                    }
                    catch
                    {
                        MessageBox.Show("Erreur de recepetion de la map bing, Réesseyer");
                    }
            }
            initToast(tripTimeFail.Count + meetingLocationFail.Count);
        }
        private void OnRoadMapReceived(object o, PlanMyWay.Lib.Manager.EventArgs.RoadMapReceivedEventArgs e)
        {
            int counRdv = -1;
            int endRdv = 0;
            try
            {
                if (e.Error)
                {
                    Debug.WriteLine("Aucun rendez-vous n'existe pour le jour sélectionné");
                    MessageBox.Show("Aucun rendez-vous n'existe pour le jour sélectionné ou bien Vérifier les lieux de départ et d'arrivée dans l'application");
                    indicator.IsIndeterminate = false;
                    indicator.IsVisible = false;
                    btn_refresh.IsEnabled = true;
                    btn_setting.IsEnabled = true;
                    traitementEnd = true;
                    
                    return;
                }
                myRoadM = e.RoadMap;
                map_Rdv.SetView(e.RoadMap.RouteResult.Summary.BoundingRectangle);
                var strokeThickness = e.RoadMap.Trips.Count * 2 + 1;
                foreach (var portion in e.RoadMap.Trips)
                {
                    endRdv++;
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
                    map_Rdv.Children.Add(polyline);
                    polyline.Stroke = new SolidColorBrush(Color.FromArgb(255, 27, 161, 226));
                    if (portion.IsTripFail)
                    {
                        polyline.StrokeThickness += 2;
                        polyline.Stroke = new SolidColorBrush(Colors.Red);
                    }
                    if (endRdv == e.RoadMap.Trips.Count)
                        polyline.Stroke = new SolidColorBrush(Colors.Black);
                    strokeThickness -= 2;
                   
                }

                foreach (var rdv in e.RoadMap.Meetings)
                {
                    
                    if (rdv.IsLocationFail)
                        continue;
                    Pushpin p = new Pushpin();
                    if (counRdv == 1 && rdv.GetType() == typeof(ReferenceMeeting))
                    {
                        p.Content = "Départ";
                    }
                    else
                    {
                        counRdv++;
                        p.Content = "RDV" + (counRdv) + " : " + rdv.Subject;
                       
                    }
                    if (counRdv != 1 && rdv.GetType() == typeof(ReferenceMeeting))
                    {
                        p.Content = "Arrivée";
                    }

                    p.Background = new SolidColorBrush(Colors.Black);
                    p.Location = rdv.Location;
                    map_Rdv.Children.Add(p);
                }
                if (e.RoadMap != null)
                {
                    m = e.RoadMap.ValidMeetings;
                    meetingLocationFail = e.RoadMap.LocationFailMeetings;
                    tripTimeFail = e.RoadMap.FailTrips;
                    myRoadM = e.RoadMap;
                }
                InitGrid();
                InitAnomlie();
                btn_refresh.IsEnabled = true;
                btn_setting.IsEnabled = true;
                traitementEnd = true;
            }
            catch(Exception exception)
            {
                MessageBox.Show("Erreur de recepetion de la map bing, Réesseyer");
                Console.WriteLine(exception.Message.ToString());
                traitementEnd = true;
            }
        }
        private void initToast(int nbrErreur)
        {
            if (nbrErreur != 0)
            {
                ToastPrompt toast = new ToastPrompt();
                toast.Title = "Attention !";
                toast.Message = "Vous avez : " + nbrErreur + " erreur(s) temps/trajet";
                toast.FontSize = 20;
                toast.TextOrientation = System.Windows.Controls.Orientation.Vertical;
                toast.Completed += toast_Completed;
                toast.Show();
            }
         
        }
        private void initRdvOpti()
        {
            List<Meeting>tmp = new List<Meeting>();
            RowDefinition tmpRow;
            tmp=myRoadM.OptimizedMeetingOrder;
            for (int i = 0; i < tmp.Count; i++)
            {
                if (i == 0)
                {
                    tmpRow = new RowDefinition();
                    tmpRow.Height = GridLength.Auto;
                    gr_InfoRDOpti.RowDefinitions.Add(tmpRow);
                    flbI_InfoRdv = new FlecheBasInfo();
                    flbI_InfoRdv._EnabledAnimation = false;
                    flbI_InfoRdv.SujetRdv = "Départ";
                    flbI_InfoRdv.AdrRdv = tmp[i].City;
                    flbI_InfoRdv.InitInfo();
                    flbI_InfoRdv.Margin = new Thickness(1, 1, 1, 1);
                    flbI_InfoRdv.SetValue(Grid.RowProperty, i);
                    gr_InfoRDOpti.Children.Add(flbI_InfoRdv);
                    continue;
                }
                if (tmp[i].Subject != "Départ" && tmp[i].Subject != "Arrivée")
                {
                    tmpRow = new RowDefinition();
                    tmpRow.Height = GridLength.Auto;
                    gr_InfoRDOpti.RowDefinitions.Add(tmpRow);
                    flbI_InfoRdv._EnabledAnimation = false;
                    flbI_InfoRdv = new FlecheBasInfo();
                    flbI_InfoRdv.SujetRdv = tmp[i].Subject + " (" + tmp[i].DateTime.TimeOfDay + ")";
                    flbI_InfoRdv.AdrRdv = tmp[i].Address;
                    flbI_InfoRdv.InitInfo();
                    flbI_InfoRdv.SetValue(Grid.RowProperty, i);
                    flbI_InfoRdv.Margin = new Thickness(1, 5, 1, 5);
                    gr_InfoRDOpti.Children.Add(flbI_InfoRdv);
                }
                if (i == tmp.Count - 1)
                {
                    var monTxt = new TextBlock();
                    tmpRow = new RowDefinition();
                    tmpRow.Height = GridLength.Auto;
                    gr_InfoRDOpti.RowDefinitions.Add(tmpRow);
                    monTxt.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    monTxt.SetValue(Grid.RowProperty, i+1);
                    monTxt.Text = "Arrivée";
                    monTxt.FontSize = 35;
                    System.Windows.Shapes.Rectangle monRect = new System.Windows.Shapes.Rectangle();
                    monRect.Height = 45;
                    monRect.Width =340;
                    monRect.SetValue(Grid.RowProperty, i+1);
                    monRect.SetValue(System.Windows.Shapes.Rectangle.FillProperty, App.Current.Resources["PhoneAccentBrush"]);
                    gr_InfoRDOpti.Children.Add(monRect);
                    gr_InfoRDOpti.Children.Add(monTxt);
                    monTxt = new TextBlock();
                    tmpRow = new RowDefinition();
                    tmpRow.Height = GridLength.Auto;
                    gr_InfoRDOpti.RowDefinitions.Add(tmpRow);
                    tmpRow = new RowDefinition();
                    tmpRow.Height = GridLength.Auto;
                    gr_InfoRDOpti.RowDefinitions.Add(tmpRow);
                    monTxt.SetValue(Grid.RowProperty, i+2);
                    monTxt.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    monTxt.Text = tmp[i].City;
                    monTxt.FontSize = 20;
                    gr_InfoRDOpti.Children.Add(monTxt);
                }
            }
        }
        private void btn_settings_Click(object sender, EventArgs e)
        {
            if (traitementEnd)
            {
                NavigationService.Navigate(new Uri("/Page/PivotParam.xaml", UriKind.Relative));
            }
        }

        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            if (traitementEnd)
            {
                btn_refresh.IsEnabled = false;
                btn_setting.IsEnabled = false;
                traitementEnd = false;
                initRoadTrip();
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
          
        }
        void toast_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            //add some code here
        }
    }
}
