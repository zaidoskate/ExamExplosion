﻿#pragma checksum "..\..\GamePreferences.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "5A284638756661858B0556F47A0E3A78B0ED9A8A3699C0539F2B2F2822559F8F"
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

using ExamExplosion;
using ExamExplosion.Properties;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace ExamExplosion {
    
    
    /// <summary>
    /// GamePreferences
    /// </summary>
    public partial class GamePreferences : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\GamePreferences.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label MaxPlayerslbl;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\GamePreferences.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider MaxPlayersSlider;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\GamePreferences.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock MaxPlayersValue;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\GamePreferences.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label TimePerTurnlbl;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\GamePreferences.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider TimePerTurnSlider;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\GamePreferences.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock TimePerTurnValue;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\GamePreferences.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label MaxHPlbl;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\GamePreferences.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider MaxHPSlider;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\GamePreferences.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock MaxHPValue;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\GamePreferences.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label Createlbl;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\GamePreferences.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label Cancellbl;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ExamExplosion;component/gamepreferences.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\GamePreferences.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.MaxPlayerslbl = ((System.Windows.Controls.Label)(target));
            return;
            case 2:
            this.MaxPlayersSlider = ((System.Windows.Controls.Slider)(target));
            
            #line 13 "..\..\GamePreferences.xaml"
            this.MaxPlayersSlider.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.MaxPlayersSlider_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.MaxPlayersValue = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.TimePerTurnlbl = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.TimePerTurnSlider = ((System.Windows.Controls.Slider)(target));
            
            #line 17 "..\..\GamePreferences.xaml"
            this.TimePerTurnSlider.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.TimePerTurnSlider_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            this.TimePerTurnValue = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 7:
            this.MaxHPlbl = ((System.Windows.Controls.Label)(target));
            return;
            case 8:
            this.MaxHPSlider = ((System.Windows.Controls.Slider)(target));
            
            #line 21 "..\..\GamePreferences.xaml"
            this.MaxHPSlider.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.MaxHPSlider_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 9:
            this.MaxHPValue = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 10:
            this.Createlbl = ((System.Windows.Controls.Label)(target));
            return;
            case 11:
            this.Cancellbl = ((System.Windows.Controls.Label)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

