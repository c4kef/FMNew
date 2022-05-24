﻿using ClosedXML.Excel;
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
    public partial class GamesSchedule
    {
        private CollectionViewSource _cvs;
        DataTable dt, dt_t;
        SqlDataAdapter adapter, adapter_t;

        public List<string> TList { get; set; }
        public List<string> RList { get; set; }

        private string tval;
        private string rval;
        public string TVal
        {
            get
            {
                return tval;
            }
            set
            {
                tval = value;

                foreach (DataRowView dr in dataGrid.ItemsSource)
                {
                    (dataGrid.ItemContainerGenerator.ContainerFromItem(dr) as DataGridRow).Visibility = Visibility.Visible;

                    if (value != "Без фильтра" && !dr[4].ToString().ToLower().Contains(value.ToLower()))
                        (dataGrid.ItemContainerGenerator.ContainerFromItem(dr) as DataGridRow).Visibility = Visibility.Collapsed;
                }
            }
        }
        public string RVal
        {
            get
            {
                return rval;
            }
            set
            {
                rval = value;

                foreach (DataRowView dr in dataGrid.ItemsSource)
                { 
                    (dataGrid.ItemContainerGenerator.ContainerFromItem(dr) as DataGridRow).Visibility = Visibility.Visible;

                    if (value != "Без фильтра" && dr[5].ToString().ToLower() != value.ToLower())
                        (dataGrid.ItemContainerGenerator.ContainerFromItem(dr) as DataGridRow).Visibility = Visibility.Collapsed;
                }
            }
        }

        public GamesSchedule()
        {
            Loaded += OnLoaded;
            TList = new List<string>();
            var dt_t = new DataTable();
            new SqlDataAdapter(new SqlCommand("SELECT * FROM tournaments", Globals.connection)).Fill(dt_t);
            foreach (DataRow row in dt_t.Rows)
                TList.Add((string)row.ItemArray[1]);
            TList.Add("Без фильтра");

            RList = new List<string>() { "Выигран", "Не выигран", "Не играли", "Без фильтра" };

            InitializeComponent();
            this.DataContext = this;

            _cvs = new CollectionViewSource();
        }

        public void FillGrid()
        {
            SqlCommand cmdSel = new SqlCommand("SELECT ID_game_shedule, date, team, stadium, Tournaments.name, result, ticket_count, revenue FROM Gamesschedule join Tournaments ON (Gamesschedule.ID_tournament = Tournaments.ID_tournament)", Globals.connection);
            adapter = new SqlDataAdapter(cmdSel);
            dt = new DataTable();
            adapter.Fill(dt);
            dataGrid.ItemsSource = dt.DefaultView;
            
            cmdSel = new SqlCommand("SELECT * FROM tournaments", Globals.connection);
            adapter_t = new SqlDataAdapter(cmdSel);
            dt_t = new DataTable();
            adapter_t.Fill(dt_t);
            dataGridTrn.ItemsSource = dt_t.DefaultView;
        }

        private void Export(object sender, RoutedEventArgs e)
        {
            var wb = new XLWorkbook();

            var dd = dt;
            dd.TableName = "Data";
            dd.Columns.Remove("ID_game_shedule");

            wb.Worksheets.Add(dd);
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
        
        private void ExportT(object sender, RoutedEventArgs e)
        {
            var wb = new XLWorkbook();

            var dd = dt_t;
            
            dd.TableName = "Data";
            dd.Columns.Remove("ID_tournament");

            wb.Worksheets.Add(dd);
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
        
        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;
            FillGrid();
            dt.RowChanged += Dt_RowSaved;
            dt.RowDeleted += Dt_RowSaved;
            
            dt_t.RowChanged += Dt_RowSaved;
            dt_t.RowDeleted += Dt_RowSaved;
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
                
                swl = new SqlCommandBuilder(adapter_t);
                adapter.Update(dt_t);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void Add(object sender, RoutedEventArgs e)
        {
            GamesScheduleDialog dialog = new GamesScheduleDialog();

            var dt_t = new DataTable();
            new SqlDataAdapter(new SqlCommand("SELECT * FROM tournaments", Globals.connection)).Fill(dt_t);
            foreach (DataRow row in dt_t.Rows)
                dialog.ListTours.Add((string)row.ItemArray[1]);

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                await new SqlCommand($@"INSERT INTO gamesschedule (date, team, stadium, ID_tournament, result, ticket_count, revenue) VALUES (N'{dialog.Date}', N'{dialog.Team}', N'{dialog.Stadium}', (select ID_tournament from tournaments where name = N'{dialog.Tournaments}'), N'{dialog.ResultVal}', '0', '{dialog.Revenue}')", Globals.connection).ExecuteNonQueryAsync();
                FillGrid();
            }
        }
        
        private async void AddT(object sender, RoutedEventArgs e)
        {
            GamesScheduleTrnDialog dialog = new GamesScheduleTrnDialog();

            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
            {
                await new SqlCommand($@"INSERT INTO tournaments VALUES (N'{dialog.TName}')", Globals.connection).ExecuteNonQueryAsync();
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

            GamesScheduleDialog dialog = new GamesScheduleDialog();
            object[] cells = dt.Rows[dataGrid.SelectedIndex].ItemArray;

            dialog.Date = DateTime.Parse(cells[1].ToString());
            dialog.Team = (string)cells[2];
            dialog.Stadium = (string)cells[3];
            dialog.Tournaments = (string) cells[4];
            dialog.ResultVal = (string) cells[5];
            dialog.Revenue = decimal.Parse(cells[7].ToString());

            var dt_t = new DataTable();
            new SqlDataAdapter(new SqlCommand("SELECT * FROM tournaments", Globals.connection)).Fill(dt_t);
            foreach (DataRow row in dt_t.Rows)
                dialog.ListTours.Add((string)row.ItemArray[1]);

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                await new SqlCommand($@"UPDATE gamesschedule SET date = N'{dialog.Date}', team = N'{dialog.Team}', stadium = N'{dialog.Stadium}', ID_tournament = (select ID_tournament from tournaments where name = N'{dialog.Tournaments}'), result = N'{dialog.ResultVal}', revenue = '{dialog.Revenue}' WHERE ID_game_shedule = '{cells[0]}'", Globals.connection).ExecuteNonQueryAsync();
                FillGrid();
            }
        }
        
        private async void EditT(object sender, RoutedEventArgs e)
        {
            if (dataGridTrn.SelectedItem is null)
            {
                MessageBox.Show("Выберите запись для редактирования!", "Ошибка!");
                return;
            }

            GamesScheduleTrnDialog dialog = new GamesScheduleTrnDialog();
            object[] cells = dt_t.Rows[dataGridTrn.SelectedIndex].ItemArray;

            dialog.TName = (string) cells[1];

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                await new SqlCommand($@"UPDATE tournaments SET name = N'{dialog.TName}' WHERE ID_tournament = '{cells[0]}'", Globals.connection).ExecuteNonQueryAsync();
                FillGrid();
                
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
                await new SqlCommand($@"DELETE FROM gamesschedule WHERE ID_game_shedule = '{dt.Rows[dataGrid.SelectedIndex].ItemArray[0]}'", Globals.connection).ExecuteNonQueryAsync();
                FillGrid();
                
            }
        }
        
        private async void RemoveT(object sender, RoutedEventArgs e)
        {
            if (dataGridTrn.SelectedItem is null)
            {
                MessageBox.Show("Выберите запись для удаления!", "Ошибка!");
                return;
            }

            MessageBoxResult res = MessageBox.Show("Вы уверены, что хотите удалить данную запись?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res != MessageBoxResult.No)
            {
                try
                {
                    await new SqlCommand(
                        $@"DELETE FROM tournaments WHERE ID_tournament = '{dt_t.Rows[dataGridTrn.SelectedIndex].ItemArray[0]}'",
                        Globals.connection).ExecuteNonQueryAsync();
                    FillGrid();
                }
                catch
                {
                    MessageBox.Show("Упс, похоже ты забыл удалить из графика игр");
                }
            }
        }

        private void See(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem is null)
            {
                MessageBox.Show("Выберите что посмотреть", "Ошибка!");
                return;
            }

            object[] cells = dt.Rows[dataGrid.SelectedIndex].ItemArray;

            if ((string) cells[5] == "Не играли")
            {
                MessageBox.Show("Игра еще не проводилась");
                return;
            }
            
            MessageBox.Show($"{DateTime.Parse(cells[1].ToString())}\n{(string)cells[3]}\n{(string)cells[4]}");
        }
    }
}
