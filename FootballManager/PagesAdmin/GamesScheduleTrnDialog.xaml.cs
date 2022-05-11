using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FootballManager.PagesAdmin
{
    [Serializable]
    public partial class GamesScheduleTrnDialog : ContentDialog
    {
        public GamesScheduleTrnDialog()
        {
            InitializeComponent();
            this.DataContext = this;
            Check();
        }

        private string tname;
        public string TName
        {
            get
            {
                return tname;
            }
            set
            {
                if (tname == value)
                    return;

                tname = value;
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
        private void Check() => this.IsPrimaryButtonEnabled = !string.IsNullOrEmpty(TName);
    }
}
