using FootballManager;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
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
    public partial class OperationsPlayers
    {
        private DataTable dt_p, dt_o;
        private SqlDataAdapter adapter_oe, adapter_se;

        public OperationsPlayers()
        {
            Loaded += OnLoaded;

            InitializeComponent();
        }

        public void FillGrid()
        {
            dt_p = new DataTable();
            dt_o = new DataTable();
//
            adapter_oe = new SqlDataAdapter(new SqlCommand("SELECT ID_BuyPlayer, surname, datebuy, sumbuy FROM buyplayer", Globals.connection));
            adapter_oe.Fill(dt_p);

            adapter_se = new SqlDataAdapter(new SqlCommand("SELECT ID_SellPlayer, surname, datesell, sumsell FROM sellplayer", Globals.connection));
            adapter_se.Fill(dt_o);

            dataGridOrderP.ItemsSource = dt_p.DefaultView;
            dataGridOrderO.ItemsSource = dt_o.DefaultView;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;
            FillGrid();
            dt_p.RowChanged += (o, oo) => { SaveGridOE(); };
            dt_p.RowDeleted += (o, oo) => { SaveGridOE(); };

            dt_o.RowChanged += (o, oo) => { SaveGridSE(); };
            dt_o.RowDeleted += (o, oo) => { SaveGridSE(); };
        }
        private void ExportP(object sender, RoutedEventArgs e)
        {
            var wb = new XLWorkbook();
            var dtP = dt_p;
            //dtP.Columns.Remove("id");
            dtP.TableName = "Data";
            wb.Worksheets.Add(dtP);
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


        private void ExportO(object sender, RoutedEventArgs e)
        {
            var wb = new XLWorkbook();
            var dtP = dt_o;
            //dtP.Columns.Remove("id");
            dtP.TableName = "Data";
            wb.Worksheets.Add(dtP);
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


        private void SaveGridOE()
        {
            try
            {
                var swl = new SqlCommandBuilder(adapter_oe);
                adapter_oe.Update(dt_p);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SaveGridSE()
        {
            try
            {
                var swl = new SqlCommandBuilder(adapter_se);
                adapter_se.Update(dt_o);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //PlayersAdd
        public static async Task AddP(string surname, decimal sum) => await new SqlCommand($@"INSERT INTO buyplayer (datebuy, sumbuy, surname) VALUES ('{DateTime.Now.Date}', '{sum}', N'{surname}')",Globals.connection).ExecuteNonQueryAsync();
        public static async Task AddO(string surname, decimal sum) => await new SqlCommand($"INSERT INTO SellPlayer (datesell, sumsell, surname) VALUES ('{DateTime.Now.Date}', '{sum}', N'{surname}')",Globals.connection).ExecuteNonQueryAsync();
            //await new SqlCommand($@"INSERT INTO playersOrder (ID_Player, date, sum) VALUES ((select ID_Player from playerlist_orders where surname = N'{pid}'), N'{DateTime.Now.Date.ToString("MM/dd/yyyy")}', {sum})", Globals.connection).ExecuteNonQueryAsync();

        private void SearchO(object sender, KeyEventArgs e)
        {
            foreach (DataRowView dr in dataGridOrderO.ItemsSource)
            {
                (dataGridOrderO.ItemContainerGenerator.ContainerFromItem(dr) as DataGridRow).Visibility = Visibility.Visible;

                if (SearchUIO.Text != string.Empty && !dr[4].ToString().ToLower().Contains(SearchUIO.Text.ToLower()))
                    (dataGridOrderO.ItemContainerGenerator.ContainerFromItem(dr) as DataGridRow).Visibility = Visibility.Collapsed;
            }
        }

        private async void EditP(object sender, RoutedEventArgs e)
        {
            if (dataGridOrderP.SelectedItem is null)
            {
                MessageBox.Show("Выберите запись для редактирования!", "Ошибка!");
                return;
            }

            OrderP dialog = new OrderP();
            object[] cells = dt_p.Rows[dataGridOrderP.SelectedIndex].ItemArray;

            dialog.Player = (string) cells[4];
            dialog.Date = DateTime.Parse(cells[2].ToString());
            dialog.Sum = (int)cells[3];
            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                await new SqlCommand(
                    $@"UPDATE buyplayer SET surname = N'{dialog.Player}', datebuy = '{dialog.Date.Value.Date}', sumbuy '{dialog.Sum}' WHERE ID_BuyPlayer = '{cells[0]}'",
                    Globals.connection).ExecuteNonQueryAsync();
                FillGrid();
            }
        }

        private async void RemoveP(object sender, RoutedEventArgs e)
        {
            if (dataGridOrderP.SelectedItem is null)
            {
                MessageBox.Show("Выберите запись для удаления!", "Ошибка!");
                return;
            }

            MessageBoxResult res = MessageBox.Show("Вы уверены, что хотите удалить данную запись?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res != MessageBoxResult.No)
            {
                await new SqlCommand($@"DELETE FROM buyplayer WHERE ID_BuyPlayer = '{dt_p.Rows[dataGridOrderP.SelectedIndex].ItemArray[0]}'", Globals.connection).ExecuteNonQueryAsync();
                FillGrid();
                
            }
        }

        private async void EditO(object sender, RoutedEventArgs e)
        {
            if (dataGridOrderP.SelectedItem is null)
            {
                MessageBox.Show("Выберите запись для редактирования!", "Ошибка!");
                return;
            }

            OrderP dialog = new OrderP();
            object[] cells = dt_p.Rows[dataGridOrderP.SelectedIndex].ItemArray;

            dialog.Date = DateTime.Parse(cells[2].ToString());
            dialog.Player = (string) cells[4];
            dialog.Sum = (int) cells[3];

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                await new SqlCommand(
                    $@"UPDATE sellplayer SET surname = N'{dialog.Player}', datesell = '{dialog.Date.Value.Date}', sumsell = '{dialog.Sum}' WHERE ID_SellPlayer = '{cells[0]}'",
                    Globals.connection).ExecuteNonQueryAsync();
                FillGrid();
            }
        }

        private async void RemoveO(object sender, RoutedEventArgs e)
        {
            if (dataGridOrderO.SelectedItem is null)
            {
                MessageBox.Show("Выберите запись для удаления!", "Ошибка!");
                return;
            }

            MessageBoxResult res = MessageBox.Show("Вы уверены, что хотите удалить данную запись?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res != MessageBoxResult.No)
            {
                await new SqlCommand($@"DELETE FROM SellPlayer WHERE ID_SellPlayer = '{dt_o.Rows[dataGridOrderO.SelectedIndex].ItemArray[0]}'", Globals.connection).ExecuteNonQueryAsync();
                FillGrid();
                
            }
        }

        private void SearchP(object sender, KeyEventArgs e)
        {
            foreach (DataRowView dr in dataGridOrderP.ItemsSource)
            {
                (dataGridOrderP.ItemContainerGenerator.ContainerFromItem(dr) as DataGridRow).Visibility = Visibility.Visible;

                if (SearchUIP.Text != string.Empty && !dr[4].ToString().ToLower().Contains(SearchUIP.Text.ToLower()) && !dr[4].ToString().ToLower().Contains(SearchUIP.Text.ToLower()))
                    (dataGridOrderP.ItemContainerGenerator.ContainerFromItem(dr) as DataGridRow).Visibility = Visibility.Collapsed;
            }
        }
    }
}
