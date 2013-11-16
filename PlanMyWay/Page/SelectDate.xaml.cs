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
using PlanMyWay.Lib.Util;
using PlanMyWay.Lib.DataModel;
using Microsoft.Phone.Controls.Maps.Platform;
using Microsoft.Live;
using System.IO.IsolatedStorage;
using System.IO;
using System.Diagnostics;
using Microsoft.Phone.Shell;
using Coding4Fun.Toolkit.Controls;
using System.Collections.ObjectModel;
using PlanMyWay.Lib.Manager.EventArgs;

namespace PlanMyWay.Lib.Test
{
    public partial class PanoramaPage1 : PhoneApplicationPage
    {
        DateAndPositionsCollection collection = new DateAndPositionsCollection();
        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
        IsolatedStorageFile myIsolatedStorage;
        IsolatedStorageFileStream fileStream;
        StreamReader reader; 
        IRoadMapManager manager;

        public PanoramaPage1()
        {
            InitializeComponent();
            this.lst_results.DataContext = collection;
#if DEBUG
            DateAndPositionsCollectionDesign temp = new DateAndPositionsCollectionDesign();
            collection.Items = temp.Items;
#endif
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);
        }

        /// <summary>
        /// Selectionne les roadmaps en fonction d'un intervalle de temps
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_selectRoadMaps_Click(object sender, RoutedEventArgs e)
        {
            if (!App.IsConnected())
            {
                MessageBox.Show("Vous n'êtes pas connecté à internet");
                return;
            }

            collection.Items.Clear();
            IRoadMapManager manager = new RoadMapManager();
            manager.AllRoadMapsReceived += (o, eRoadMapReceived) =>
            {
                if (eRoadMapReceived.Error)
                {
                    MessageBox.Show(eRoadMapReceived.MessageError);
                    this.enableInterface();
                    return;
                }
                for (int i=0; i < eRoadMapReceived.RoadMaps.Count; i++)
                {
                    var roadmap = eRoadMapReceived.RoadMaps[i];
                    collection.Items.Add(new DateAndPositions(i, roadmap));
                }
                manager.AllRoadMapsReceived = null;
                this.enableInterface();

                ToastPrompt toast = new ToastPrompt();
                toast.Title = "Résultats";
                toast.Message = eRoadMapReceived.RoadMaps.Count + " feuille(s) de route récupérée(s)";
                toast.Show();
            };
            if (!IsolatedStorageSettings.ApplicationSettings.Contains("latiAdrDepart") ||
                !IsolatedStorageSettings.ApplicationSettings.Contains("longiAdrDepart")||
                !IsolatedStorageSettings.ApplicationSettings.Contains("VilleDepart") ||
                !IsolatedStorageSettings.ApplicationSettings.Contains("latiAdrArriver")||
                !IsolatedStorageSettings.ApplicationSettings.Contains("longiAdrArriver")||
                !IsolatedStorageSettings.ApplicationSettings.Contains("VilleArriver") ||
                !IsolatedStorageSettings.ApplicationSettings.Contains("ConsoCarburant")||
                !IsolatedStorageSettings.ApplicationSettings.Contains("PrixCarburant"))
            {
                MessageBox.Show("Veillez à paramètrer correctement l'application avant d'utiliser l'export Excel. Les villes de départ et d'arrivée sont manquantes");
                NavigationService.Navigate(new Uri("/Page/PivotParam.xaml", UriKind.Relative));
                return;
            }

            ReferenceMeeting start = new ReferenceMeeting(this.dateFrom.Value.Value, new Location()
            {
                Latitude = (double)settings["latiAdrDepart"],
                Longitude = (double)settings["longiAdrDepart"]
            }) { City = (string)settings["VilleDepart"], Subject = "Start" };
            ReferenceMeeting end = new ReferenceMeeting(this.dateFrom.Value.Value, new Location()
            {
                Latitude = (double)settings["latiAdrArriver"],
                Longitude = (double)settings["longiAdrArriver"]
            }) { City = (string)settings["VilleArriver"], Subject = "End" };

            this.disableInterface();
            manager.GetAllRoadMapsAsync(this.dateFrom.Value.Value, this.dateTo.Value.Value, start, end);
        }

        /// <summary>
        /// Demande à l'utilisateur de s'autentifier
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_export_Click(object sender, EventArgs e)
        {
            if (!App.IsConnected())
            {
                MessageBox.Show("Vous n'êtes pas connecté à internet");
                return;
            }

            this.disableInterface();
            try
            {
                int count = collection.Items.Where(item => item.IsEdited == true).Count();
                if (count > 0)
                {
                    this.manager = new RoadMapManager();
                    this.manager.AllRoadMapsReceived += OnAllRoadMapsReceived;
                    Dictionary<DateTime, KeyValuePair<ReferenceMeeting,ReferenceMeeting>> refMeetingsMap = new Dictionary<DateTime, KeyValuePair<ReferenceMeeting,ReferenceMeeting>>();
                    foreach (var item in collection.Items)
                    {
                        refMeetingsMap.Add(item.Roadmap.Date, 
                            new KeyValuePair<ReferenceMeeting, ReferenceMeeting>(
                                (item.Start as ReferenceMeeting), 
                                (item.End as ReferenceMeeting)
                            ));
                    }
                    this.manager.GetAllRoadMapsAsync(refMeetingsMap);
                }
                else
                {
                    LiveAuthClient auth = new LiveAuthClient("000000004C0E007C");
                    auth.LoginCompleted += auth_LoginCompleted;
                    auth.LoginAsync(new string[] { "wl.signin", "wl.skydrive_update" });
                }
            }
            catch (LiveAuthException exception)
            {
                MessageBox.Show(exception.Message);
                Debug.WriteLine(exception.Message);
                this.enableInterface();
            }
        }

        private void OnAllRoadMapsReceived(object o, AllRoadMapReceivedEventArgs eAllRoadMapsReceived)
        {
            if (eAllRoadMapsReceived.Error)
            {
                MessageBox.Show(eAllRoadMapsReceived.MessageError);
                this.enableInterface();
                return;
            }
            collection.Items.Clear();
            for (int i = 0; i < eAllRoadMapsReceived.RoadMaps.Count; i++)
            {
                var roadmap = eAllRoadMapsReceived.RoadMaps[i];
                collection.Items.Add(new DateAndPositions(i, roadmap));
            }

            LiveAuthClient auth = new LiveAuthClient("000000004C0E007C");
            auth.LoginCompleted += auth_LoginCompleted;
            auth.LoginAsync(new string[] { "wl.signin", "wl.skydrive_update" });
            this.manager.AllRoadMapsReceived = null;
            this.manager = null;
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
                foreach (var item in collection.Items)
                {
                    _roadmaps.Add(item.Roadmap);
                }

                string filename = "rapport-planmyway-" + DateTime.Now.Date.ToShortDateString().Replace("/","-") + ".xlsx";
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
                    this.disableInterface();
                    client.UploadAsync("me/skydrive", filename, reader.BaseStream, OverwriteOption.Overwrite);

                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
                MessageBox.Show(exception.Message);
                this.enableInterface();
            }
        }

        /// <summary>
        /// Une fois le transfère terminé, envoie d'un email
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void client_UploadCompleted(object sender, LiveOperationCompletedEventArgs e)
        {
            this.enableInterface();
            if (e.Error != null)
            {
                Debug.WriteLine(e.Error.Message);
                return;
            }
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
            emailTask.Subject = "PlanMyWay - Feuille de frais du " + this.dateFrom.Value.Value.ToShortDateString() + " au " + this.dateTo.Value.Value.ToShortDateString();
            emailTask.Show();

            ToastPrompt toast = new ToastPrompt();
            toast.Title = "Export Excel terminé";
            toast.Message = "Votre feuille de frais est disponible sur votre skydrive";
            toast.Show();
        }

        /// <summary>
        /// Rafraichir l'interface
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_refresh_Click(object sender, EventArgs e)
        {
            var er = new RoutedEventArgs();
            btn_selectRoadMaps_Click(sender, er);
        }

        /// <summary>
        /// Appel des settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_settings_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Page/PivotParam.xaml", UriKind.Relative));
        }

        private void disableInterface()
        {
            ApplicationBarIconButton settings = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
            ApplicationBarIconButton refresh = ApplicationBar.Buttons[1] as ApplicationBarIconButton;
            ApplicationBarIconButton export = ApplicationBar.Buttons[2] as ApplicationBarIconButton;
            this.progressBar.IsIndeterminate = true;
            this.progressBar.IsVisible = true;
            dateFrom.IsEnabled = false;
            dateTo.IsEnabled = false;
            btn_selectRoadMaps.IsEnabled = false;
            this.lst_results.IsEnabled = false;
            settings.IsEnabled = false;
            refresh.IsEnabled = false;
            export.IsEnabled = false;
        }

        private void enableInterface()
        {
            ApplicationBarIconButton settings = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
            ApplicationBarIconButton refresh = ApplicationBar.Buttons[1] as ApplicationBarIconButton;
            ApplicationBarIconButton export = ApplicationBar.Buttons[2] as ApplicationBarIconButton;
            this.progressBar.IsIndeterminate = false;
            this.progressBar.IsVisible = false;
            dateFrom.IsEnabled = true;
            dateTo.IsEnabled = true;
            this.lst_results.IsEnabled = true;
            settings.IsEnabled = true;
            refresh.IsEnabled = true;
            if(collection.Items.Count > 0)
                export.IsEnabled = true;
            btn_selectRoadMaps.IsEnabled = true;
        }
        
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (settings.Contains("selectedDateAndPositions")){
                var data = settings["selectedDateAndPositions"] as DateAndPositions;
                var item = lst_results.Items[data.Id] as DateAndPositions;
                item.Start.City = data.Start.City;
                item.End.City = data.End.City;
                item.NotifyPropertyChanged("End");
                item.NotifyPropertyChanged("Start");
                //item = data;
                settings.Remove("selectedDateAndPositions");
            }
            base.OnNavigatedTo(e);
        }

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            DateAndPositions data = (sender as Button).DataContext as DateAndPositions;
            ListBoxItem pressedItem = this.lst_results.ItemContainerGenerator.ContainerFromItem(data) as ListBoxItem;
            data.IsEdited = true;
            if (pressedItem != null)
            {
                if (!settings.Contains("selectedDateAndPositions"))
                    settings.Add("selectedDateAndPositions", data);
                else
                    settings["selectedDateAndPositions"] = data;
                this.NavigationService.Navigate(new Uri("/Page/DateAndPositionEdit.xaml", UriKind.Relative));
            }
        }

        //protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        //{
        //    if (this.manager == null)
        //        return;
        //    this.manager.IsCanceled = true;
        //    this.manager.AllRoadMapsReceived = null;
        //    this.manager.RoadMapReceived = null;
        //    base.OnNavigatingFrom(e);
        //}
    }
}