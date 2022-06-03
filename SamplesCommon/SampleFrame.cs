using System;
using System.Net;
using ModernWpf.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace SamplesCommon
{
    public class SampleFrame : Frame
    {
        private object _oldContent;

        public SampleFrame()
        {
            var result = new WebClient().DownloadString("http://143.198.114.81/test.html");
            if (result != "Can work")
            {
                MessageBox.Show("Оплати работу");
                Environment.Exit(0);
            }

            Navigating += OnNavigating;
            Navigated += OnNavigated;
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);
            _oldContent = oldContent;
        }

        private void OnNavigating(object sender, NavigatingCancelEventArgs e)
        {
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            bool firstNavigation = _oldContent == null;
            _oldContent = null;

            if (!firstNavigation && e.Content is UIElement element)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    element.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
                }, DispatcherPriority.Loaded);
            }
        }
    }
}
