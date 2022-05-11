using ModernWpf.Controls;
using System;
using System.ComponentModel;

namespace FootballManager.PagesAdmin
{
    [Serializable]
    public partial class OrdersSEDialog : ContentDialog
    {
        public OrdersSEDialog()
        {
            InitializeComponent();
            this.DataContext = this;
                Check();
        }

        private string mname;
        public string MName
        {
            get
            {
                return mname;
            }
            set
            {
                if (mname == value)
                    return;

                mname = value;
                Check();
            }
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
                e.Handled = !Char.IsLetter(Convert.ToChar(e.Text));
            }
            catch { }
        }
        private void CheckDigits(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            try
            {
                e.Handled = System.Text.RegularExpressions.Regex.IsMatch(e.Text, "[^0-9]");;
            }
            catch { }
        }
        public void Check() => this.IsPrimaryButtonEnabled = !(string.IsNullOrEmpty(MName) || Price == null);
    }
}
