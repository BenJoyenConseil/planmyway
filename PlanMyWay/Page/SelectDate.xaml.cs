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
    public partial class SelectDate : PhoneApplicationPage
    {
        public SelectDate()
        {
            InitializeComponent();
        }

        private void btn_Generer_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Page/DetailCsv.xaml", UriKind.Relative));
        }
    }
}