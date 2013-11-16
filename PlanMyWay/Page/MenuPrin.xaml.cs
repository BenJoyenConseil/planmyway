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

namespace PlanMyWay
{
    public partial class MenuPrin : PhoneApplicationPage
    {
        public MenuPrin()
        {
            InitializeComponent();
        }

        private void btn_Parametre_Click(object sender, RoutedEventArgs e)
        {
           // NavigationService.Navigate(new Uri("/Pmw_Page/TestLstPick.xaml", UriKind.Relative));
            NavigationService.Navigate(new Uri("/Page/PivotParam.xaml", UriKind.Relative));
        }

        private void btn_EditionCsv_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Page/SelectDate.xaml", UriKind.Relative));
        }

        private void btn_MyWay_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Page/SelectDateRoadTrip.xaml", UriKind.Relative));
        }
    }
}