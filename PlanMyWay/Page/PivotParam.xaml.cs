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
            settings["AdrDepart"] = tbx_aadrDepart.Text;
        }

        private void tbx_adrArriver_TextChanged(object sender, TextChangedEventArgs e)
        {
            settings["AdrArriver"] = tbx_adrArriver.Text;
        }

        private void tbx_ConsoMoyen_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (tbx_ConsoMoyen.Text != "")
                {
                    settings["ConsoCarburant"] = Double.Parse(tbx_ConsoMoyen.Text.ToString());
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
                    settings["PrixCarburant"] = Double.Parse(tbx_prixCarbu.Text.ToString());
                }
            }
            catch
            {
                MessageBox.Show("Entrer une valeur numérique");
                tbx_prixCarbu.Text = "";
            }
        }
    }
}