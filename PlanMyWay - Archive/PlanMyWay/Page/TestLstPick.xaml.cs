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

namespace PlanMyWay.Page
{
    public partial class TestLstPick : PhoneApplicationPage
    {
        public TestLstPick()
        {
            InitializeComponent();
            this.lpkCountry.ItemsSource = Country;
        }
        String[] Country = { "Viet Nam","Japan",

                              "China","USA",

                              "Poland","Brazil",

                              "Singapore","Philipin","Malaysia"};


        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {

            String _Content = String.Format("Name: {0} \nAges: {1}\nCountry: {2}",

                txtName.Text, txtAge.Text, lpkCountry.SelectedItem);

            MessageBox.Show(_Content);

        }
    }
}