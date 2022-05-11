using ModernWpf.Controls;
using System;
using System.ComponentModel;
using System.Linq;

namespace FootballManager.PagesAdmin
{
    [Serializable]
    public partial class PlayerListDialog : ContentDialog
    {
        public PlayerListDialog()
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

        private void CheckLetters(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            try
            {
                e.Handled = !Char.IsLetter(Convert.ToChar(e.Text));}
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
        public void Check() => this.IsPrimaryButtonEnabled = !(string.IsNullOrEmpty(Patronymic) || Patronymic.Any(char.IsDigit) || string.IsNullOrEmpty(Surname) || Surname.Any(char.IsDigit) || string.IsNullOrEmpty(MName) || MName.Any(char.IsDigit) || string.IsNullOrEmpty(Nationality) || (Age == null) || string.IsNullOrEmpty(Phone) || string.IsNullOrEmpty(Position));
    }
}
