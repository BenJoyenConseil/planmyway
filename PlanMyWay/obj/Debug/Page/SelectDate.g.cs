﻿#pragma checksum "C:\Users\ben\Documents\Visual Studio 2012\Projects\PlanMyWay\PlanMyWay\Page\SelectDate.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "DF3CEF507DFF7BA0D6F4F6B0AD3F1552"
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


namespace PlanMyWay.Lib.Test {
    
    
    public partial class PanoramaPage1 : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton btn_settings;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton btn_refresh;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton btn_export;
        
        internal Microsoft.Phone.Shell.ProgressIndicator progressBar;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Microsoft.Phone.Controls.DatePicker dateFrom;
        
        internal Microsoft.Phone.Controls.DatePicker dateTo;
        
        internal System.Windows.Controls.Button btn_selectRoadMaps;
        
        internal System.Windows.Controls.ListBox lst_results;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/PlanMyWay;component/Page/SelectDate.xaml", System.UriKind.Relative));
            this.btn_settings = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("btn_settings")));
            this.btn_refresh = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("btn_refresh")));
            this.btn_export = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("btn_export")));
            this.progressBar = ((Microsoft.Phone.Shell.ProgressIndicator)(this.FindName("progressBar")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.dateFrom = ((Microsoft.Phone.Controls.DatePicker)(this.FindName("dateFrom")));
            this.dateTo = ((Microsoft.Phone.Controls.DatePicker)(this.FindName("dateTo")));
            this.btn_selectRoadMaps = ((System.Windows.Controls.Button)(this.FindName("btn_selectRoadMaps")));
            this.lst_results = ((System.Windows.Controls.ListBox)(this.FindName("lst_results")));
        }
    }
}

