﻿#pragma checksum "C:\Users\ben\Documents\Visual Studio 2012\Projects\PlanMyWay\PlanMyWay\UserControlPMW\FlecheBasInfo.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "65C0B622697854FF6D31878F3D669F65"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18010
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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


namespace PlanMyWay.UserControlPMW {
    
    
    public partial class FlecheBasInfo : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock tb_SubjectRdv;
        
        internal System.Windows.Controls.TextBlock tb_AdrRdv;
        
        internal System.Windows.Controls.Canvas ContentPanelCanvas;
        
        internal System.Windows.Shapes.Line MyLine;
        
        internal System.Windows.Shapes.Polygon maFleche;
        
        internal System.Windows.Controls.TextBlock tb_Distance;
        
        internal System.Windows.Controls.TextBlock tb_DistanceInfo;
        
        internal System.Windows.Controls.TextBlock tb_Conso;
        
        internal System.Windows.Controls.TextBlock tb_ConsoInfo;
        
        internal System.Windows.Controls.TextBlock tb_TpsTrajet;
        
        internal System.Windows.Controls.TextBlock tb_TpsTrajetInfo;
        
        internal System.Windows.Controls.TextBlock tb_Cout;
        
        internal System.Windows.Controls.TextBlock tb_CoutInfo;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/PlanMyWay;component/UserControlPMW/FlecheBasInfo.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.tb_SubjectRdv = ((System.Windows.Controls.TextBlock)(this.FindName("tb_SubjectRdv")));
            this.tb_AdrRdv = ((System.Windows.Controls.TextBlock)(this.FindName("tb_AdrRdv")));
            this.ContentPanelCanvas = ((System.Windows.Controls.Canvas)(this.FindName("ContentPanelCanvas")));
            this.MyLine = ((System.Windows.Shapes.Line)(this.FindName("MyLine")));
            this.maFleche = ((System.Windows.Shapes.Polygon)(this.FindName("maFleche")));
            this.tb_Distance = ((System.Windows.Controls.TextBlock)(this.FindName("tb_Distance")));
            this.tb_DistanceInfo = ((System.Windows.Controls.TextBlock)(this.FindName("tb_DistanceInfo")));
            this.tb_Conso = ((System.Windows.Controls.TextBlock)(this.FindName("tb_Conso")));
            this.tb_ConsoInfo = ((System.Windows.Controls.TextBlock)(this.FindName("tb_ConsoInfo")));
            this.tb_TpsTrajet = ((System.Windows.Controls.TextBlock)(this.FindName("tb_TpsTrajet")));
            this.tb_TpsTrajetInfo = ((System.Windows.Controls.TextBlock)(this.FindName("tb_TpsTrajetInfo")));
            this.tb_Cout = ((System.Windows.Controls.TextBlock)(this.FindName("tb_Cout")));
            this.tb_CoutInfo = ((System.Windows.Controls.TextBlock)(this.FindName("tb_CoutInfo")));
        }
    }
}
