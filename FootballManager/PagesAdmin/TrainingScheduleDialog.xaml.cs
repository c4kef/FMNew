using ModernWpf.Controls;
using System;
using System.ComponentModel;

namespace FootballManager.PagesAdmin
{
    [Serializable]
    public partial class TrainingScheduleDialog : ContentDialog
    {
        public TrainingScheduleDialog()
        {
            InitializeComponent();
            this.DataContext = this;
            Check();
        }

        private DateTime time = DateTime.Now;
        public DateTime Time
        {
            get
            {
                return time;
            }
            set
            {
                if (time == value)
                    return;

                time = value;
                Check();
            }
        }

        private DateTime date = DateTime.Now;
        public DateTime Date
        {
            get
            {
                return date;
            }
            set
            {
                if (date == value)
                    return;

                date = value;
                Check();
            }
        }

        private string location;
        public string Location
        {
            get
            {
                return location;
            }
            set
            {
                if (location == value)
                    return;

                location = value;
                Check();
            }
        }
        private void CheckLetters(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            try
            {
                e.Handled = !Char.IsLetter(Convert.ToChar(e.Text));
            }
            catch { }
        }
        public void Check() => this.IsPrimaryButtonEnabled = !(string.IsNullOrEmpty(Location) || Date.Date.Year < 2000 || Time.Date.Year < 2000);
    }
}
