using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using PlanMyWay.Lib.Util;
using PlanMyWay.Lib.Manager;
using PlanMyWay.Lib.DataModel;
using Microsoft.Phone.Controls.Maps.Platform;
using PlanMyWay.Lib.Manager.EventArgs;
using System.Windows.Media;

namespace PlanMyWay.Page
{
    public partial class DateAndPositionEdit : PhoneApplicationPage
    {
        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
        IMeetingManager monMM = new MeetingManager();
        DateAndPositions data = null;
        int count = 0;

        public DateAndPositionEdit()
        {
            InitializeComponent();
            if (settings.Contains("selectedDateAndPositions"))
            {
                data = settings["selectedDateAndPositions"] as DateAndPositions;
                this.DataContext = data;
                txt_start.Text = data.Start.City;
                txt_end.Text = data.End.City;
            }
        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txt_start.Text) || String.IsNullOrEmpty(txt_end.Text))
            {
                MessageBox.Show("Un ou plusieurs champs sont vides");
                return;
            }

            monMM.SearchStringCompleted += searchCompleted;
            monMM.SearchStringAsync(txt_start.Text, 1);
            monMM.SearchStringAsync(txt_end.Text, 2);
            this.disableInterface();
        }

        private void searchCompleted(object sender, SearchStringCompletedEventArgs e)
        {
            count++;
            if (e.Error)
            {
                //afficher erreur
                monMM.SearchStringCompleted -= searchCompleted;
                MessageBox.Show("Adresse invalide");
                this.enableInterface();
                if ((int)e.UserData == 1)
                {
                    this.lbl_start.Foreground = new SolidColorBrush(Colors.Red);
                    this.txt_start.BorderBrush = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    this.lbl_end.Foreground = new SolidColorBrush(Colors.Red);
                    this.txt_end.BorderBrush = new SolidColorBrush(Colors.Red);
                }
                return;
            }
            if ((int)e.UserData == 1)
            {
                data.Start = new ReferenceMeeting(data.Start.DateTime, e.Location);
                data.Start.City = e.City;
                data.Start.Address = e.Address;
                txt_start.Text = e.Address;
            }
            else
            {
                data.End = new ReferenceMeeting(data.End.DateTime, e.Location);
                data.End.City = e.City;
                data.End.Address = e.Address;
                txt_end.Text = e.Address;
            }
            if (count >= 2)
            {
                settings["selectedDateAndPositions"] = data;
                this.NavigationService.GoBack();
                monMM.SearchStringCompleted = null;
                count = 0;
                this.enableInterface();
            }
        }

        private void ApplicationBarIconButton_Click_2(object sender, EventArgs e)
        {
            settings.Remove("selectedDateAndPositions");
            this.NavigationService.GoBack();
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            settings.Remove("selectedDateAndPositions");
            base.OnBackKeyPress(e);
        }

        private void disableInterface()
        {
            ApplicationBarIconButton valider = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
            ApplicationBarIconButton annuler = ApplicationBar.Buttons[1] as ApplicationBarIconButton;
            this.txt_start.IsEnabled = false;
            this.txt_end.IsEnabled = false;
            valider.IsEnabled = false;
            annuler.IsEnabled = false;
            this.progressBar.IsIndeterminate = true;
            this.progressBar.IsVisible = true;
        }

        private void enableInterface()
        {
            ApplicationBarIconButton valider = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
            ApplicationBarIconButton annuler = ApplicationBar.Buttons[1] as ApplicationBarIconButton;
            this.txt_start.IsEnabled = true;
            this.txt_end.IsEnabled = true;
            valider.IsEnabled = true;
            annuler.IsEnabled = true;
            this.progressBar.IsIndeterminate = false;
            this.progressBar.IsVisible = false;
        }
    }
}