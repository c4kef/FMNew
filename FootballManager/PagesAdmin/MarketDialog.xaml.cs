﻿using ModernWpf.Controls;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;

namespace FootballManager.PagesAdmin
{
    [Serializable]
    public partial class MarketDialog : ContentDialog
    {
        public MarketDialog()
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

        private string surname;
        public string Surname
        {
            get
            {
                return surname;
            }
            set
            {
                if (surname == value)
                    return;

                surname = value;
                Check();
            }
        }

        private string patronymic;
        public string Patronymic
        {
            get
            {
                return patronymic;
            }
            set
            {
                if (patronymic == value)
                    return;

                patronymic = value;
                Check();
            }
        }

      

        private string nationality;
        public string Nationality
        {
            get
            {
                return nationality;
            }
            set
            {
                if (nationality == value)
                    return;

                nationality = value;
                Check();
            }
        }

        private string phone;
        public string Phone
        {
            get
            {
                return phone;
            }
            set
            {
                if (phone == value)
                    return;

                phone = value;
                Check();
            }
        }

        private string position;
        public string Position
        {
            get
            {
                return position;
            }
            set
            {
                if (position == value)
                    return;

                position = value;
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

                if (value == "Игрок клуба")
                    TeamBox.Visibility = Visibility.Collapsed;
                
                team = value;
                Check();
            }
        }

        private DateTime dateofbirth = DateTime.Now;
        public DateTime Dateofbirth
        {
            get
            {
                return dateofbirth;
            }
            set
            {
                if (dateofbirth == value)
                    return;

                dateofbirth = value;
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
        
        public void Check() => this.IsPrimaryButtonEnabled = !(string.IsNullOrEmpty(MName) || string.IsNullOrEmpty(Surname) || string.IsNullOrEmpty(Patronymic) || Dateofbirth.Date.Year < 2000 || string.IsNullOrEmpty(Nationality) || !Regex.IsMatch(Phone ?? string.Empty, @"^375(17|29|33|44)[0-9]{3}[0-9]{2}[0-9]{2}$") || string.IsNullOrEmpty(Position) || Price == null);
    }
}
