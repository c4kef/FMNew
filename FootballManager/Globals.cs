using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace FootballManager
{
    public partial class Globals
    {
        public static SqlConnection connection;

        public static bool isManager = true;

        public static event PropertyChangedEventHandler StaticPropertyChanged;

        private static void OnStaticPropertyChanged(string propertyName)
        {
            StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
        }

        public static string UserLogin { get; set; }


        private static decimal _balance = 120_000;
        private static decimal _last_balance;
        public static decimal Balance
        {
            get => _balance;
            set
            {
                _last_balance = _balance;
                new Thread(new ThreadStart(() => MessageBox.Show($"Статус: {((_last_balance < _balance) ? "Пополнение" : "Расход")}\nСумма: {_balance - _last_balance}"))).Start();
                _balance = value;
                OnStaticPropertyChanged("Balance");
            }
        }

        public static void AddOperation(DateTime dateTime, string description, decimal newValue, decimal value)
        {
            async void Action() => await new SqlCommand($"INSERT INTO operations(status, datetime, description, sum) VALUES (N'{((newValue > _balance) ? "Пополнение" : "Расход")}', '{dateTime.ToString(CultureInfo.CurrentCulture)}', N'{description}', '{value}')", connection).ExecuteNonQueryAsync();

            new Task(Action).RunSynchronously();
        }
    }
}
