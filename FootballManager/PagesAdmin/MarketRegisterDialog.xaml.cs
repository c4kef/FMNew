using ModernWpf.Controls;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace FootballManager.PagesAdmin
{
    [Serializable]
    public partial class MarketRegisterDialog : ContentDialog
    {
        public MarketRegisterDialog()
        {
            InitializeComponent();
            this.DataContext = this;
            Check();
        }

        private string login;
        public string Login
        {
            get
            {
                return login;
            }
            set
            {
                if (login == value)
                    return;

                login = value;
                Check();
            }
        }

        private string pass;
        public string Pass
        {
            get
            {
                return pass;
            }
            set
            {
                if (pass == value)
                    return;

                pass = value;
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
        
        public void Check() => this.IsPrimaryButtonEnabled = !(string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Pass));
    }
}
