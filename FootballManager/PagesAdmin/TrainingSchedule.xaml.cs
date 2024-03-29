﻿using FootballManager;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using ModernWpf.Controls;
using FootballManager.Common;
using ClosedXML.Excel;
using Microsoft.Win32;

namespace FootballManager.PagesAdmin
{
    public partial class TrainingSchedule
    {
        private CollectionViewSource _cvs;
        DataTable dt;
        SqlDataAdapter adapter;

        public TrainingSchedule()
        {
            Loaded += OnLoaded;

            InitializeComponent();
            _cvs = new CollectionViewSource();
        }

        public void FillGrid()
        {
            SqlCommand cmdSel = new SqlCommand("SELECT * FROM trainingschedule ORDER BY ID_training_schedule", Globals.connection);
            adapter = new SqlDataAdapter(cmdSel);
            dt = new DataTable();
            adapter.Fill(dt);
            dataGrid.ItemsSource = dt.DefaultView;

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
        private void Export(object sender, RoutedEventArgs e)
        {
            var wb = new XLWorkbook();
            dt.Columns.Remove("ID_training_schedule");

            dt.TableName = "Data";
            
            for (int i = 1; i < dataGrid.Columns.Count; i++)
                dt.Columns[i - 1].ColumnName = dataGrid.Columns[i].Header.ToString();

            wb.Worksheets.Add(dt);
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.AddExtension = true;
            saveDialog.Filter = "(*.xlsx)|*.xlsx";

            Nullable<bool> result = saveDialog.ShowDialog();

            if (result == true)
            {
                wb.SaveAs(saveDialog.FileName);
                MessageBox.Show("Отчёт готов!");
            }
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

        private async void Add(object sender, RoutedEventArgs e)
        {
            TrainingScheduleDialog dialog = new TrainingScheduleDialog();
            
            var dt_t = new DataTable();
            
            new SqlDataAdapter(new SqlCommand("SELECT * FROM trainingschedule", Globals.connection)).Fill(dt_t);
            foreach (DataRow row in dt_t.Rows)
                dialog.blackListTimes.Add(DateTime.Parse(row.ItemArray[1].ToString()));
            
            dt_t = new DataTable();
            
            new SqlDataAdapter(new SqlCommand("SELECT * FROM gamesschedule ", Globals.connection)).Fill(dt_t);
            foreach (DataRow row in dt_t.Rows)
                dialog.blackListTimes.Add(DateTime.Parse(row.ItemArray[1].ToString()));
            
            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                await new SqlCommand($@"INSERT INTO trainingschedule (date,  location) VALUES (N'{dialog.Date.Date + dialog.Time.TimeOfDay}', N'{dialog.Location}')", Globals.connection).ExecuteNonQueryAsync();
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

            TrainingScheduleDialog dialog = new TrainingScheduleDialog();
            object[] cells = dt.Rows[dataGrid.SelectedIndex].ItemArray;

            dialog.Date = DateTime.Parse(cells[1].ToString());
            dialog.Location = (string)cells[2];

            var dt_t = new DataTable();
            
            new SqlDataAdapter(new SqlCommand("SELECT * FROM trainingschedule", Globals.connection)).Fill(dt_t);
            foreach (DataRow row in dt_t.Rows)
                if (DateTime.Parse(cells[1].ToString()).Date != DateTime.Parse(row.ItemArray[1].ToString()).Date)
                    dialog.blackListTimes.Add(DateTime.Parse(row.ItemArray[1].ToString()));
            
            dt_t = new DataTable();
            
            new SqlDataAdapter(new SqlCommand("SELECT * FROM gamesschedule ", Globals.connection)).Fill(dt_t);
            foreach (DataRow row in dt_t.Rows)
                    dialog.blackListTimes.Add(DateTime.Parse(row.ItemArray[1].ToString()));
            
            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                await new SqlCommand($@"UPDATE trainingschedule SET date = N'{dialog.Date.Date + dialog.Time.TimeOfDay}', location = N'{dialog.Location}' WHERE ID_training_schedule = '{cells[0]}'", Globals.connection).ExecuteNonQueryAsync();
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
                await new SqlCommand($@"DELETE FROM trainingschedule WHERE ID_training_schedule = '{dt.Rows[dataGrid.SelectedIndex].ItemArray[0]}'", Globals.connection).ExecuteNonQueryAsync();
                FillGrid();
                
            }
        }
        
        private void Calendar_OnSelectedDatesChanged(object sender, CalendarModeChangedEventArgs calendarModeChangedEventArgs)
        {
            if (dataGrid.ItemsSource is null)
                return;

            foreach (DataRowView dr in dataGrid.ItemsSource)
            {
                (dataGrid.ItemContainerGenerator.ContainerFromItem(dr) as DataGridRow).Visibility = Visibility.Visible;

                if (dteSelectedMonth.DisplayDate.Month != DateTime.Parse(dr[1].ToString()).Month)
                    (dataGrid.ItemContainerGenerator.ContainerFromItem(dr) as DataGridRow).Visibility = Visibility.Collapsed;
            }
            
            dteSelectedMonth.DisplayMode = CalendarMode.Year;
        }
    }
}
