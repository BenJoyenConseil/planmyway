﻿#pragma checksum "C:\Users\ben\Documents\Visual Studio 2012\Projects\PlanMyWay\PlanMyWay\Page\PageParam.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "69F124A054D46CD4F83E74A217EA0B2A"
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
    
    
    public partial class PageParam : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.DataTemplate lpkItemTemplate;
        
        internal System.Windows.DataTemplate lpkFullItemTemplate;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.TextBlock ApplicationTitle;
        
        internal System.Windows.Controls.TextBlock PageTitle;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.TextBlock tb_PrixCarbu;
        
        internal System.Windows.Controls.TextBox txtB_PrixCarbu;
        
        internal System.Windows.Controls.TextBlock tb_TypeCarbu;
        
        internal System.Windows.Controls.TextBlock tb_NotificationActive;
        
        internal System.Windows.Controls.TextBlock tb_LieuRef;
        
        internal System.Windows.Controls.TextBox txtLieuRef;
        
        internal System.Windows.Controls.TextBlock tb_Utilisateur;
        
        internal System.Windows.Controls.TextBox txtUtil;
        
        internal Microsoft.Phone.Controls.ListPicker lpkTypeCarbu;
        
        internal System.Windows.Controls.RadioButton rb_yes;
        
        internal System.Windows.Controls.RadioButton rb_no;
        
        internal System.Windows.Controls.Button btn_Submit;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/PlanMyWay;component/Page/PageParam.xaml", System.UriKind.Relative));
            this.lpkItemTemplate = ((System.Windows.DataTemplate)(this.FindName("lpkItemTemplate")));
            this.lpkFullItemTemplate = ((System.Windows.DataTemplate)(this.FindName("lpkFullItemTemplate")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.ApplicationTitle = ((System.Windows.Controls.TextBlock)(this.FindName("ApplicationTitle")));
            this.PageTitle = ((System.Windows.Controls.TextBlock)(this.FindName("PageTitle")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.tb_PrixCarbu = ((System.Windows.Controls.TextBlock)(this.FindName("tb_PrixCarbu")));
            this.txtB_PrixCarbu = ((System.Windows.Controls.TextBox)(this.FindName("txtB_PrixCarbu")));
            this.tb_TypeCarbu = ((System.Windows.Controls.TextBlock)(this.FindName("tb_TypeCarbu")));
            this.tb_NotificationActive = ((System.Windows.Controls.TextBlock)(this.FindName("tb_NotificationActive")));
            this.tb_LieuRef = ((System.Windows.Controls.TextBlock)(this.FindName("tb_LieuRef")));
            this.txtLieuRef = ((System.Windows.Controls.TextBox)(this.FindName("txtLieuRef")));
            this.tb_Utilisateur = ((System.Windows.Controls.TextBlock)(this.FindName("tb_Utilisateur")));
            this.txtUtil = ((System.Windows.Controls.TextBox)(this.FindName("txtUtil")));
            this.lpkTypeCarbu = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("lpkTypeCarbu")));
            this.rb_yes = ((System.Windows.Controls.RadioButton)(this.FindName("rb_yes")));
            this.rb_no = ((System.Windows.Controls.RadioButton)(this.FindName("rb_no")));
            this.btn_Submit = ((System.Windows.Controls.Button)(this.FindName("btn_Submit")));
        }
    }
}

