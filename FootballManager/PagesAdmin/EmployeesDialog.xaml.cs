using ModernWpf.Controls;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace FootballManager.PagesAdmin
{
    [Serializable]
    public partial class EmployeesDialog : ContentDialog
    {
        public EmployeesDialog()
        {
            InitializeComponent();
            this.DataContext = this;
            Check();
        }

        private string name;
        public string EName
        {
            get
            {
                return name;
            }
            set
            {
                if (name == value)
                    return;

                name = value;
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
        private string dateofbirth;
        public string Dateofbirth
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
        private string _position;
        public string Position
        {
            get
            {
                return _position;
            }
            set
            {
                if (value != _position)
                {
                    _position = value;
                    Check();
                }
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
        private string nationality;
        public string Nationality
        {
            get
            {
                return nationality;
            }
            set
            {
                if (nationality != value)
                    return;

                nationality = value;
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
        public void Check() => this.IsPrimaryButtonEnabled = !(string.IsNullOrEmpty(team) || string.IsNullOrEmpty(dateofbirth) || !Regex.IsMatch(Phone ?? string.Empty, @"^\+375 (17|29|33|44) [0-9]{3}-[0-9]{2}-[0-9]{2}$") || string.IsNullOrEmpty(Position) || string.IsNullOrEmpty(Patronymic) || Patronymic.Any(char.IsDigit) || string.IsNullOrEmpty(Surname) || Surname.Any(char.IsDigit) || string.IsNullOrEmpty(EName) || EName.Any(char.IsDigit));
    }
}
