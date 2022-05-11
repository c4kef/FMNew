using ModernWpf.Controls;
using System;
using System.ComponentModel;
using System.Linq;

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

        private int? age;
        public int? Age
        {
            get
            {
                return age;
            }
            set
            {
                if (age == value)
                    return;

                age = value;
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
        public void Check() => this.IsPrimaryButtonEnabled = !(age == null || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(Position) || string.IsNullOrEmpty(Patronymic) || Patronymic.Any(char.IsDigit) || string.IsNullOrEmpty(Surname) || Surname.Any(char.IsDigit) || string.IsNullOrEmpty(EName) || EName.Any(char.IsDigit));
    }
}
