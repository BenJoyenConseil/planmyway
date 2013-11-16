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
using PlanMyWay.Design;
using Microsoft.Live;
using PlanMyWay.Lib.Util;
using System.IO;

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
        ApplicationBarIconButton btn_export;
        bool traitementEnd = false;

        TripFailCollectionDesign tripFailCollection = new TripFailCollectionDesign();
        LocationFailCollectionDesign locationFailCollection = new LocationFailCollectionDesign();
        IsolatedStorageFile myIsolatedStorage;
        IsolatedStorageFileStream fileStream;
        StreamReader reader;


        public PivotRoadTrip()
        {
            InitializeComponent();

            if (!App.IsConnected())
            {
                MessageBox.Show("Aucune connexion n'est détectée !");
                return;
            }

            btn_setting = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
            btn_refresh = ApplicationBar.Buttons[1] as ApplicationBarIconButton;
            btn_export = ApplicationBar.Buttons[2] as ApplicationBarIconButton;
            btn_refresh.IsEnabled = false;
            btn_setting.IsEnabled = false;
            btn_export.IsEnabled = false;
            initRoadTrip();
            this.lst_anomalieTemps.DataContext = tripFailCollection;
            this.lst_anomalieLieu.DataContext = locationFailCollection;
        }

        private void initRoadTrip()
        {
            if ((double)settings["latiAdrDepart"] != 0.0 &&
                (double)settings["longiAdrDepart"] != 0.0 &&
                (double)settings["latiAdrArriver"] != 0.0 &&
                (double)settings["longiAdrArriver"] != 0.0)
            {
                rt_ConsoTrajet.tb_Info1.Text = "Consomation";
                rt_CoutTot.tb_Info1.Text = "Coût total";
                rt_DistRdv.tb_Info1.Text = "Distance parcourue";
                rt_nbrRdv.tb_Info1.Text = "Nombres de RDVs";
                rt_TpsTrajetRdv.tb_Info1.Text = "Temps de trajet";

                //gr_AnomalieLieu.Children.Clear();
                //gr_AnomalieTemps.Children.Clear();
                locationFailCollection.Items.Clear();
                tripFailCollection.Items.Clear();

                gr_InfoRD.Children.Clear();
                gr_InfoRDOpti.Children.Clear();
                map_Rdv.Children.Clear();
                myRMM = new RoadMapManager();
                myMM = new MeetingManager();
                tripTimeFail = new List<Trip>();
                meetingLocationFail = new List<Meeting>();

                //Récupération du lieu de départ
                var myRefMeetDepart = new ReferenceMeeting();
                myRefMeetDepart.Address = (string)settings["AdrDepart"];
                myRefMeetDepart.City = (string)settings["VilleDepart"];
                myRefMeetDepart.Location = new Location() { Latitude = (double)settings["latiAdrDepart"], Longitude = (double)settings["longiAdrDepart"] };

                //Récupération du lieu d'arrivée
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
                MessageBox.Show("Veuillez vérifier vos lieux de départ et d'arrivée dans les paramètres.");
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
                            //flbI_InfoRdv.Margin = new Thickness(1, 1, 1, 1);
                            flbI_InfoRdv.SetValue(Grid.RowProperty, i);
                            flbI_InfoRdv.Margin = new Thickness(1, 5, 1, 5);
                            gr_InfoRD.Children.Add(flbI_InfoRdv);
                            continue;
                        }
                        tmpRow = new RowDefinition();
                        tmpRow.Height = GridLength.Auto;
                        gr_InfoRD.RowDefinitions.Add(tmpRow);
                        flbI_InfoRdv = new FlecheBasInfo();
                        flbI_InfoRdv.SujetRdv = m[i].Subject+" ("+m[i].DateTime.ToShortTimeString()+")";
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
                        monTxt.FontSize = 18.75;
                        monTxt.FontFamily = new System.Windows.Media.FontFamily("Segoe UI Light");
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
            //tripFailCollection.Items.Clear();
            //locationFailCollection.Items.Clear();
            //RectAnomalie tmp;
            //RowDefinition tmpRow;
            //int compteurLigne = 1;
            for (int i = 0; i < tripTimeFail.Count; i++)
            {
                
                    try
                    {
                        tripFailCollection.Items.Add(new TripFailDesign(tripTimeFail[i]));
                        //tmpRow = new RowDefinition();
                        //tmpRow.Height = GridLength.Auto;
                        //gr_AnomalieTemps.RowDefinitions.Add(tmpRow);
                        //tmp = new RectAnomalie();
                        //tmp.NomRdv = tripTimeFail[i].Start.Subject + " => " + tripTimeFail[i].End.Subject;
                        //tmp.Distance_AdrRdv = tripTimeFail[i].Distance + " km";
                        //tmp.TpsPrevu = tripTimeFail[i].Duration + " min ";
                        //tmp.InitInfo();
                        //tmp.Height = 50;
                        //tmp.Width = 400;
                        //tmp.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                        //tmp.Margin = new Thickness(1, 5, 1, 5);
                        //tmp.SetValue(Grid.RowProperty, compteurLigne);
                        //gr_AnomalieTemps.Children.Add(tmp);
                        //compteurLigne++;
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
                        locationFailCollection.Items.Add(new LocationFailDesign(meetingLocationFail[i]));
                        //tmpRow = new RowDefinition();
                        //tmpRow.Height = GridLength.Auto;
                        ////gr_AnomalieLieu.RowDefinitions.Add(tmpRow);
                        //tmp = new RectAnomalie();
                        //tmp.NomRdv = meetingLocationFail[i].Subject;
                        //tmp.Distance_AdrRdv = meetingLocationFail[i].Address;
                        //tmp.TpsPrevu = "=> Adresse invalide";
                        //tmp.InitInfo();
                        //tmp.Height = 50;
                        //tmp.Width = 400;
                        //tmp.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                        //tmp.Margin = new Thickness(1, 5, 1, 5);
                        //tmp.SetValue(Grid.RowProperty, compteurLigne);
                        ////gr_AnomalieLieu.Children.Add(tmp);
                        //compteurLigne++;
                    }
                    catch
                    {
                        MessageBox.Show("Erreur de recepetion de la map bing, Réesseyer");
                    }
            }
            initToast(tripTimeFail.Count + meetingLocationFail.Count);
        }

        static SolidColorBrush red = new SolidColorBrush(Colors.Red);
        static SolidColorBrush black = new SolidColorBrush(Colors.Black);
        static SolidColorBrush blue = new SolidColorBrush(Color.FromArgb(255, 27, 161, 226));
        private void OnRoadMapReceived(object o, PlanMyWay.Lib.Manager.EventArgs.RoadMapReceivedEventArgs e)
        {
            try
            {
                if (e.Error)
                {
                    MessageBox.Show(e.MessageError);
                    indicator.IsIndeterminate = false;
                    indicator.IsVisible = false;
                    btn_refresh.IsEnabled = true;
                    btn_setting.IsEnabled = true;
                    btn_export.IsEnabled = true;
                    traitementEnd = true;
                    this.NavigationService.GoBack();
                    return;
                }

                myRoadM = e.RoadMap;
                map_Rdv.SetView(e.RoadMap.RouteResult.Summary.BoundingRectangle);
                var strokeThickness = e.RoadMap.Trips.Count * 2 + 1;
                int endRdv = 0;
                foreach (var portion in e.RoadMap.Trips)
                {
                    endRdv++;
                    //Random rand = new Random();
                    //byte r = (byte)rand.Next(0, 255);
                    //byte g = (byte)rand.Next(0, 255);
                    //byte b = (byte)rand.Next(0, 255);
                    MapPolyline polyline = new MapPolyline();
                    polyline.StrokeThickness = strokeThickness;
                    polyline.Opacity = 1;
                    polyline.Locations = new LocationCollection();
                    foreach (var point in portion.RoutePath)
                        polyline.Locations.Add(point);
                    polyline.Stroke = PivotRoadTrip.blue;
                    
                    if (portion.IsTripFail)
                    {
                        polyline.StrokeThickness += 2;
                        polyline.Stroke = PivotRoadTrip.red;
                    }
                    else if (endRdv == e.RoadMap.ValidMeetings.Count - 1)
                    {
                        polyline.Stroke = PivotRoadTrip.black;
                    }
                    strokeThickness -= 2;
                    map_Rdv.Children.Add(polyline);
                }

                //Création des points sur la carte correspondant au rendez-vous
                int counRdv = 0;
                foreach (var rdv in e.RoadMap.Meetings)
                {
                    
                    if (rdv.IsLocationFail)
                        continue;
                    Pushpin p = new Pushpin();
                    if (counRdv == 0 && rdv.GetType() == typeof(ReferenceMeeting))
                    {
                        p.Content = "Départ";
                    }
                    else if (rdv.GetType() == typeof(ReferenceMeeting))
                    {
                        p.Content = "Arrivée";
                    }
                    else
                    {
                        p.Content = "RDV" + (counRdv) + " : " + rdv.Subject;
                       
                    }

                    p.Background = new SolidColorBrush(Colors.Black);
                    p.Location = rdv.Location;
                    map_Rdv.Children.Add(p);
                    counRdv++;
                }
                if (e.RoadMap != null)
                {
                    m = e.RoadMap.ValidMeetings;
                    meetingLocationFail = e.RoadMap.LocationFailMeetings;
                    tripTimeFail = e.RoadMap.FailTrips;
                    myRoadM = e.RoadMap;
                }
                myRMM.RoadMapReceived = null;
                InitGrid();
                InitAnomlie();
                btn_refresh.IsEnabled = true;
                btn_setting.IsEnabled = true;
                btn_export.IsEnabled = true;
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
                toast.Background = new SolidColorBrush(Colors.Red);
                toast.TextOrientation = System.Windows.Controls.Orientation.Vertical;
                //toast.Completed += toast_Completed;
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
                btn_export.IsEnabled = false;
                traitementEnd = false;
                initRoadTrip();
            }
        }

        private void btn_export_Click(object sender, EventArgs e)
        {
            if (!App.IsConnected())
            {
                MessageBox.Show("Aucune connexion internet n'est détectée.");
                return;
            }
            if (myRoadM == null)
            {
                return;
            }

            try
            {
                LiveAuthClient auth = new LiveAuthClient("000000004C0E007C");
                auth.LoginCompleted += auth_LoginCompleted;
                auth.LoginAsync(new string[] { "wl.signin", "wl.skydrive_update" });
            }
            catch (LiveAuthException exception)
            {
                MessageBox.Show(exception.Message);
                Debug.WriteLine(exception.Message);
            }
        }

        /// <summary>
        /// Appel une fois que l'utilisateur est connecté à Skydrive
        /// Génère et Upload le fichier XLS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void auth_LoginCompleted(object sender, LoginCompletedEventArgs e)
        {
            if (!App.IsConnected())
            {
                MessageBox.Show("Vous n'êtes pas connecté à internet");
                return;
            }

            try
            {
                List<RoadMap> _roadmaps = new List<RoadMap>();
                _roadmaps.Add(this.myRoadM);

                string filename = "rapport-planmyway-" + myRoadM.Date.ToShortDateString().Replace("/", "-") + ".xlsx";
                Debug.WriteLine(filename);
                //SaveTableur();
                SpreadSheetRoadmapGenerator.GenerateXLS(filename, _roadmaps, (double)settings["ConsoCarburant"], (double)settings["PrixCarburant"]);
                if (e.Session != null && e.Status == LiveConnectSessionStatus.Connected)
                {
                    myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();
                    fileStream = myIsolatedStorage.OpenFile(filename, FileMode.Open, FileAccess.Read);
                    reader = new StreamReader(fileStream);
                    App.Session = e.Session;
                    LiveConnectClient client = new LiveConnectClient(e.Session);
                    client.UploadCompleted += client_UploadCompleted;
                    client.UploadAsync("me/skydrive", filename, reader.BaseStream, OverwriteOption.Overwrite);

                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
                MessageBox.Show(exception.Message);
                //this.enableInterface();
            }
        }

        /// <summary>
        /// Une fois le transfère terminé, envoie d'un email
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void client_UploadCompleted(object sender, LiveOperationCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Debug.WriteLine(e.Error.Message);
                //this.enableInterface();
                return;
            }
            //this.enableInterface();
            Debug.WriteLine("Upload File completed!");
            myIsolatedStorage.Dispose();
            fileStream.Dispose();
            reader.Dispose();
            IDictionary<string, object> file = e.Result;
            String link = file["source"] as string;
            Microsoft.Phone.Tasks.EmailComposeTask emailTask = new Microsoft.Phone.Tasks.EmailComposeTask();
            string str = String.Format("Bonjour, \n\n Le document Excel que vous avez demandé et désormais disponible à l'adresse suivante : \n\n");
            str += String.Format("{0}\n\n ", link);
            str += String.Format(" Lors de l'ouverture de ce document certaines erreurs pourraient survenir. Il vous suffit de les accepter pour pouvoir ouvrir votre feuille de frais. \nL'application étant encore en phase de test, n'hésitez pas à nous faire des retours sur la page adéquat du Windows Market Place. \nToute l'équipe de PlanMyWay vous remercie de votre confiance. \n\nCordialement,");
            emailTask.Body = str;
            emailTask.Subject = "PlanMyWay - Feuille de frais du " + this.myRoadM.Date.ToShortDateString();
            emailTask.Show();

            MessageBox.Show("Export Excel terminé. Votre feuille de frais est disponible sur votre skydrive");
        }
    }
}
