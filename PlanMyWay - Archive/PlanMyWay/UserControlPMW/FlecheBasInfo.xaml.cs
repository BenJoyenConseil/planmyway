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

namespace PlanMyWay.UserControlPMW
{
    public partial class FlecheBasInfo : UserControl
    {
        Storyboard sbAnimeFleche;
        Duration duration;
        DoubleAnimation myDoubleAnimation1;
        DoubleAnimation myDoubleAnimation2;
        DoubleAnimation myDoubleAnimation3;
        DoubleAnimation myDoubleAnimation4;
        public bool _EnabledAnimation = true;
        private bool _IsTripFail = false;

        public bool IsTripFail1
        {
            get { return _IsTripFail; }
            set { _IsTripFail = value; }
        }
        private string _distance;

        public string Distance
        {
            get { return _distance; }
            set { _distance = value; }
        }
        private string _tpsTrajet;

        public string TpsTrajet
        {
            get { return _tpsTrajet; }
            set { _tpsTrajet = value; }
        }
        private string _conSomation;

        public string ConSomation
        {
            get { return _conSomation; }
            set { _conSomation = value; }
        }
        private string _cout;

        public string Cout
        {
            get { return _cout; }
            set { _cout = value; }
        }
        private string _sujetRdv;

        public string SujetRdv
        {
            get { return _sujetRdv; }
            set { _sujetRdv = value; }
        }
        private string _adrRdv;

        public string AdrRdv
        {
            get { return _adrRdv; }
            set { _adrRdv = value; }
        }
        bool IsAnimeEnd = false;

        public FlecheBasInfo()
        {
            InitializeComponent();
            InfoSupDisable();
            sbAnimeFleche = new Storyboard();
            duration = new Duration(TimeSpan.FromSeconds(0.25));
            myDoubleAnimation1 = new DoubleAnimation();
            myDoubleAnimation2 = new DoubleAnimation();
            myDoubleAnimation3 = new DoubleAnimation();
            myDoubleAnimation4 = new DoubleAnimation();
          
            sbAnimeFleche.Children.Add(myDoubleAnimation1);
            sbAnimeFleche.Children.Add(myDoubleAnimation2);
            sbAnimeFleche.Children.Add(myDoubleAnimation3);
            sbAnimeFleche.Children.Add(myDoubleAnimation4);
            myDoubleAnimation1.Duration = duration;
            myDoubleAnimation2.Duration = duration;
            myDoubleAnimation3.Duration = duration;
            myDoubleAnimation4.Duration = duration;
            Storyboard.SetTarget(myDoubleAnimation1, MyLine);
            Storyboard.SetTarget(myDoubleAnimation2, maFleche);
            Storyboard.SetTarget(myDoubleAnimation3, this);
            Storyboard.SetTarget(myDoubleAnimation4, LayoutRoot);
            Storyboard.SetTargetProperty(myDoubleAnimation1, new PropertyPath("(Y2)"));
            Storyboard.SetTargetProperty(myDoubleAnimation2, new PropertyPath("(Canvas.Top)"));
            Storyboard.SetTargetProperty(myDoubleAnimation3, new PropertyPath("(Height)"));
            Storyboard.SetTargetProperty(myDoubleAnimation4, new PropertyPath("(Height)"));
            myDoubleAnimation1.From = 50;
            myDoubleAnimation2.From = 50;
           
            myDoubleAnimation1.To =130;
            myDoubleAnimation2.To =109;
            myDoubleAnimation3.To = 250;
            myDoubleAnimation4.To = 250;
            LayoutRoot.Resources.Add("unique_id", sbAnimeFleche);
           

            //sbAnimeFleche.Begin();
            

        }

        private void MyLine_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            //sbAnimeFleche.Begin();
           
        }

        private void MyLine_GotFocus(object sender, RoutedEventArgs e)
        {
            //sbAnimeFleche.Begin();
            
        }

        private void tb_info1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void ContentPanelCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_EnabledAnimation)
            {
                ManageAnimation();
            }
        }

        private void ManageAnimation()
        {

            if (!IsAnimeEnd)
            {
                sbAnimeFleche.Begin();
                InfoSupEnabled();
            }
            else
            {
                MyLine.Y2 = 51;
                Canvas.SetTop(maFleche, 51);
                this.Height = 196;
                ContentPanelCanvas.Height = 150;
                InfoSupDisable();
            }
            IsAnimeEnd = !IsAnimeEnd;
        }
        public void InitInfo()
        {
            tb_SubjectRdv.Text = this.SujetRdv;
            tb_AdrRdv.Text = this.AdrRdv;
            tb_ConsoInfo.Text = this.ConSomation;
            tb_CoutInfo.Text = this.Cout;
            tb_DistanceInfo.Text = this.Distance;
            tb_TpsTrajetInfo.Text = this.TpsTrajet;
            if (_IsTripFail)
            {
                MyLine.Stroke = new SolidColorBrush(Colors.Red);
                maFleche.Stroke = new SolidColorBrush(Colors.Red);
            }
            else
            {
                MyLine.Stroke = new SolidColorBrush(Colors.Green);
                maFleche.Stroke = new SolidColorBrush(Colors.Green);
            }
        }
        private void InfoSupDisable()
        {
            tb_Conso.Visibility = Visibility.Collapsed;
            tb_ConsoInfo.Visibility = Visibility.Collapsed;
            tb_Cout.Visibility = Visibility.Collapsed;
            tb_CoutInfo.Visibility = Visibility.Collapsed;
            tb_Distance.Visibility = Visibility.Collapsed;
            tb_DistanceInfo.Visibility = Visibility.Collapsed;
            tb_TpsTrajet.Visibility = Visibility.Collapsed;
            tb_TpsTrajetInfo.Visibility = Visibility.Collapsed;
        }

        private void InfoSupEnabled()
        {
            tb_Conso.Visibility = Visibility.Visible;
            tb_ConsoInfo.Visibility = Visibility.Visible;
            tb_Cout.Visibility = Visibility.Visible;
            tb_CoutInfo.Visibility = Visibility.Visible;
            tb_Distance.Visibility = Visibility.Visible;
            tb_DistanceInfo.Visibility = Visibility.Visible;
            tb_TpsTrajet.Visibility = Visibility.Visible;
            tb_TpsTrajetInfo.Visibility = Visibility.Visible;
        }

    }
}
