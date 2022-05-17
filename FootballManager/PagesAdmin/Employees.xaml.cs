using ClosedXML.Excel;
using FootballManager;
using Microsoft.Win32;
using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace FootballManager.PagesAdmin
{
    public partial class Employees
    {
        private CollectionViewSource _cvs;
        DataTable dt;
        SqlDataAdapter adapter;

        public Employees()
        {
            Loaded += OnLoaded;

            InitializeComponent();
            _cvs = new CollectionViewSource();
        }

        public void FillGrid()
        {
            SqlCommand cmdSel = new SqlCommand("SELECT * FROM employees", Globals.connection);
            adapter = new SqlDataAdapter(cmdSel);
            dt = new DataTable();
            adapter.Fill(dt);
            dataGrid.ItemsSource = dt.DefaultView;

        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;
            FillGrid();
            dt.RowChanged += Dt_RowSaved;
            dt.RowDeleted += Dt_RowSaved;
        }

        private void Dt_RowSaved(object sender, DataRowChangeEventArgs e)
        {
            SaveGrid();
        }

        private void Export(object sender, RoutedEventArgs e)
        {
            var wb = new XLWorkbook();
            var dtT = dt;
            dtT.Columns.Remove("ID_Employee");
            dtT.TableName = "Data";
            wb.Worksheets.Add(dtT);
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.AddExtension = true;
            saveDialog.Filter = "(*.xlsx)|*.xlsx";

            Nullable<bool> result = saveDialog.ShowDialog();

            if (result == true)
            {
                wb.SaveAs(saveDialog.FileName);
                MessageBox.Show("Отчёт готов");
            }
        }
        private void SaveGrid()
        {
            try
            {
                var swl = new SqlCommandBuilder(adapter);
                adapter.Update(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void Add(object sender, RoutedEventArgs e)
        {
            EmployeesDialog dialog = new EmployeesDialog();
            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                //To-Do возможно ошибка в  N'{dialog.Age}', замени на  {dialog.Age}
                await new SqlCommand(
                    $@"INSERT INTO employees (name, surname, patronymic, position, phone, team, dateofbirth, nationality ) VALUES (N'{dialog.EName}', N'{dialog.Surname}', N'{dialog.Patronymic}', N'{dialog.Position}', N'{dialog.Phone}', N'{dialog.Team}',N'{dialog.Dateofbirth}')",
                    Globals.connection).ExecuteNonQueryAsync();
                FillGrid();
            }
        }

        private void Search(object sender, KeyEventArgs e)
        {
            foreach (DataRowView dr in dataGrid.ItemsSource)
            {
                (dataGrid.ItemContainerGenerator.ContainerFromItem(dr) as DataGridRow).Visibility = Visibility.Visible;

                if (SearchUI.Text != string.Empty && !dr[1].ToString().ToLower().Contains(SearchUI.Text.ToLower()) && !dr[2].ToString().ToLower().Contains(SearchUI.Text.ToLower()) && !dr[3].ToString().ToLower().Contains(SearchUI.Text.ToLower()))
                    (dataGrid.ItemContainerGenerator.ContainerFromItem(dr) as DataGridRow).Visibility = Visibility.Collapsed;
            }
        }

        private async void Remove(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem is null)
            {
                MessageBox.Show("Выберите запись для удаления!", "Ошибка!");
                return;
            }

            MessageBoxResult res = MessageBox.Show("Вы уверены, что хотите удалить данную запись?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res != MessageBoxResult.No)
            {
                await new SqlCommand($@"DELETE FROM employees WHERE ID_Employee = '{dt.Rows[dataGrid.SelectedIndex].ItemArray[0]}'", Globals.connection).ExecuteNonQueryAsync();
                FillGrid();
                
            }    
        }

        private async void Edit(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem is null)
            {
                MessageBox.Show("Выберите запись для редактирования!", "Ошибка!");
                return;
            }

            EmployeesDialog dialog = new EmployeesDialog();

            object[] cells = dt.Rows[dataGrid.SelectedIndex].ItemArray;

            dialog.EName = (string)cells[1];
            dialog.Surname = (string)cells[2];
            dialog.Patronymic = (string)cells[3];
            dialog.Dateofbirth = (string)cells[4];
            dialog.Position = (string)cells[5];
            dialog.Phone = (string)cells[6];
            dialog.Nationality = (string)cells[7];
            dialog.Team = (string)cells[8];
          

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                try
                {
                    await new SqlCommand($@"UPDATE employees SET name = N'{dialog.EName}', surname = N'{dialog.Surname}', patronymic = N'{dialog.Patronymic}', position = N'{dialog.Position}', phone = N'{dialog.Phone}', team = N'{dialog.Team}', nationality = N'{dialog.Nationality}' , dateofbirth = N'{dialog.Dateofbirth}', WHERE ID_Employee = '{cells[0]}'", Globals.connection).ExecuteNonQueryAsync();
                    FillGrid();
                    
                }
                catch
                {
                    MessageBox.Show("Вы заполнили не все поля, попробуйте еще раз");
                }
            }
        }
    }
}
