using FootballManager;
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
            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                await new SqlCommand($@"INSERT INTO trainingschedule (date, time, location) VALUES (N'{dialog.Date}', N'{dialog.Time}', N'{dialog.Location}')", Globals.connection).ExecuteNonQueryAsync();
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

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                await new SqlCommand($@"UPDATE trainingschedule SET date = N'{dialog.Date}', location = N'{dialog.Location}' WHERE ID_training_schedule = '{cells[0]}'", Globals.connection).ExecuteNonQueryAsync();
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
    }
}
