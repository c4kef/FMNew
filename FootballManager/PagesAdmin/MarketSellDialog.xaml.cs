using ModernWpf.Controls;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace FootballManager.PagesAdmin
{
    [Serializable]
    public partial class MarketSellDialog : ContentDialog
    {
        public MarketSellDialog()
        {
            InitializeComponent();
            this.DataContext = this;
            Check();
        }

        private decimal? price;
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
                Check();
            }
        }
        
        private void CheckLetters(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            try
            {
                e.Handled = Char.IsLetter(Convert.ToChar(e.Text));
            }
            catch { }
        }
        
        public void Check() => this.IsPrimaryButtonEnabled = Price != null;
    }
}
