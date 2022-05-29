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
using System.IO;

namespace FootballManager
{
    public partial class Window1 : Window
    {
        public Window1()
        {
            //DESKTOP-B4OPU5P\SQLEXPRESS
            //C4ke
            Globals.connection = new SqlConnection(@"Data Source=C4ke;Initial Catalog=footballclub1; Integrated Security = True; MultipleActiveResultSets=True");
            Globals.connection.Open();

            if (File.Exists("money.txt"))
                Globals.Balance = int.Parse(File.ReadAllText("money.txt"));
            
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

            var reader = new SqlCommand($"SELECT * FROM manager WHERE login = N'{Login.Text}' AND password = N'{Password.Password}'", Globals.connection).ExecuteReader();
            if (reader.HasRows)
            {
                Auth.Visibility = Visibility.Hidden;
                Globals.UserLogin = "Менеджер";
                Globals.isManager = true;
                new MainWindow().Show();
                this.Close();
                return;
            }

            reader = new SqlCommand($"SELECT * FROM playerList WHERE login = N'{Login.Text}' AND pass = N'{Password.Password}'", Globals.connection).ExecuteReader();
            if (reader.HasRows)
            {
                Auth.Visibility = Visibility.Hidden;
                Globals.UserLogin = "Игрок";
                Globals.isManager = false;
                new MainWindow().Show();
                this.Close();
                return;
            }

            MessageBox.Show("логин или пароль не корректен");
        }
    }
}
