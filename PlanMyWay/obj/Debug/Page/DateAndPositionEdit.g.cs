﻿#pragma checksum "C:\Users\ben\Documents\Visual Studio 2012\Projects\PlanMyWay\PlanMyWay\Page\DateAndPositionEdit.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "7B709C2E1ABBCADF29C0379125DBAD20"
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


namespace PlanMyWay.Page {
    
    
    public partial class DateAndPositionEdit : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal Microsoft.Phone.Shell.ProgressIndicator progressBar;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.TextBlock lbl_start;
        
        internal System.Windows.Controls.TextBox txt_start;
        
        internal System.Windows.Controls.TextBlock lbl_end;
        
        internal System.Windows.Controls.TextBox txt_end;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/PlanMyWay;component/Page/DateAndPositionEdit.xaml", System.UriKind.Relative));
            this.progressBar = ((Microsoft.Phone.Shell.ProgressIndicator)(this.FindName("progressBar")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.lbl_start = ((System.Windows.Controls.TextBlock)(this.FindName("lbl_start")));
            this.txt_start = ((System.Windows.Controls.TextBox)(this.FindName("txt_start")));
            this.lbl_end = ((System.Windows.Controls.TextBlock)(this.FindName("lbl_end")));
            this.txt_end = ((System.Windows.Controls.TextBox)(this.FindName("txt_end")));
        }
    }
}
