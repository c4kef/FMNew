﻿using ModernWpf.Controls;
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

        public struct DateLists
        {
            public string Date{ get; set; }
            public int Id{ get; set; }
        }

        public List<DateLists> DateList { get; set; }
        private DateLists _date;
        public DateLists Date
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
        public void Check() => this.IsPrimaryButtonEnabled = !(Count == null || Count == 0 || Price == null || Price == 0|| Date.Equals(default(DateLists)));
    }
}
