using FootballManager;
using ModernWpf.Controls;
using FootballManager.Pages;
using FootballManager.Presets;
using SamplesCommon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;
using Windows.UI.Xaml.Media;
using Frame = ModernWpf.Controls.Frame;
using ModernWpf;

namespace FootballManager
{
    public partial class NavigationRootPage
    {
        private const string AutoHideScrollBarsKey = "AutoHideScrollBars";

        public static NavigationRootPage Current
        {
            get => _current.Value;
            private set => _current.Value = value;
        }

        public static Frame RootFrame
        {
            get => _rootFrame.Value;
            private set => _rootFrame.Value = value;
        }

        private static readonly ThreadLocal<NavigationRootPage> _current = new ThreadLocal<NavigationRootPage>();
        private static readonly ThreadLocal<Frame> _rootFrame = new ThreadLocal<Frame>();

        private bool _ignoreSelectionChange;
        private readonly ControlPagesData _controlPagesData = new ControlPagesData();
        private Type _startPage;

        public NavigationRootPage()
        {
            InitializeComponent();

            BalancePanel.Visibility = (Globals.isManager) ? Visibility.Visible : Visibility.Hidden;

            Current = this;
            RootFrame = rootFrame;

            SetStartPage();
            if (_startPage != null)
            {
                PagesList.SelectedItem = PagesList.Items.OfType<ControlInfoDataItem>().FirstOrDefault(x => x.PageType == _startPage);
            }

            NavigateToSelectedPage();
            SetApplicationTheme(ApplicationTheme.Dark);
        }

        partial void SetStartPage();

        private void ContextMenu_Loaded(object sender, RoutedEventArgs e)
        {
            //Debug.WriteLine(nameof(ContextMenu_Loaded));
            var menu = (ContextMenu)sender;
            var tabItem = (TabItem)menu.PlacementTarget;
            var content = (FrameworkElement)tabItem.Content;
            FindMenuItem(menu, ThemeManager.GetRequestedTheme(content)).IsChecked = true;
        }

        private void ToggleTheme(object sender, RoutedEventArgs e)
        {
            GetTabItemContent(sender as MenuItem)?.ToggleTheme();
        }

        private void ThemeMenuItem_Checked(object sender, RoutedEventArgs e)
        {
            //Debug.WriteLine($"{((RadioMenuItem)e.Source).Header} checked");
            var menuItem = (RadioMenuItem)e.Source;
            var tabItemContent = GetTabItemContent(menuItem);
            if (tabItemContent != null)
            {
                ThemeManager.SetRequestedTheme(tabItemContent, (ElementTheme)menuItem.Tag);
            }
        }

        private void ThemeMenuItem_Unchecked(object sender, RoutedEventArgs e)
        {
            //Debug.WriteLine($"{((RadioMenuItem)e.Source).Header} unchecked");
        }

        private RadioMenuItem FindMenuItem(ContextMenu menu, ElementTheme theme)
        {
            return menu.Items.OfType<RadioMenuItem>().First(x => (ElementTheme)x.Tag == theme);
        }

        private FrameworkElement GetTabItemContent(MenuItem menuItem)
        {
            return ((menuItem
                ?.Parent as ContextMenu)
                ?.PlacementTarget as TabItem)
                ?.Content as FrameworkElement;
        }

        private void NavigateToSelectedPage()
        {
            if (PagesList.SelectedValue is Type type)
            {
                RootFrame?.Navigate(type);
            }
        }

        private void PagesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_ignoreSelectionChange)
            {
                NavigateToSelectedPage();
            }
        }

        private void RootFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                RootFrame.RemoveBackEntry();
            }
        }

        private void RootFrame_Navigated(object sender, NavigationEventArgs e)
        {
            Debug.Assert(!RootFrame.CanGoForward);

            _ignoreSelectionChange = true;
            PagesList.SelectedValue = RootFrame.CurrentSourcePageType;
            _ignoreSelectionChange = false;
        }

        private void Default_Checked(object sender, RoutedEventArgs e)
        {
            SetApplicationTheme(null);
        }

        private void Light_Checked(object sender, RoutedEventArgs e)
        {
            SetApplicationTheme(ApplicationTheme.Light);
        }

        private void Dark_Checked(object sender, RoutedEventArgs e)
        {
            SetApplicationTheme(ApplicationTheme.Dark);
        }

        private void PresetMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is MenuItem menuItem)
            {
                PresetManager.Current.ColorPreset = (string)menuItem.Header;
            }
        }

        private void SizingMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is MenuItem menuItem)
            {
                bool compact = menuItem.Tag as string == "Compact";

                var xcr = Application.Current.Resources.MergedDictionaries.OfType<XamlControlsResources>().FirstOrDefault();
                if (xcr != null)
                {
                    xcr.UseCompactResources = compact;
                }
            }
        }

        private void ShadowsAuto_Checked(object sender, RoutedEventArgs e)
        {
            Application.Current.Resources.Remove(SystemParameters.DropShadowKey);
        }

        private void ShadowsEnabled_Checked(object sender, RoutedEventArgs e)
        {
            Application.Current.Resources[SystemParameters.DropShadowKey] = true;
        }

        private void ShadowsDisabled_Checked(object sender, RoutedEventArgs e)
        {
            Application.Current.Resources[SystemParameters.DropShadowKey] = false;
        }

        private void AutoHideScrollBarsAuto_Checked(object sender, RoutedEventArgs e)
        {
            Application.Current.Resources.Remove(AutoHideScrollBarsKey);
        }

        private void AutoHideScrollBarsOn_Checked(object sender, RoutedEventArgs e)
        {
            Application.Current.Resources[AutoHideScrollBarsKey] = true;
        }

        private void AutoHideScrollBarsOff_Checked(object sender, RoutedEventArgs e)
        {
            Application.Current.Resources[AutoHideScrollBarsKey] = false;
        }

        private void ForceGC(object sender, RoutedEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void NewWindow(object sender, RoutedEventArgs e)
        {
            var thread = new Thread(() =>
            {
                var window = new MainWindow();
                window.Closed += delegate
                {
                    Dispatcher.CurrentDispatcher.BeginInvokeShutdown(DispatcherPriority.Normal);
                };
                window.Show();
                Dispatcher.Run();
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            thread.Start();
        }

        private void OnThemeButtonClick(object sender, RoutedEventArgs e)
        {
            DispatcherHelper.RunOnMainThread(() =>
            {
                if (ThemeManager.Current.ActualApplicationTheme == ApplicationTheme.Dark)
                {
                    ThemeManager.Current.ApplicationTheme = ApplicationTheme.Light;
                }
                else
                {
                    ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
                }
            });
        }

        private void SetApplicationTheme(ApplicationTheme? theme)
        {
            DispatcherHelper.RunOnMainThread(() =>
            {
                ThemeManager.Current.ApplicationTheme = theme;
            });
        }

        private void Leave(object sender, RoutedEventArgs e) => MainWindow.mainWindow.Leave();
        private void About(object sender, RoutedEventArgs e) => new About().Show();
    }

    public class ControlPagesData : List<ControlInfoDataItem>
    {
        public ControlPagesData()
        {
            if (!Globals.isManager)
            {
                AddPage(typeof(PlayerList), new FontAwesome.WPF.FontAwesome() { Icon = FontAwesome.WPF.FontAwesomeIcon.List, FontSize = 24 }, "Список игроков");
                AddPage(typeof(GamesSchedule), new FontAwesome.WPF.FontAwesome() { Icon = FontAwesome.WPF.FontAwesomeIcon.Calendar, FontSize = 24 }, "График игр");
                AddPage(typeof(TrainingSchedule), new FontAwesome.WPF.FontAwesome() { Icon = FontAwesome.WPF.FontAwesomeIcon.FutbolOutline, FontSize = 24 }, "График тренировок");
            }
            else
            {
                AddPage(typeof(PagesAdmin.Operations), new FontAwesome.WPF.FontAwesome() { Icon = FontAwesome.WPF.FontAwesomeIcon.Money, FontSize = 24 }, "Операции");
                AddPage(typeof(PagesAdmin.PlayerList), new FontAwesome.WPF.FontAwesome() { Icon = FontAwesome.WPF.FontAwesomeIcon.List, FontSize = 24 }, "Список игроков");
                AddPage(typeof(PagesAdmin.Market), new FontAwesome.WPF.FontAwesome() { Icon = FontAwesome.WPF.FontAwesomeIcon.Bullseye, FontSize = 24 }, "Игроки на продаже");
                AddPage(typeof(PagesAdmin.OperationsPlayers), new FontAwesome.WPF.FontAwesome() { Icon = FontAwesome.WPF.FontAwesomeIcon.Check, FontSize = 24 }, "История покупок\nи продаж игроков");
                AddPage(typeof(PagesAdmin.GamesSchedule), new FontAwesome.WPF.FontAwesome() { Icon = FontAwesome.WPF.FontAwesomeIcon.Calendar, FontSize = 24 }, "График игр");
                AddPage(typeof(PagesAdmin.TrainingSchedule), new FontAwesome.WPF.FontAwesome() { Icon = FontAwesome.WPF.FontAwesomeIcon.FutbolOutline, FontSize = 24 }, "График тренировок");
                AddPage(typeof(PagesAdmin.Tickets), new FontAwesome.WPF.FontAwesome() { Icon = FontAwesome.WPF.FontAwesomeIcon.Ticket, FontSize = 24 }, "Билеты");
                AddPage(typeof(PagesAdmin.Orders), new FontAwesome.WPF.FontAwesome() { Icon = FontAwesome.WPF.FontAwesomeIcon.ShoppingBag, FontSize = 24 }, "Покупки");
                AddPage(typeof(PagesAdmin.Employees), new FontAwesome.WPF.FontAwesome() { Icon = FontAwesome.WPF.FontAwesomeIcon.Users, FontSize = 24 }, "Сотрудники");
                AddPage(typeof(PagesAdmin.Contracts), new FontAwesome.WPF.FontAwesome() { Icon = FontAwesome.WPF.FontAwesomeIcon.File, FontSize = 24 }, "Контракты");
            }
        }

        private void AddPage(Type pageType, FontAwesome.WPF.FontAwesome icon, string displayName = null)
        {
            Add(new ControlInfoDataItem(pageType, icon, displayName));
        }
    }

    public class ControlInfoDataItem
    {
        public ControlInfoDataItem(Type pageType, FontAwesome.WPF.FontAwesome icon, string title = null)
        {
            PageType = pageType;
            Title = title ?? pageType.Name.Replace("Page", null);
            FontIcon = icon;
        }

        public FontAwesome.WPF.FontAwesome FontIcon { get; }
        
        public string Title { get; }

        public Type PageType { get; }

        public override string ToString()
        {
            return Title;
        }
    }
}
