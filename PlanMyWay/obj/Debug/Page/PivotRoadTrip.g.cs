﻿#pragma checksum "C:\Users\ben\Documents\Visual Studio 2012\Projects\PlanMyWay\PlanMyWay\Page\PivotRoadTrip.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "963193027DDE1E6EFB2653DAA231377F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18010
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using PlanMyWay.UserControlPMW;
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
    
    
    public partial class PivotRoadTrip : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid gridPinc;
        
        internal System.Windows.Controls.TextBlock textBlock1;
        
        internal System.Windows.Controls.TextBlock textBlock2;
        
        internal System.Windows.Controls.Grid gr_InfoRD;
        
        internal System.Windows.Controls.Grid gr_InforJournee;
        
        internal PlanMyWay.UserControlPMW.RecText rt_nbrRdv;
        
        internal PlanMyWay.UserControlPMW.RecText rt_DistRdv;
        
        internal PlanMyWay.UserControlPMW.RecText rt_TpsTrajetRdv;
        
        internal PlanMyWay.UserControlPMW.RecText rt_ConsoTrajet;
        
        internal PlanMyWay.UserControlPMW.RecText rt_CoutTot;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/PlanMyWay;component/Page/PivotRoadTrip.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.gridPinc = ((System.Windows.Controls.Grid)(this.FindName("gridPinc")));
            this.textBlock1 = ((System.Windows.Controls.TextBlock)(this.FindName("textBlock1")));
            this.textBlock2 = ((System.Windows.Controls.TextBlock)(this.FindName("textBlock2")));
            this.gr_InfoRD = ((System.Windows.Controls.Grid)(this.FindName("gr_InfoRD")));
            this.gr_InforJournee = ((System.Windows.Controls.Grid)(this.FindName("gr_InforJournee")));
            this.rt_nbrRdv = ((PlanMyWay.UserControlPMW.RecText)(this.FindName("rt_nbrRdv")));
            this.rt_DistRdv = ((PlanMyWay.UserControlPMW.RecText)(this.FindName("rt_DistRdv")));
            this.rt_TpsTrajetRdv = ((PlanMyWay.UserControlPMW.RecText)(this.FindName("rt_TpsTrajetRdv")));
            this.rt_ConsoTrajet = ((PlanMyWay.UserControlPMW.RecText)(this.FindName("rt_ConsoTrajet")));
            this.rt_CoutTot = ((PlanMyWay.UserControlPMW.RecText)(this.FindName("rt_CoutTot")));
        }
    }
}

