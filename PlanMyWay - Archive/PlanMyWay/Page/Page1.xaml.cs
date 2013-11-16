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
using Coding4Fun.Phone.Controls;
using System.Windows.Media.Imaging;
using Coding4Fun.Toolkit.Controls;

namespace PlanMyWay.Page
{
    public partial class Page1 : PhoneApplicationPage
    {

        // Constructor
        public Page1()
        {
            InitializeComponent();
        }

        void toast_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            //add some code here
        }

        private void btnToastPrompt_Click(object sender, RoutedEventArgs e)
        {
            ToastPrompt toast = new ToastPrompt();

            toast.Title = "ToastPrompt";
            toast.Message = "Some message";
            toast.FontSize = 50;
            toast.TextOrientation = System.Windows.Controls.Orientation.Vertical;
            toast.ImageSource = new BitmapImage(new Uri("ApplicationIcon.png", UriKind.RelativeOrAbsolute));

            toast.Completed += toast_Completed;
            toast.Show();
        }

        private void btnCustomizedToastPrompt_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush gray = new SolidColorBrush(Colors.LightGray);
            SolidColorBrush orange = new SolidColorBrush(Color.FromArgb(200, 255, 117, 24));

            ToastPrompt toast = new ToastPrompt
            {
                Background = gray,
                Foreground = orange,
                Title = "Customized",
                Message = "Custom colors",
                FontSize = 40,
                TextOrientation = System.Windows.Controls.Orientation.Vertical,
                ImageSource =
                    new BitmapImage(new Uri("..\\ApplicationIcon.png", UriKind.RelativeOrAbsolute))
            };

            toast.Completed += toast_Completed;
            toast.Show();
        }

        private void btnVerticalText_Click(object sender, RoutedEventArgs e)
        {
            ToastPrompt toast = new ToastPrompt();
            toast.Title = "Vertical";
            toast.Message = "Vertical text message";
            toast.FontSize = 40;
            toast.TextOrientation = System.Windows.Controls.Orientation.Vertical;
            toast.ImageSource = new BitmapImage(new Uri("ApplicationIcon.png", UriKind.RelativeOrAbsolute));

            toast.Show();
        }

        private void btnHorizontalText_Click(object sender, RoutedEventArgs e)
        {
            ToastPrompt toast = new ToastPrompt();
            toast.FontSize = 30;
            toast.Title = "Horizontal";
            toast.Message = "Horizontal text";
            toast.TextOrientation = System.Windows.Controls.Orientation.Horizontal;
            toast.ImageSource = new BitmapImage(new Uri("ApplicationIcon.png", UriKind.RelativeOrAbsolute));

            toast.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var toast = new ToastPrompt
            {
                Title = "Simple usage",
                Message = "Message",
                ImageSource = new BitmapImage(new Uri("..\\ApplicationIcon.png", UriKind.RelativeOrAbsolute))
            };
            toast.Show();
        }
    }

}