using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FootballManager.PagesAdmin
{
    [Serializable]
    public partial class ContractsDialog : ContentDialog
    {
        public ContractsDialog()
        {
            InitializeComponent();
            this.DataContext = this;
            PList = new List<string>();
            Check();
        }

        private DateTime datestart = DateTime.Now;
        public DateTime DateStart
        {
            get
            {
                return datestart;
            }
            set
            {
                if (datestart == value)
                    return;

                datestart = value;
                Check();
            }
        }

        private DateTime dateend = DateTime.Now;
        public DateTime DateEnd
        {
            get
            {
                return dateend;
            }
            set
            {
                if (dateend == value)
                    return;

                dateend = value;
                Check();
            }
        }

        private decimal? monthpay;
        public decimal? MonthPay
        {
            get
            {
                return monthpay;
            }
            set
            {
                if (monthpay == value)
                    return;

                monthpay = value;
                Check();
            }
        }

        public List<string> PList { get; set; }

        private string pval;

        public string PValue
        {
            get
            {
                return pval;
            }
            set
            {
                if (pval == value)
                    return;

                pval = value;
                Check();
            }
        }

        private void CheckDigits(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            try
            {
                e.Handled = System.Text.RegularExpressions.Regex.IsMatch(e.Text, "[^0-9]");;
            }
            catch { }
        }
        public void Check() => this.IsPrimaryButtonEnabled = !(MonthPay == null || string.IsNullOrEmpty(PValue) || DateEnd.Year < 2000 || DateStart.Year < 2000);
    }
}
