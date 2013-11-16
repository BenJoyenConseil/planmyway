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

namespace PlanMyWay.Page
{
    public partial class PivotRoadTrip : PhoneApplicationPage
    {
        RoadMapManager myRMM;
        RoadMap myRoadM;
        MeetingCollection m;
        MeetingManager myMM;
        Meeting MeetTmps;
        Meeting myDepart;
        Meeting myArriver;
        private TextBlock monTxt;
        ProgressIndicator indicator;
        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
        ReferenceMeeting meetRef;
        FlecheBasInfo flbI_InfoRdv;


        public PivotRoadTrip()
        {
            InitializeComponent();
            rt_ConsoTrajet.tb_Info1.Text = "Consomation";
            rt_CoutTot.tb_Info1.Text = "Coût total";
            rt_DistRdv.tb_Info1.Text = "Distance parcourue";
            rt_nbrRdv.tb_Info1.Text = "Nombres de RDVs";
            rt_TpsTrajetRdv.tb_Info1.Text = "Temps de trajet";
            myRMM = new RoadMapManager();
            myMM = new MeetingManager();
            myMM.LocationChecked += LocationChekedDepart;
            myMM.SearchStringAsync((string)settings["AdrDepart"]);
            indicator = new ProgressIndicator
            {
                IsVisible = true,
                IsIndeterminate = true,
                Text = "Chargement..."
            };
           
          

            SystemTray.SetProgressIndicator(this, indicator);
        }

        private void RoadMapReceived(object o, RoadMapReceivedEventArgs e)
        {
            if (e.RoadMap != null)
            {
                m = e.RoadMap.Meetings;
                myRoadM = e.RoadMap;
            }
            InitGrid();
           

        }

        private void LocationChekedDepart(object o, LocationCheckedEventArgs e)
        {
            MeetTmps = e.Meeting;
            myDepart = new Meeting();
            myDepart.Location = MeetTmps.Location;
            myDepart.City = MeetTmps.City;
            myDepart.Subject = "Lieu de départ";

            myMM.SearchStringAsync((string)settings["AdrArriver"]);
            myMM.LocationChecked -= LocationChekedDepart;
            myMM.LocationChecked += LocationChekedArriver;
        }

        private void LocationChekedArriver(object o, LocationCheckedEventArgs e)
        {
            ReferenceMeeting myRefMeetDepart = new ReferenceMeeting((DateTime)settings["Date"], myDepart.Location);
            myRefMeetDepart.City = myDepart.City;
            myRefMeetDepart.Subject = myDepart.Subject;
            ReferenceMeeting myRefMeetArriver = new ReferenceMeeting((DateTime)settings["Date"], myDepart.Location);            
            MeetTmps = e.Meeting;
            myArriver = new Meeting();
            myArriver.Location = MeetTmps.Location;
            myArriver.City = MeetTmps.City;
            myRefMeetArriver.City = myDepart.City;
            myRefMeetArriver.Subject = myDepart.Subject;
            myRMM.RoadMapReceived += RoadMapReceived;
            myRMM.GetRoadMapAsync((DateTime)settings["Date"], myRefMeetDepart, myRefMeetArriver);
        }
        private void InitGrid()
        {
            Trip monTrip;
            double tpsTot=0;
            if (m != null)
            {
                for (int i = 0; i < m.Count; i++)
                {

                    monTrip =myRoadM.Trips.Where(o => o.Start.Equals(m[i])).FirstOrDefault();
                    if (monTrip != null  && i!= m.Count-1 && !m[i].IsLocationFail)
                    {
                        if (i == 0)
                        {
                            gr_InfoRD.RowDefinitions.Add(new RowDefinition());
                            flbI_InfoRdv = new FlecheBasInfo();
                            flbI_InfoRdv.SujetRdv = "Lieu de départ";
                            flbI_InfoRdv.AdrRdv = m[i].Adress;
                            flbI_InfoRdv.Distance = monTrip.Distance + " km";
                            flbI_InfoRdv.TpsTrajet = monTrip.Duration + " min";
                            tpsTot = tpsTot + monTrip.Duration;
                            flbI_InfoRdv.ConSomation = Math.Round(((monTrip.Distance * (double)settings["ConsoCarburant"]) / 100), 2) + " L";
                            flbI_InfoRdv.Cout =Math.Round((((monTrip.Distance * (double)settings["ConsoCarburant"]) / 100) * (double)settings["PrixCarburant"]),2) + " €";
                            flbI_InfoRdv.InitInfo();
                            flbI_InfoRdv.SetValue(Grid.RowProperty, i);
                            gr_InfoRD.Children.Add(flbI_InfoRdv);
                            continue;
                        }

                        gr_InfoRD.RowDefinitions.Add(new RowDefinition());
                        flbI_InfoRdv = new FlecheBasInfo();
                        flbI_InfoRdv.SujetRdv = m[i].Subject;
                        flbI_InfoRdv.AdrRdv = m[i].Adress;
                        flbI_InfoRdv.Distance = monTrip.Distance + " km";
                        flbI_InfoRdv.TpsTrajet = monTrip.Duration + " min";
                        tpsTot = tpsTot + monTrip.Duration;
                        flbI_InfoRdv.ConSomation = Math.Round(((monTrip.Distance * (double)settings["ConsoCarburant"])/100),2) + " L";
                        flbI_InfoRdv.Cout = Math.Round((((monTrip.Distance * (double)settings["ConsoCarburant"]) / 100) * (double)settings["PrixCarburant"]), 2) + " €";
                        flbI_InfoRdv.InitInfo();
                        
                        flbI_InfoRdv.SetValue(Grid.RowProperty, i);
                        //monTxt.SetValue(Grid.ColumnProperty, 0);
                        //monTxt.Text = 


                        if (m[i].IsLocationFail)
                        {
                            // monTxt.Foreground = new SolidColorBrush(Colors.Red);
                        }
                        else
                        {
                            // monTxt.Foreground = new SolidColorBrush(Colors.Green);
                        }
                        gr_InfoRD.Children.Add(flbI_InfoRdv);

                        //monTxt = new TextBlock();
                        //monTxt.SetValue(Grid.RowProperty,i);
                        //monTxt.SetValue(Grid.ColumnProperty, 1);
                        //monTxt.Text = m[i].City;
                        //if (i != 0)
                        //{
                        //    if (m[i].IsLocationFail)
                        //    {
                        //        monTxt.Foreground = new SolidColorBrush(Colors.Red);
                        //        monTxt.Text = "Lieu inconnu";
                        //    }
                        //    else
                        //    {
                        //        monTxt.Foreground = new SolidColorBrush(Colors.Green);

                        //    }
                        //    gridPinc.Children.Add(monTxt);
                        //}
                    }
                    if (i == m.Count - 1)
                    {
                        var monTxt = new TextBlock();
                        gr_InfoRD.RowDefinitions.Add(new RowDefinition());
                        monTxt.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                        monTxt.SetValue(Grid.RowProperty, i);
                        monTxt.Text = "Arrivée";
                        monTxt.FontSize = 25;
                        gr_InfoRD.Children.Add(monTxt);
                        monTxt = new TextBlock();
                        gr_InfoRD.RowDefinitions.Add(new RowDefinition());
                        monTxt.SetValue(Grid.RowProperty, i+1);
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
            indicator.IsIndeterminate = false;
            indicator.IsVisible = false;
        }
    }
}