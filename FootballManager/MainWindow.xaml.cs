using FootballManager;
using ModernWpf.Controls;
using FootballManager.Helpers;
using FootballManager.Properties;
using System.Data.SqlClient;
using SamplesCommon;
using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.IO;
using ClosedXML.Excel;
using FootballManager.PagesAdmin;
using Microsoft.Win32;

namespace FootballManager
{
    public partial class MainWindow
    {
        public static MainWindow mainWindow;

        public MainWindow()
        {
            try
            {
                mainWindow = this;

                InitializeComponent();

                var rootFrame = NavigationRootPage.RootFrame;
                SetBinding(TitleBar.IsBackButtonVisibleProperty, new Binding { Path = new PropertyPath(System.Windows.Controls.Frame.CanGoBackProperty), Source = rootFrame });
                
                
                SubscribeToResourcesChanged();
                PageGrid.Children.Add(new NavigationRootPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            DispatcherHelper.RunOnMainThread(() =>
            {
                if (this == Application.Current.MainWindow)
                {
                    this.SetPlacement(Settings.Default.MainWindowPlacement);
                }
            });
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (!e.Cancel)
            {
                DispatcherHelper.RunOnMainThread(() =>
                {
                    if (this == Application.Current.MainWindow)
                    {
                        Settings.Default.MainWindowPlacement = this.GetPlacement();
                        Settings.Default.Save();
                    }
                });
            }
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            var rootFrame = NavigationRootPage.RootFrame;
            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
            }
        }

        [Conditional("DEBUG")]
        private void SubscribeToResourcesChanged()
        {
            Type t = typeof(FrameworkElement);
            EventInfo ei = t.GetEvent("ResourcesChanged", BindingFlags.NonPublic | BindingFlags.Instance);
            Type tDelegate = ei.EventHandlerType;
            MethodInfo h = GetType().GetMethod(nameof(OnResourcesChanged), BindingFlags.NonPublic | BindingFlags.Instance);
            Delegate d = Delegate.CreateDelegate(tDelegate, this, h);
            MethodInfo addHandler = ei.GetAddMethod(true);
            object[] addHandlerArgs = { d };
            addHandler.Invoke(this, addHandlerArgs);
        }

        private void OnResourcesChanged(object sender, EventArgs e)
        {
        }

        public void Leave()
        {
            new Window1().Show();
            this.Close();
            Globals.isManager = false;
        }
    }
}
