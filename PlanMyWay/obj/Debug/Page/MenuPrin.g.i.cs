﻿#pragma checksum "C:\Users\ben\Documents\Visual Studio 2012\Projects\PlanMyWay\PlanMyWay\Page\MenuPrin.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "7D59AD50AACC7D57D408990F72665D14"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.18033
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace PlanMyWay {
    
    
    public partial class MenuPrin : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal Microsoft.Phone.Shell.ApplicationBar appBar;
        
        internal Microsoft.Phone.Controls.Panorama panorama;
        
        internal Microsoft.Phone.Controls.PanoramaItem mainPane;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.Button btn_Logo;
        
        internal System.Windows.Controls.Button btn_MyWay;
        
        internal System.Windows.Controls.Button btn_EditionCsv;
        
        internal System.Windows.Controls.Button btn_Parametre;
        
        internal System.Windows.Controls.Button btn_Credits;
        
        internal System.Windows.Controls.TextBlock tb_PasDeConnection;
        
        internal System.Windows.Controls.ScrollViewer aideScroll;
        
        internal System.Windows.Controls.StackPanel pubPane;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/PlanMyWay;component/Page/MenuPrin.xaml", System.UriKind.Relative));
            this.appBar = ((Microsoft.Phone.Shell.ApplicationBar)(this.FindName("appBar")));
            this.panorama = ((Microsoft.Phone.Controls.Panorama)(this.FindName("panorama")));
            this.mainPane = ((Microsoft.Phone.Controls.PanoramaItem)(this.FindName("mainPane")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.btn_Logo = ((System.Windows.Controls.Button)(this.FindName("btn_Logo")));
            this.btn_MyWay = ((System.Windows.Controls.Button)(this.FindName("btn_MyWay")));
            this.btn_EditionCsv = ((System.Windows.Controls.Button)(this.FindName("btn_EditionCsv")));
            this.btn_Parametre = ((System.Windows.Controls.Button)(this.FindName("btn_Parametre")));
            this.btn_Credits = ((System.Windows.Controls.Button)(this.FindName("btn_Credits")));
            this.tb_PasDeConnection = ((System.Windows.Controls.TextBlock)(this.FindName("tb_PasDeConnection")));
            this.aideScroll = ((System.Windows.Controls.ScrollViewer)(this.FindName("aideScroll")));
            this.pubPane = ((System.Windows.Controls.StackPanel)(this.FindName("pubPane")));
        }
    }
}

