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
using Microsoft.Phone.UserData;
using Microsoft.Phone.Shell;

namespace PlanMyWay.Page
{
    public partial class DetailCsv : PhoneApplicationPage
    {
        private List<Appointment> listAppointement;
        private ListBox lstAppointement;

        public DetailCsv()
        {
            InitializeComponent();
            listAppointement = new List<Appointment>();
            lstAppointement = new ListBox();
            initListAppointment();
           
        }

        private void initListAppointment()
        {
            Appointments aAppointment = new Appointments();

            aAppointment.SearchCompleted += new EventHandler<AppointmentsSearchEventArgs>(GetAppointments);

            DateTime starttime = DateTime.Now;
            starttime = starttime.Subtract(TimeSpan.FromDays(1));

            DateTime endtime = starttime.AddDays(10);

            int maxAppointment = 20;

            aAppointment.SearchAsync(starttime, endtime, maxAppointment,null);
        }

        void GetAppointments(object sender, AppointmentsSearchEventArgs e)
        {

            try
            {
                lstPickAppointement.DataContext = e.Results;
                lstPickAppointement.DisplayMemberPath = "Subject";

            }

            catch (System.Exception)
            {

                lstPickAppointement.Items.Add("No Appointments Found!!!");
            }

            //for (int i = 0; i < lstAppointment.Items.Count; i++)
            //{
            //    listAppointement.Add((Appointment)lstAppointment.Items[i]);
            //}

            //if (lstAppointment.Items.Any())
            //{

            //    txtAppointments.Text = "Below is the List of Appointments";

            //}

            //else
            //{

            //    txtAppointments.Text = "No Appointments Found!!!";

            //}
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            PhoneApplicationService.Current.State["Appointements"] = lstPickAppointement.SelectedItem;
            NavigationService.Navigate(new Uri("/Page/DetailsAppointement.xaml", UriKind.Relative));
          
        }
    }
}