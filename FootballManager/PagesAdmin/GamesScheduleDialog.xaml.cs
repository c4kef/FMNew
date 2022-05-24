﻿using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FootballManager.PagesAdmin
{
    [Serializable]
    public partial class GamesScheduleDialog : ContentDialog
    {
        public GamesScheduleDialog()
        {
            InitializeComponent();
            this.DataContext = this;
            ListTours = new List<string>();
            Result = new List<string>() { "Выигран", "Не выигран", "Не играли" };
            Check();
        }

        /*private DateTime time = DateTime.Now;
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
        }*/

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

        private string team;
        public string Team
        {
            get
            {
                return team;
            }
            set
            {
                if (team == value)
                    return;

                team = value;
                Check();
            }
        }

        private string stadium;
        public string Stadium
        {
            get
            {
                return stadium;
            }
            set
            {
                if (stadium == value)
                    return;

                stadium = value;
                Check();
            }
        }

        private string result;
        public string ResultVal
        {
            get
            {
                return result;
            }
            set
            {
                if (result == value)
                    return;

                result = value;
                Check();
            }
        }

        private string tournaments;
        public string Tournaments
        {
            get
            {
                return tournaments;
            }
            set
            {
                if (tournaments == value)
                    return;

                tournaments = value;
                Check();
            }
        }
        
        private decimal? revenue;
        public decimal? Revenue
        {
            get
            {
                return revenue;
            }
            set
            {
                if (revenue == value)
                    return;

                revenue = value;
                Check();
            }
        }

        public List<string> ListTours { get; set; }
        public List<string> Result { get; set; }
        private void CheckLetters(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            try
            {
                e.Handled = !Char.IsLetter(Convert.ToChar(e.Text));
            }
            catch { }
        }//revenue
        
        private void CheckDigits(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            try
            {
                e.Handled = Char.IsLetter(Convert.ToChar(e.Text));
            }
            catch { }
        }//revenue
        public void Check() => this.IsPrimaryButtonEnabled = !(/*Time.Date.Year < 2000 || */(Date.Date.Year < 2000) || Revenue is null || string.IsNullOrEmpty(Stadium) || string.IsNullOrEmpty(Team) || string.IsNullOrEmpty(Tournaments) || string.IsNullOrEmpty(ResultVal));
    }
}
