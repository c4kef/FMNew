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
    public partial class Market
    {
        private CollectionViewSource _cvs;
        DataTable dt;
        SqlDataAdapter adapter;

        public Market()
        {
            Loaded += OnLoaded;

            InitializeComponent();

            _cvs = new CollectionViewSource();
        }

        public void FillGrid()
        {
            SqlCommand cmdSel = new SqlCommand("SELECT * FROM market ORDER BY ID_Market ASC", Globals.connection);
            adapter = new SqlDataAdapter(cmdSel);
            dt = new DataTable();
            adapter.Fill(dt);
            dataGrid.ItemsSource = dt.DefaultView;
            dataGrid.DataContext = this;

        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;
            FillGrid();
            dt.RowChanged += Dt_RowSaved;
            dt.RowDeleted += Dt_RowSaved;
        }

        private void Export(object sender, RoutedEventArgs e)
        {
            var wb = new XLWorkbook();
            dt.Columns.Remove("ID_Market");
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

        private async void BuyPlayer(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem is null)
            {
                MessageBox.Show("Выберите игрока для покупки!", "Ошибка!");
                return;
            }

            MessageBoxResult res = MessageBox.Show("Вы действительно хотите купить данного игрока?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res != MessageBoxResult.No)
            {
                var selectedPlayers = dataGrid.SelectedItems;
                decimal price = 0;
                foreach (DataRowView row in selectedPlayers)
                    price += decimal.Parse(row.Row.ItemArray[6].ToString());

                if (Globals.Balance - price >= 0)
                {
                    var name = string.Empty;
                    for (int i = 0; i < selectedPlayers.Count; i++)
                    {
                        name = ((DataRowView)selectedPlayers[i]).Row.ItemArray[1].ToString();

                        await new SqlCommand($"INSERT INTO playerlist(surname, name, patronymic, dateofbirth, position,  nationality,  phone, team) VALUES (N'{((DataRowView)selectedPlayers[i]).Row.ItemArray[1]}', N'{((DataRowView)selectedPlayers[i]).Row.ItemArray[2]}', N'{((DataRowView)selectedPlayers[i]).Row.ItemArray[3]}', N'{((DataRowView)selectedPlayers[i]).Row.ItemArray[4]}', N'{((DataRowView)selectedPlayers[i]).Row.ItemArray[5]}', N'{((DataRowView)selectedPlayers[i]).Row.ItemArray[6]}', N'{((DataRowView)selectedPlayers[i]).Row.ItemArray[7]}' , N'{((DataRowView)selectedPlayers[i]).Row.ItemArray[8]}')", Globals.connection).ExecuteNonQueryAsync();
                        await new SqlCommand($"DELETE FROM market WHERE ID_Market = '{((DataRowView)selectedPlayers[i]).Row.ItemArray[0]}'", Globals.connection).ExecuteNonQueryAsync();
                        FillGrid();
                    }
                    
                    Globals.AddOperation(DateTime.Now, "Покупка игрока", Globals.Balance - price, price);
                    Globals.Balance -= price;
                    
                    //To-Do
                    await OperationsPlayers.AddP(name, price);

                    MessageBox.Show("Игрок успешно приобретен!");
                }
                else
                    MessageBox.Show("Не достаточно средств!", "Ошибка!");
            }
        }

        private async void Add(object sender, RoutedEventArgs e)
        {
            MarketDialog dialog = new MarketDialog();
            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                try
                {
                    await new SqlCommand($@"INSERT INTO market (surname, name, patronymic, dateofbirth , team, nationality, price, phone, position) VALUES (N'{dialog.Surname}', N'{dialog.MName}', N'{dialog.Patronymic}', N'{dialog.Dateofbirth}',N'{dialog.Team}', N'{dialog.Nationality}', N'{dialog.Price}', N'{dialog.Phone}', N'{dialog.Position}')", Globals.connection).ExecuteNonQueryAsync();
                    FillGrid();
                    
                }
                catch
                {
                    MessageBox.Show("Вы заполнили не все поля, попробуйте еще раз");
                }
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

        private async void Edit(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem is null)
            {
                MessageBox.Show("Выберите запись для редактирования!", "Ошибка!");
                return;
            }

            MarketDialog dialog = new MarketDialog();
            object[] cells = dt.Rows[dataGrid.SelectedIndex].ItemArray;

            dialog.Surname = (string)cells[1];
            dialog.MName = (string)cells[2];
            dialog.Patronymic = (string)cells[3];
            dialog.Team = (string)cells[4];
            dialog.Nationality = (string)cells[5];
            dialog.Price = (decimal)cells[6];
            dialog.Phone = (string)cells[7];
            dialog.Position = (string)cells[8];
            dialog.Dateofbirth = (string)cells[9];

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                await new SqlCommand(
                    $@"UPDATE market SET name = N'{dialog.MName}', surname = N'{dialog.Surname}', patronymic = N'{dialog.Patronymic}',  nationality = N'{dialog.Nationality}', phone = N'{dialog.Phone}', position = N'{dialog.Position}', price = '{dialog.Price}', dateofbirth= '{dialog.Dateofbirth}', team = '{dialog.Team}' WHERE ID_Market = '{cells[0]}'",
                    Globals.connection).ExecuteNonQueryAsync();
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
                await new SqlCommand($@"DELETE FROM market WHERE ID_Market = '{dt.Rows[dataGrid.SelectedIndex].ItemArray[0]}'", Globals.connection).ExecuteNonQueryAsync();
                FillGrid();
                
            }
        }
    }
}
