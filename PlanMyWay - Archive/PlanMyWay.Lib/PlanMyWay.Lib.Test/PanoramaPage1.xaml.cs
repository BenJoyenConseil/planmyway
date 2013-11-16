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

namespace PlanMyWay.Lib.Test
{
    public partial class PanoramaPage1 : PhoneApplicationPage
    {
        DateAndPositionsCollection collection = new DateAndPositionsCollection();
        List<RoadMap> _roadmaps = null;


        public PanoramaPage1()
        {
            InitializeComponent();
            this.lst_results.DataContext = collection;
        }

        private void btn_selectRoadMaps_Click(object sender, RoutedEventArgs e)
        {
            IRoadMapManager manager = new RoadMapManager();
            ApplicationBarIconButton settings = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
            ApplicationBarIconButton refresh = ApplicationBar.Buttons[1] as ApplicationBarIconButton;
            ApplicationBarIconButton export = ApplicationBar.Buttons[2] as ApplicationBarIconButton;

            manager.AllRoadMapsReceived += (o, eRoadMapReceived) =>
            {
                var l = new List<DateAndPositions>();
                foreach (var roadmap in eRoadMapReceived.RoadMaps)
                {
                    l.Add(new DateAndPositions(roadmap.Date, roadmap.Meetings.FirstOrDefault(), roadmap.Meetings.LastOrDefault()));
                }
                this._roadmaps = eRoadMapReceived.RoadMaps;
                collection.Items = l;
                this.progressBar.IsIndeterminate = false;
                this.lbl_progressBar.Visibility = System.Windows.Visibility.Collapsed;
                dateFrom.IsEnabled = true;
                dateTo.IsEnabled = true;
                settings.IsEnabled = true;
                refresh.IsEnabled = true;
                export.IsEnabled = true;
                btn_selectRoadMaps.IsEnabled = true;
                if (eRoadMapReceived.Error)
                    MessageBox.Show("une erreur est survenue");
            };


            
            /// <summary>
            /// TODO : à remplacer par les meetings des settings
            /// </summary>
            ReferenceMeeting start = new ReferenceMeeting(new DateTime(2013, 1, 2, 8, 30, 0), new Location()
            {
                Latitude = 48.85693,
                Longitude = 2.3412
            }) { City = "Paris", Subject = "Start" };
            ReferenceMeeting end = start;
            end.Subject = "End";

            manager.GetAllRoadMapsAsync(this.dateFrom.Value.Value, this.dateTo.Value.Value, start, end);
            this.progressBar.IsIndeterminate = true;
            dateFrom.IsEnabled = false;
            dateTo.IsEnabled = false;
            btn_selectRoadMaps.IsEnabled = false;

            settings.IsEnabled = false;
            refresh.IsEnabled = false;
            export.IsEnabled = false;
            this.lbl_progressBar.Visibility = System.Windows.Visibility.Visible;
        }

        private void btn_export_Click(object sender, EventArgs e)
        {
            try
            {
                LiveAuthClient auth = new LiveAuthClient("000000004C0E007C");
                auth.LoginCompleted += auth_LoginCompleted;
                auth.LoginAsync(new string[] { "wl.signin", "wl.skydrive_update" });
                //LiveLoginResult loginResult = await auth.LoginAsync(new string[] { "wl.basic" }); wl.signin wl.skydrive_update

            }
            catch (LiveAuthException exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        void auth_LoginCompleted(object sender, LoginCompletedEventArgs e)
        {
            try
            {
                IsolatedStorageFile myIsolatedStorage;
                IsolatedStorageFileStream fileStream;
                StreamReader reader;
                //SaveTableur();
                SpreadSheetRoadmapGenerator.GenerateXLS("feuilles-de-route.xlsx", _roadmaps, 5.5f, 1.6f);
                if (e.Session != null && e.Status == LiveConnectSessionStatus.Connected)
                {
                    myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();
                    fileStream = myIsolatedStorage.OpenFile("feuilles-de-route.xlsx", FileMode.Open, FileAccess.Read);
                    reader = new StreamReader(fileStream);
                    App.Session = e.Session;
                    LiveConnectClient client = new LiveConnectClient(e.Session);
                    client.UploadCompleted += client_UploadCompleted;
                    client.UploadAsync("me/skydrive", "feuilles-de-route.xlsx", reader.BaseStream, OverwriteOption.Overwrite);

                    //myIsolatedStorage.Dispose();
                    //fileStream.Dispose();
                    //reader.Dispose();
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void client_UploadCompleted(object sender, LiveOperationCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Debug.WriteLine(e.Error.Message);
                return;
            }
            Debug.WriteLine("Upload File completed!");
            IDictionary<string, object> file = e.Result;
            String link = file["source"] as string;
            Microsoft.Phone.Tasks.EmailComposeTask emailTask = new Microsoft.Phone.Tasks.EmailComposeTask();
            emailTask.Body = link;
            emailTask.Show();
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            var er = new RoutedEventArgs();

            btn_selectRoadMaps_Click(sender, er);
        }
    }
}