﻿#pragma checksum "..\..\..\..\pages\LoginPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "8176CC9D1F10C33F2389D5963F28E7EC"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18033
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

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


namespace 正方系统抢课辅助 {
    
    
    /// <summary>
    /// LoginPage
    /// </summary>
    public partial class LoginPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 7 "..\..\..\..\pages\LoginPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal 正方系统抢课辅助.LoginPage loginPage;
        
        #line default
        #line hidden
        
        
        #line 10 "..\..\..\..\pages\LoginPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid grid_login;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\..\pages\LoginPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox Txb_StuNum;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\..\pages\LoginPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox Txb_Password;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\..\pages\LoginPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox checkBox_isSave;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\..\pages\LoginPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Btn_Login;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\..\pages\LoginPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txb_Remark;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\..\pages\LoginPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas Canvas_progressBar;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\..\pages\LoginPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ProgressBar progressBar;
        
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
            System.Uri resourceLocater = new System.Uri("/正方系统抢课辅助;component/pages/loginpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\pages\LoginPage.xaml"
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
            this.loginPage = ((正方系统抢课辅助.LoginPage)(target));
            
            #line 8 "..\..\..\..\pages\LoginPage.xaml"
            this.loginPage.Loaded += new System.Windows.RoutedEventHandler(this.loginPage_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.grid_login = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.Txb_StuNum = ((System.Windows.Controls.ComboBox)(target));
            
            #line 22 "..\..\..\..\pages\LoginPage.xaml"
            this.Txb_StuNum.LostFocus += new System.Windows.RoutedEventHandler(this.Txb_StuNum_LostFocus);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Txb_Password = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 5:
            this.checkBox_isSave = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 6:
            this.Btn_Login = ((System.Windows.Controls.Button)(target));
            
            #line 33 "..\..\..\..\pages\LoginPage.xaml"
            this.Btn_Login.Click += new System.Windows.RoutedEventHandler(this.Button_Click_1);
            
            #line default
            #line hidden
            return;
            case 7:
            this.txb_Remark = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 8:
            this.Canvas_progressBar = ((System.Windows.Controls.Canvas)(target));
            return;
            case 9:
            this.progressBar = ((System.Windows.Controls.ProgressBar)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

