using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FootballManager.PagesAdmin
{
    [Serializable]
    public partial class TicketsDialog : ContentDialog
    {
        public TicketsDialog()
        {
            InitializeComponent();
            this.DataContext = this;
            Check();
        }

        public List<string> DateList { get; set; }
        private string _date;
        public string Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
                Check();
            }
        }

        private decimal? price = null;
        public decimal? Price
        {
            get
            {
                return price;
            }
            set
            {
                if (price == value)
                    return;

                price = value;
                Calculate();
            Check();
            }
        }

        private int? count;
        public int? Count
        {
            get
            {
                return count;
            }
            set
            {
                if (count == value)
                    return;

                count = value;
                Calculate();
            Check();
            }
        }

        private void Calculate()
        {
            if (count is null || price is null)
                return;

            Balance.Content = $"Доход: {Count * Price}";
        }
        private void CheckDigits(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            try
            {
                e.Handled = System.Text.RegularExpressions.Regex.IsMatch(e.Text, "[^0-9]");;
            }
            catch { }
        }
        public void Check() => this.IsPrimaryButtonEnabled = !(Count == null || Price == null || string.IsNullOrEmpty(Date));
    }
}
