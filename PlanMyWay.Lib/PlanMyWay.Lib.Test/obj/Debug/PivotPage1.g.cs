﻿#pragma checksum "C:\Users\ben\Documents\Visual Studio 2012\Projects\PlanMyWay\PlanMyWay.Lib\PlanMyWay.Lib.Test\PivotPage1.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "530136E280C38836CB072F2505405F5C"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.18033
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Live.Controls;
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


namespace PlanMyWay.Lib.Test {
    
    
    public partial class PivotPage1 : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Microsoft.Live.Controls.SignInButton signInButton;
        
        internal System.Windows.Controls.TextBox Title;
        
        internal System.Windows.Controls.TextBox Text;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/PlanMyWay.Lib.Test;component/PivotPage1.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.signInButton = ((Microsoft.Live.Controls.SignInButton)(this.FindName("signInButton")));
            this.Title = ((System.Windows.Controls.TextBox)(this.FindName("Title")));
            this.Text = ((System.Windows.Controls.TextBox)(this.FindName("Text")));
        }
    }
}

