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
    public partial class RectAnomalie : UserControl
    {
        Storyboard sbAnimeFleche;
        Duration duration;
        DoubleAnimation myDoubleAnimation1;
        DoubleAnimation myDoubleAnimation2;
        private bool _RecDeplier = false;
        public RectAnomalie()
        {
            InitializeComponent();
            sbAnimeFleche = new Storyboard();
            duration = new Duration(TimeSpan.FromSeconds(0.5));
            myDoubleAnimation1 = new DoubleAnimation();
            myDoubleAnimation2 = new DoubleAnimation();

            sbAnimeFleche.Children.Add(myDoubleAnimation1);
            sbAnimeFleche.Children.Add(myDoubleAnimation2);
            myDoubleAnimation1.Duration = duration;
            myDoubleAnimation2.Duration = duration;
            Storyboard.SetTarget(myDoubleAnimation1, LayoutRoot);
            Storyboard.SetTarget(myDoubleAnimation2, this);
            Storyboard.SetTargetProperty(myDoubleAnimation1, new PropertyPath("(Height)"));
            Storyboard.SetTargetProperty(myDoubleAnimation2, new PropertyPath("(Height)"));
            myDoubleAnimation1.From = 80;
            myDoubleAnimation2.From = 80;

            myDoubleAnimation1.To = 150;
            myDoubleAnimation2.To = 150;
            LayoutRoot.Resources.Add("unique_id", sbAnimeFleche);
        }

        private void LayoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_RecDeplier)
            {
                maFlecheBas.Visibility = System.Windows.Visibility.Collapsed;
                maFlecheHaut.Visibility = System.Windows.Visibility.Visible;
                sbAnimeFleche.Begin();
            }
            else
            {
                LayoutRoot.Height = 80;
                this.Height = 80;
                maFlecheBas.Visibility = System.Windows.Visibility.Visible;
                maFlecheHaut.Visibility = System.Windows.Visibility.Collapsed;
            }
            _RecDeplier = !_RecDeplier;
        }
    }
}
