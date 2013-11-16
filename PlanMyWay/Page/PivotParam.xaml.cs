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
using System.IO.IsolatedStorage;
using PlanMyWay.Lib.Manager;
using PlanMyWay.Lib.Manager.EventArgs;
using Coding4Fun.Toolkit.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Advertising.Mobile.UI;

namespace PlanMyWay.Page
{
    public partial class PivotParam : PhoneApplicationPage
    {
        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
        ApplicationBarIconButton btn_Ok;
        
        public PivotParam()
        {
            InitializeComponent();
            btn_Ok = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
            InitChamps();

            if ((Application.Current as App).IsTrial)
            {

                AdControl control = new AdControl("9ec83715-ac1e-41b5-9b0d-62b3cb52524a", "10063501", true);
                control.Width = 300;
                control.Height = 50;
                pubPane.Children.Insert(0, control);
            }
        }

        private void pivot1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (pivot1.SelectedIndex == 0)
            {
                pivot1.Title = " Compte";
            }
            if (pivot1.SelectedIndex == 1)
            {
                pivot1.Title = "Consommation";
            }
            if (pivot1.SelectedIndex == 2)
            {
                pivot1.Title = "Paramètres - Compte";
            }
            if (pivot1.SelectedIndex == 3)
            {
                pivot1.Title = "Paramètres - Consommation";
            }
        }

        private void InitChamps()
        {
            tbx_aadrDepart.Text = settings["AdrDepart"].ToString();
            tbx_adrArriver.Text = settings["AdrArriver"].ToString();
            tbx_ConsoMoyen.Text = settings["ConsoCarburant"].ToString();
            tbx_prixCarbu.Text = settings["PrixCarburant"].ToString();
        }

        private void tbx_ConsoMoyen_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (tbx_ConsoMoyen.Text != "")
                {
                    settings["ConsoCarburant"] = Double.Parse(tbx_ConsoMoyen.Text.Replace(".", ","));
                    save();
                }
            }
            catch
            {
                MessageBox.Show("Entrer une valeur numérique");
                tbx_ConsoMoyen.Text = "";
            }
        }

        private void tbx_prixCarbu_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (tbx_prixCarbu.Text != "")
                {
                    settings["PrixCarburant"] = Double.Parse(tbx_prixCarbu.Text.Replace(".",","));
                    save();
                }
            }
            catch
            {
                MessageBox.Show("entrer une valeur numérique");
                tbx_prixCarbu.Text = "";
            }
        }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbx_aadrDepart.Text))
                return;
            verifAdrDepart();
           
        }
        private void verifAdrDepart()
        {
            MeetingManager monMM = new MeetingManager();
            monMM.SearchStringCompleted += (o, eSearchString) =>
            {
                if (eSearchString.Error)
                {
                    //afficher erreur
                    monMM.SearchStringCompleted = null;

                    ToastPrompt toast = new ToastPrompt();
                    toast.Title = "Erreur";
                    toast.Message = "Adresse de départ invalide";
                    toast.Background = new SolidColorBrush(Colors.Red);
                    toast.Show();
                    return;
                }
                settings["AdrDepart"] = eSearchString.Address;
                settings["VilleDepart"] = eSearchString.City;
                settings["latiAdrDepart"] = eSearchString.Location.Latitude;
                settings["longiAdrDepart"] = eSearchString.Location.Longitude;
                ToastPrompt toast2 = new ToastPrompt();
                toast2.Title = "Succès";
                toast2.Message = "Adresse de départ reconnue";
                toast2.Show();
                tbx_aadrDepart.Text = eSearchString.Address;
                monMM.SearchStringCompleted = null;
                save();
            };
            monMM.SearchStringAsync(tbx_aadrDepart.Text);
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbx_adrArriver.Text))
                return;
            verifAdrArriver();
        }
        private void verifAdrArriver()
        {
            MeetingManager monMM = new MeetingManager();
            monMM.SearchStringCompleted += (o, eSearchString) =>
            {
                if (eSearchString.Error)
                {
                    //afficher erreur
                    monMM.SearchStringCompleted = null;
                    ToastPrompt toast = new ToastPrompt();
                    toast.Title = "Erreur";
                    toast.Message = "Adresse d'arrivée invalide";
                    toast.Background = new SolidColorBrush(Colors.Red);
                    toast.Show();
                    btn_Ok.IsEnabled = true;
                    button1.IsEnabled = true;
                    button2.IsEnabled = true;
                    return;
                }
                settings["AdrArriver"] = eSearchString.Address;
                settings["VilleArriver"] = eSearchString.City;
                settings["latiAdrArriver"] = eSearchString.Location.Latitude;
                settings["longiAdrArriver"] = eSearchString.Location.Longitude;
                ToastPrompt toast2 = new ToastPrompt();
                toast2.Title = "Succès";
                toast2.Message = "Adresse d'arrivée reconnue";
                toast2.Show();
                tbx_adrArriver.Text = eSearchString.Address;
                monMM.SearchStringCompleted = null;
                save();
                btn_Ok.IsEnabled = true;
                button1.IsEnabled = true;
                button2.IsEnabled = true;
            };
            monMM.SearchStringAsync(tbx_adrArriver.Text);
        }
        private void save()
        {
            if (settings.Contains("selectedDateAndPositions"))
                settings.Remove("selectedDateAndPositions");
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            btn_Ok.IsEnabled = false;
            button1.IsEnabled = false;
            button2.IsEnabled = false;
            verifAdrDepart();
            verifAdrArriver();
            this.NavigationService.GoBack();
        }
    }
}