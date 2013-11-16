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
using Microsoft.Live;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using PlanMyWay.Html;
using System.Text;
using PlanMyWay.Lib.Util;
using PlanMyWay.Lib.DataModel;
using PlanMyWay.Lib.Manager;
using Microsoft.Phone.Controls.Maps.Platform;

namespace PlanMyWay.Lib.Test
{
    public partial class PivotPage1 : PhoneApplicationPage
    {
        IsolatedStorageFile myIsolatedStorage;
        IsolatedStorageFileStream fileStream;
        StreamReader reader;
        IRoadMapManager manager = new RoadMapManager();
        static ReferenceMeeting startSetting = new ReferenceMeeting() { City = "Paris", Location = new Location() { Latitude = 48.85693, Longitude = 2.3412 } };
        static ReferenceMeeting endSetting = startSetting;
        DateAndPositionsCollection collection = new DateAndPositionsCollection();

        public PivotPage1()
        {
            InitializeComponent();
            this.lst_dateAndpositions.DataContext = collection;
        }

        private void signInButton_SessionChanged_1(object sender, Microsoft.Live.Controls.LiveConnectSessionChangedEventArgs e)
        {
            if (e.Status != LiveConnectSessionStatus.Connected)
                return;


            manager.AllRoadMapsReceived += (o, eventAllRoadMaps) =>
            {
                if (eventAllRoadMaps.RoadMaps.Count  == 0)
                {
                    Debug.WriteLine("Aucun rendez-vous n'existe pour les jours sélectionnés");
                    return;
                }

                try
                {
                    //SaveTableur();
                    SpreadSheetRoadmapGenerator.GenerateXLS("feuilles-de-route.xlsx", eventAllRoadMaps.RoadMaps, 5.5f, 1.6f);
                    if (e.Session != null && e.Status == LiveConnectSessionStatus.Connected)
                    {
                        myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();
                        fileStream = myIsolatedStorage.OpenFile("feuilles-de-route.xlsx", FileMode.Open, FileAccess.Read);
                        reader = new StreamReader(fileStream);
                        App.Session = e.Session;
                        LiveConnectClient client = new LiveConnectClient(e.Session);
                        client.UploadCompleted += client_UploadCompleted;
                        client.UploadAsync("me/skydrive", "feuilles-de-route.xlsx", reader.BaseStream, OverwriteOption.Overwrite);
                    }
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.Message);
                }
            };
            ReferenceMeeting start = new ReferenceMeeting(new DateTime(2013, 1, 2, 8, 30, 0), new Location()
            {
                Latitude = 48.85693,
                Longitude = 2.3412
            }) { City = "Paris", Subject = "Start" };
            ReferenceMeeting end = start;
            end.Subject = "End";

            manager.GetAllRoadMapsAsync(new DateTime(2013, 1, 2), new DateTime(2013, 2, 10), start, end);

        }

        private void saveFile()
        {
            Element htmlFile = new Element();

            htmlFile.Tag = "html";
            Element head = new Element("head");
            head.Add(new Element("title") { Text = "Test html title" });
            Element body = new Element("body");
            body.Add(new Element("p") { Text = "<span color=\"Red\">Salut comment ça va??</span>" });
            htmlFile.Add(head);
            htmlFile.Add(body);
            try
            {
                using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
                using (Stream writeFile = new IsolatedStorageFileStream("rapport-PlanMyWay.xls", FileMode.Create, file))
                {
                    //writeFile.Write(htmlFile.ToString());
                    //writeFile.Flush();
                    //writeFile.Close();
                    //file.Dispose();
                    //writeFile.Dispose();

                    ExcelWriter writer = new ExcelWriter(writeFile);
                    writer.BeginWrite();
                    writer.WriteCell(0, 0, "ExcelWriter Demo");
                    writer.WriteCell(1, 0, "int");
                    writer.WriteCell(1, 1, 10);
                    writer.WriteCell(2, 0, "double");
                    writer.WriteCell(2, 1, 1.5);
                    writer.WriteCell(3, 0, "empty");
                    writer.WriteCell(3, 1);
                    writer.EndWrite();
                    writeFile.Close();
                }
            }
            catch (Exception e)
            {
            }
        }


        void client_UploadCompleted(object sender, LiveOperationCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Debug.WriteLine(e.Error.Message);
                return;
            }
            Debug.WriteLine("Upload File completed!");
            IDictionary<string, object> file = e.Result;
            String link = file["source"] as string;
            myIsolatedStorage.Dispose();
            fileStream.Dispose();
            reader.Dispose();
            Microsoft.Phone.Tasks.EmailComposeTask emailTask = new Microsoft.Phone.Tasks.EmailComposeTask();
            emailTask.Body = link;
            emailTask.Show();
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            manager.AllRoadMapsReceived += (o, eRoadMapReceived) =>
            {
                var l = new List<DateAndPositions>();
                foreach (var roadmap in eRoadMapReceived.RoadMaps)
                {
                    l.Add(new DateAndPositions(roadmap.Date, roadmap.Meetings.FirstOrDefault(), roadmap.Meetings.LastOrDefault()));
                }
                collection.Items = l;
                this.progressBar.IsIndeterminate = false;
                if (eRoadMapReceived.Error)
                    MessageBox.Show("une erreur est survenue");
            };
            manager.GetAllRoadMapsAsync(this.dateFrom.Value.Value, this.dateTo.Value.Value, startSetting, endSetting);
            this.progressBar.IsIndeterminate = true;
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void ApplicationBarIconButton_Click_1(object sender, System.EventArgs e)
        {
        }

    }
}