﻿#pragma checksum "..\..\..\..\PagesAdmin\EmployeesDialog.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "18967CD9410CDEC79BD3C06A79267109FECC00D8"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using FootballManager.PagesAdmin;
using ModernWpf;
using ModernWpf.Controls;
using ModernWpf.Controls.Primitives;
using ModernWpf.DesignTime;
using ModernWpf.MahApps;
using ModernWpf.MahApps.Controls;
using ModernWpf.Markup;
using ModernWpf.Media.Animation;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
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


namespace FootballManager.PagesAdmin {
    
    
    /// <summary>
    /// EmployeesDialog
    /// </summary>
    public partial class EmployeesDialog : ModernWpf.Controls.ContentDialog, System.Windows.Markup.IComponentConnector {
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.17.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/FootballManager;component/pagesadmin/employeesdialog.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\PagesAdmin\EmployeesDialog.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.17.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 14 "..\..\..\..\PagesAdmin\EmployeesDialog.xaml"
            ((System.Windows.Controls.TextBox)(target)).PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.CheckLetters);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 15 "..\..\..\..\PagesAdmin\EmployeesDialog.xaml"
            ((System.Windows.Controls.TextBox)(target)).PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.CheckLetters);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 16 "..\..\..\..\PagesAdmin\EmployeesDialog.xaml"
            ((System.Windows.Controls.TextBox)(target)).PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.CheckLetters);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 18 "..\..\..\..\PagesAdmin\EmployeesDialog.xaml"
            ((System.Windows.Controls.TextBox)(target)).PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.CheckLetters);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 19 "..\..\..\..\PagesAdmin\EmployeesDialog.xaml"
            ((System.Windows.Controls.TextBox)(target)).PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.CheckDigits);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

