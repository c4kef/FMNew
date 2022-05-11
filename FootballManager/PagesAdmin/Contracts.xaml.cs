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
    public partial class Contracts
    {
        private CollectionViewSource _cvs;
        DataTable dt;
        SqlDataAdapter adapter;

        public Contracts()
        {
            Loaded += OnLoaded;

            InitializeComponent();
            _cvs = new CollectionViewSource();
        }

        public void FillGrid()
        {
            SqlCommand cmdSel = new SqlCommand("SELECT ID_Contract, date_start, date_ended, month_pay, PlayerList.surname FROM contracts join playerList ON (Contracts.ID_Player = playerlist.ID_Player)", Globals.connection);
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
            ContractsDialog dialog = new ContractsDialog();

            var dt_p = new DataTable();

            new SqlDataAdapter(new SqlCommand("SELECT * FROM playerlist", Globals.connection)).Fill(dt_p);

            //Тут заполняем всех игроков по фамилии
            foreach (DataRow row in dt_p.Rows)
                dialog.PList.Add(row.ItemArray[1].ToString());

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                await new SqlCommand(
                    $@"INSERT INTO contracts (date_start, date_ended, month_pay, ID_Player) VALUES (N'{dialog.DateStart.Date}', N'{dialog.DateEnd.Date}', '{dialog.MonthPay}', (select ID_Player from PlayerList where surname = N'{dialog.PValue}'))",
                    Globals.connection).ExecuteNonQueryAsync();
                FillGrid();
            }
        }

        private void Search(object sender, KeyEventArgs e)
        {
            foreach (DataRowView dr in dataGrid.ItemsSource)
            {
                (dataGrid.ItemContainerGenerator.ContainerFromItem(dr) as DataGridRow).Visibility = Visibility.Visible;

                if (SearchUI.Text != string.Empty && !dr[4].ToString().ToLower().Contains(SearchUI.Text.ToLower()))
                    (dataGrid.ItemContainerGenerator.ContainerFromItem(dr) as DataGridRow).Visibility = Visibility.Collapsed;
            }
        }

        private void Export(object sender, RoutedEventArgs e)
        {
            var wb = new XLWorkbook();
            var dtt = dt;
            dtt.Columns.Remove("ID_Contract");
            dtt.TableName = "Data";
            wb.Worksheets.Add(dtt);
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.AddExtension = true;
            saveDialog.Filter = "(*.xlsx)|*.xlsx";

            Nullable<bool> result = saveDialog.ShowDialog();

            if (result == true)
            {
                wb.SaveAs(saveDialog.FileName);
                MessageBox.Show("Успех!");
            }
        }

        private async void Remove(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem is null)
            {
                MessageBox.Show("Выберите запись для удаления!", "Ошибка!");
                return;
            }

            MessageBoxResult res = MessageBox.Show("Вы уверены что хотите удалить запись?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res != MessageBoxResult.No)
            {
                await new SqlCommand($@"DELETE FROM contracts WHERE ID_Contract = '{dt.Rows[dataGrid.SelectedIndex].ItemArray[0]}'", Globals.connection).ExecuteNonQueryAsync();
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

            ContractsDialog dialog = new ContractsDialog();

            object[] cells = dt.Rows[dataGrid.SelectedIndex].ItemArray;

            dialog.DateStart = DateTime.Parse(cells[1].ToString());
            dialog.DateEnd = DateTime.Parse(cells[2].ToString());
            dialog.MonthPay = (decimal) cells[3];
            dialog.PValue = (string) cells[4];

            var dt_p = new DataTable();

            new SqlDataAdapter(new SqlCommand("SELECT * FROM playerlist", Globals.connection)).Fill(dt_p);

            foreach (DataRow row in dt_p.Rows)
                dialog.PList.Add(row.ItemArray[1].ToString());

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                await new SqlCommand(
                    $@"UPDATE contracts SET date_start = N'{dialog.DateStart.Date}', date_ended = N'{dialog.DateEnd.Date}', month_pay = '{dialog.MonthPay}', ID_Player = (select ID_Player from PlayerList where surname = N'{dialog.PValue}') WHERE ID_Contract = '{cells[0]}'",
                    Globals.connection).ExecuteNonQueryAsync();
                FillGrid();
            }
        }
    }
}
