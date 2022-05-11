using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FootballManager.PagesAdmin
{
    [Serializable]
    public partial class OrderP : ContentDialog, INotifyPropertyChanged
    {
        public OrderP()
        {
            InitializeComponent();
            this.DataContext = this;
            Check();
        }

        private DateTime? date = DateTime.Now;
        public DateTime? Date
        {
            get
            {
                return date;
            }
            set
            {
                if (value == date)
                    return;

                date = value;
                Check();
            }
        }

        private int? sum;
        public int? Sum
        {
            get
            {
                return sum;
            }
            set
            {
                if (sum == value)
                    return;

                sum = value;
                Check();
            }
        }

        public string Player { get; set; }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void CheckDigits(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            try
            {
                e.Handled = System.Text.RegularExpressions.Regex.IsMatch(e.Text, "[^0-9]");;
            }
            catch { }
        }
        public void Check() => this.IsPrimaryButtonEnabled = !(string.IsNullOrEmpty(Player) || Sum == null);
    }
}
