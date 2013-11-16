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
    public partial class PageParam : PhoneApplicationPage
    {
        private enum TypeCarburant { sp95, sp98, Diesel, GPL, electrique };
        private enum Langue { Anglais, Francais };
        private double _prixCarburant;
        public double PrixCarburant1
        {
            get { return _prixCarburant; }
            set { _prixCarburant = value; }
        }
        private bool _activeBell;
        public bool ActiveBell
        {
            get { return _activeBell; }
            set { _activeBell = value; }
        }

        public double PrixCarburant
        {
            get { return _prixCarburant; }
            set { _prixCarburant = value; }
        }
        ListPicker lstp_Carbu;
        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
        bool changmeent = false;


        String[] Carburant = { "SP95","SP98","ELECTRIQUE","GPL","DIESEL"};

        public PageParam()
        {
            InitializeComponent();
            this.lpkTypeCarbu.ItemsSource = Carburant;
            this.txtB_PrixCarbu.Text =((double)settings["PrixCarburant"])+"";
            this.txtUtil.Text = ((string)settings["Utilisateur"]);
            lpkTypeCarbu.SelectedIndex = ((int)settings["Carburant"]);
            _activeBell =(bool) settings["ActiveNotif"];
            txtLieuRef.Text =(string) settings["LieuRef"];
            if (_activeBell)
            {
                rb_yes.IsChecked = _activeBell;
            }
            else
            {
                rb_no.IsChecked = !_activeBell;
            }
            SetInputScop(txtB_PrixCarbu);
        }
        private void SetInputScop(TextBox tb)
        {
            InputScopeNameValue digitsInputNameValue = InputScopeNameValue.TelephoneNumber;
            tb.InputScope = new InputScope()
            {
                Names = { new InputScopeName() { NameValue = digitsInputNameValue } }
            };
        }

        private void txtB_PrixCarbu_TextChanged(object sender, TextChangedEventArgs e)
        {
            settings["PrixCarburant"] = double.Parse(txtB_PrixCarbu.Text);
        }

        private void txtUtil_TextChanged(object sender, TextChangedEventArgs e)
        {
            settings["Utilisateur"] = txtUtil.Text;
        }

        private void lpkTypeCarbu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (changmeent)
            {
                settings["Carburant"] = lpkTypeCarbu.SelectedIndex;
            }
            else
            {
                changmeent = true;
            }

        }

        private void rb_yes_Checked(object sender, RoutedEventArgs e)
        {
            settings["ActiveNotif"] = rb_yes.IsChecked;
        }

        private void rb_no_Checked(object sender, RoutedEventArgs e)
        {
            settings["ActiveNotif"] = !rb_no.IsChecked;
        }

        private void txtLieuRef_TextChanged(object sender, TextChangedEventArgs e)
        {
            settings["LieuRef"] = txtLieuRef.Text;
        }

        private void btn_Submit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Page/MenuPrin.xaml", UriKind.Relative));
        }
    }
}