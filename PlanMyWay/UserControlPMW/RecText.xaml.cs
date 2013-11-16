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
    public partial class RecText : UserControl
    {
        private string _Affinfo1;

        public string Affinfo1
        {
            get { return _Affinfo1; }
            set { _Affinfo1 = value; }
        }       
       
        private string _DonneInfo2;

        public string DonneInfo2
        {
            get { return _DonneInfo2; }
            set { _DonneInfo2 = value; }
        }
        public RecText()
        {
            InitializeComponent();
        }

        public void InitInfo()
        {
            tb_Info1.Text = _Affinfo1;
            tb_info2.Text = _DonneInfo2;
        }
    }
}
