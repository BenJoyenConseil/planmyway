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

namespace PlanMyWay.Page
{
    public partial class PivotParam : PhoneApplicationPage
    {
        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
        public PivotParam()
        {
            InitializeComponent();
            InitChamps();
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
            tbx_adrmail.Text = settings["AdrMail"].ToString();
            tbx_aadrDepart.Text = settings["AdrDepart"].ToString();
            tbx_adrArriver.Text = settings["AdrArriver"].ToString();
            tbx_ConsoMoyen.Text = settings["ConsoCarburant"].ToString();
            tbx_prixCarbu.Text = settings["PrixCarburant"].ToString();
        }

        private void tbx_adrmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            settings["AdrMail"] = tbx_adrmail.Text;
        }

        private void tbx_aadrDepart_TextChanged(object sender, TextChangedEventArgs e)
        {
         
        }

        public void OnSearChStringCompleted(object sender, SearchStringCompletedEventArgs e)
        {

        }

        private void tbx_adrArriver_TextChanged(object sender, TextChangedEventArgs e)
        {
          
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
                    MessageBox.Show("Adresse invalide");
                    return;
                }
                settings["AdrDepart"] = eSearchString.Address;
                settings["VilleDepart"] = eSearchString.City;
                settings["latiAdrDepart"] = eSearchString.Location.Latitude;
                settings["longiAdrDepart"] = eSearchString.Location.Longitude;
                MessageBox.Show("Adresse reconnue");
                tbx_aadrDepart.Text = eSearchString.Address;
                monMM.SearchStringCompleted = null;
                save();
            };
            monMM.SearchStringAsync(tbx_aadrDepart.Text);
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
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
                    MessageBox.Show("Adresse invalide");
                    btn_Ok.IsEnabled = true;
                    button1.IsEnabled = true;
                    button2.IsEnabled = true;
                    return;
                }
                settings["AdrArriver"] = eSearchString.Address;
                settings["VilleArriver"] = eSearchString.City;
                settings["latiAdrArriver"] = eSearchString.Location.Latitude;
                settings["longiAdrArriver"] = eSearchString.Location.Longitude;
                MessageBox.Show("Adresse reconnue");
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

        private void btn_Ok_Click(object sender, RoutedEventArgs e)
        {
            btn_Ok.IsEnabled = false;
            button1.IsEnabled = false;
            button2.IsEnabled = false;
            verifAdrDepart();
            verifAdrArriver();

        }
    }
}