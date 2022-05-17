using FootballManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ClosedXML.Excel;
using FootballManager.PagesAdmin;
using Microsoft.Win32;
using System.Data.SqlClient;

namespace FootballManager
{
    public partial class Window1 : Window
    {
        public Window1()
        {
            Globals.connection = new SqlConnection(@"Data Source=FELIX-PC\SQLEXPRESS02;Initial Catalog=footballclub1; Integrated Security = True; MultipleActiveResultSets=True");
            Globals.connection.Open();
            InitializeComponent();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void LoginBtn(object sender, RoutedEventArgs e)
        {
            /*  if (Login.Text == "manager" && Password.Password == "1111")
              {
                  Auth.Visibility = Visibility.Hidden;
                  Globals.UserLogin = "Менеджер";
                  Globals.isManager = true;
                  new MainWindow().Show();
                  this.Close();
              }
              else if (Login.Text == "user" && Password.Password == "2222")
              {
                  Auth.Visibility = Visibility.Hidden;
                  Globals.UserLogin = "Игрок";
                  Globals.isManager = false;
                  new MainWindow().Show();
                  this.Close();
              }
              else
                  MessageBox.Show("Неверный логин или пароль!");*/

            SqlCommand bdsql = new SqlCommand($"SELECT * FROM manager WHERE login = N'{Login.Text}' AND password = N'{Password.Password}'", Globals.connection);

            SqlCommand check_not_valid = new SqlCommand($"INSERT INTO manager(login, password) values (N'{Login.Text}', N'{Password.Password}')",Globals.connection);
            SqlDataReader reader = bdsql.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                var login = reader.GetString(1);
                var password = reader.GetString(2);
                if (login == Login.Text && password == Password.Password)
                {
                    new MainWindow().Show();
                    this.Close();
                }
                else MessageBox.Show("логин или пароль не корректен");
            }
            else 
            {
                check_not_valid.ExecuteNonQuery();
                new MainWindow().Show();
                this.Close();
            }

        }
    }
}
