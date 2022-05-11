using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FootballManager.PagesAdmin
{
    public struct EmployeesData
    {
        public int id { get; set; }
        public string surname { get; set; }
    }

    public struct SporteQuipmentData
    {
        public int id { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
    }

    [Serializable]
    public partial class OrderSOEDialog : ContentDialog, INotifyPropertyChanged
    {
        public OrderSOEDialog()
        {
            InitializeComponent();
            this.DataContext = this;
            Status = new List<string>();
            Status.Add("Выполнен");
            Status.Add("Не выполнен");
            OnPropertyChanged("Status");
            Check();
        }

        private DateTime? dateordercreated = DateTime.Now;
        public DateTime? DateOrderCreated
        {
            get
            {
                return dateordercreated;
            }
            set
            {
                if (value == dateordercreated)
                    return;

                dateordercreated = value;
                Check();
            }
        }

        private DateTime? dateorderended = DateTime.Now;
        public DateTime? DateOrderEnded
        {
            get
            {
                return dateorderended;
            }
            set
            {
                if (value == dateorderended)
                    return;

                dateorderended = value;
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
                Calculate();
                Check();
            }
        }

        public List<string> Status { get; set; }

        public string StatusValue { get; set; }

        public List<EmployeesData> Employees { get; set; }
        private EmployeesData employees;
        public EmployeesData EmployeesValue
        {
            get
            {
                return employees;
            }
            set
            {
                employees = value;
                Check();
            }
        }

        public List<SporteQuipmentData> SporteQuipment { get; set; }
        private SporteQuipmentData sportequipment;
        public SporteQuipmentData SporteQuipmentValue
        {
            get
            {
                return sportequipment;
            }
            set
            {
                sportequipment = value;
                Calculate();
                Check();
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void Calculate()
        {
            if (Count is null)
                return;

            Balance.Content = $"Траты: {Count * SporteQuipmentValue.price}";
        }
        private void CheckDigits(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            try
            {
                e.Handled = System.Text.RegularExpressions.Regex.IsMatch(e.Text, "[^0-9]");
            }
            catch { }
        }
        public void Check() => this.IsPrimaryButtonEnabled = !(string.IsNullOrEmpty(StatusValue) || Count == null || DateOrderEnded.Value.Date.Year < 2000 || DateOrderCreated.Value.Date.Year < 2000 || EmployeesValue.Equals(default(EmployeesData)) || SporteQuipmentValue.Equals(default(SporteQuipmentData)));
    }
}
