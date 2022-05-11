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
using ClosedXML.Excel;
using Microsoft.Win32;

namespace FootballManager.PagesAdmin
{
    public partial class Tickets
    {
        private CollectionViewSource _cvs;
        DataTable dt;
        SqlDataAdapter adapter;

        public Tickets()
        {
            Loaded += OnLoaded;

            InitializeComponent();

            _cvs = new CollectionViewSource();
        }

        public void FillGrid()
        {
            SqlCommand cmdSel = new SqlCommand("SELECT * FROM gamesschedule WHERE ticket_price != 0 ORDER BY ID_game_shedule ASC", Globals.connection);
            adapter = new SqlDataAdapter(cmdSel);
            dt = new DataTable();
            adapter.Fill(dt);
            dataGrid.ItemsSource = dt.DefaultView;
            dataGrid.DataContext = this;

        }
        private void Export(object sender, RoutedEventArgs e)
        {
            var wb = new XLWorkbook();

            dt.Columns.Remove("ID_tournament");

            dt.TableName = "Data";
            wb.Worksheets.Add(dt);
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
            TicketsDialog dialog = new TicketsDialog();
            dialog.DateList = new List<string>();

            var dt_g = new DataTable();
            new SqlDataAdapter(new SqlCommand("SELECT * FROM gamesschedule WHERE ticket_price = 0 ORDER BY ID_game_shedule ASC", Globals.connection)).Fill(dt_g);

            foreach (DataRow row in dt_g.Rows)
                dialog.DateList.Add(row.ItemArray[1].ToString());

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                await new SqlCommand(
                    $@"UPDATE gamesschedule SET ticket_price = N'{dialog.Price}', ticket_count = N'{dialog.Count}' WHERE date = '{dialog.Date}'",
                    Globals.connection).ExecuteNonQueryAsync();
                FillGrid();
                var sum = (decimal) dialog.Price * (int) dialog.Count;
                Globals.AddOperation(DateTime.Now, "Продажа билетов", Globals.Balance + sum, sum);
                Globals.Balance += sum;
            }
        }

        private async void Edit(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem is null)
            {
                MessageBox.Show("Выберите запись для редактирования!", "Ошибка!");
                return;
            }

            TicketsDialog dialog = new TicketsDialog();
            dialog.DateList = new List<string>();
            object[] cells = dt.Rows[dataGrid.SelectedIndex].ItemArray;

            dialog.Date = cells[0].ToString();

            var dt_g = new DataTable();
            new SqlDataAdapter(new SqlCommand("SELECT * FROM gamesschedule ORDER BY ID_game_shedule ASC", Globals.connection)).Fill(dt_g);

            foreach (DataRow row in dt_g.Rows)
                dialog.DateList.Add(row.ItemArray[1].ToString());

            dialog.Price = (decimal)cells[7];
            dialog.Count = (int)cells[8];

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                try
                {
                    if (DateTime.Parse(dialog.Date).Date.Year < 2000)
                        new Exception("Year not corrected");

                    await new SqlCommand($@"UPDATE gamesschedule SET ticket_price = N'{dialog.Price}', ticket_count = N'{dialog.Count}' WHERE ID_game_shedule = '{cells[0]}'", Globals.connection).ExecuteNonQueryAsync();
                    FillGrid();
                    
                }
                catch
                {
                    MessageBox.Show("Вы заполнили не все поля, попробуйте еще раз");
                }
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
                await new SqlCommand($@"UPDATE gamesschedule SET ticket_price = N'0', ticket_count = N'0' WHERE ID_game_shedule = '{dt.Rows[dataGrid.SelectedIndex].ItemArray[0]}'", Globals.connection).ExecuteNonQueryAsync();
                FillGrid();
                
            }
        }
    }
}
