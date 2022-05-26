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
            var dtt = dt;
            dtt.Columns.Remove("ID_Market");
            dtt.Columns.Remove("ID_Player");
            dtt.TableName = "Data";
            
            for (int i = 2; i < dataGrid.Columns.Count; i++)
                dtt.Columns[i - 2].ColumnName = dataGrid.Columns[i].Header.ToString();
            
            wb.Worksheets.Add(dtt);
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
            
            if ((dataGrid.SelectedItem as DataRowView).Row.ItemArray[6].ToString() == "Игрок клуба")
            {
                MessageBox.Show("Это игрок из вашего клуба");
                return;
            }
            
            MessageBoxResult res = MessageBox.Show("Вы действительно хотите купить данного игрока?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res != MessageBoxResult.No)
            {
                var selectedPlayers = dataGrid.SelectedItems;
                decimal price = 0;

                foreach (DataRowView row in selectedPlayers)
                    price += decimal.Parse(row.Row.ItemArray[5].ToString());

                if (Globals.Balance - price >= 0)
                {
                    var name = string.Empty;
                    for (int i = 0; i < selectedPlayers.Count; i++)
                    {
                        name = ((DataRowView)selectedPlayers[i]).Row.ItemArray[2].ToString();

                        again:
                        var dialog = new MarketRegisterDialog();
                        var result = await dialog.ShowAsync();

                        if (result == ContentDialogResult.Primary)
                            await new SqlCommand($"INSERT INTO playerlist(surname, name, patronymic, dateofbirth, position,  nationality,  phone, team, login, pass) VALUES (N'{((DataRowView)selectedPlayers[i]).Row.ItemArray[2]}', N'{((DataRowView)selectedPlayers[i]).Row.ItemArray[3]}', N'{((DataRowView)selectedPlayers[i]).Row.ItemArray[4]}', N'{((DataRowView)selectedPlayers[i]).Row.ItemArray[7]}', N'{((DataRowView)selectedPlayers[i]).Row.ItemArray[9]}', N'{((DataRowView)selectedPlayers[i]).Row.ItemArray[8]}', N'{((DataRowView)selectedPlayers[i]).Row.ItemArray[10]}' , N'Игрок клуба', N'{dialog.Login}', N'{dialog.Pass}')", Globals.connection).ExecuteNonQueryAsync();
                        else 
                            goto again;

                        //To-Do
                        await new SqlCommand($"DELETE FROM market WHERE ID_Market = '{((DataRowView)selectedPlayers[i]).Row.ItemArray[0]}'", Globals.connection).ExecuteNonQueryAsync();
                        FillGrid();
                    }
                    
                    Globals.AddOperation(DateTime.Now, "Покупка игрока", Globals.Balance - price, price);
                    Globals.Balance -= price;
                    
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
                await new SqlCommand(
                    $@"INSERT INTO market (surname, name, patronymic, dateofbirth , team, nationality, price, phone, position) VALUES (N'{dialog.Surname}', N'{dialog.MName}', N'{dialog.Patronymic}', N'{dialog.Dateofbirth}',N'{dialog.Team}', N'{dialog.Nationality}', N'{dialog.Price}', N'{dialog.Phone}', N'{dialog.Position}')",
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

        private async void Edit(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem is null)
            {
                MessageBox.Show("Выберите запись для редактирования!", "Ошибка!");
                return;
            }

            MarketDialog dialog = new MarketDialog();
            object[] cells = dt.Rows[dataGrid.SelectedIndex].ItemArray;

            dialog.Surname = (string)cells[2];
            dialog.MName = (string)cells[3];
            dialog.Patronymic = (string)cells[4];
            dialog.Price = (decimal)cells[5];
            dialog.Team = (string)cells[6];
            dialog.Dateofbirth = DateTime.Parse(cells[7].ToString());
            dialog.Nationality = (string)cells[8];
            dialog.Position = (string)cells[9];
            dialog.Phone = (string)cells[10];

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                await new SqlCommand(
                    $@"UPDATE market SET name = N'{dialog.MName}', surname = N'{dialog.Surname}', patronymic = N'{dialog.Patronymic}',  nationality = N'{dialog.Nationality}', phone = N'{dialog.Phone}', position = N'{dialog.Position}', price = '{dialog.Price}', dateofbirth= '{dialog.Dateofbirth}', team = N'{dialog.Team}' WHERE ID_Market = '{cells[0]}'",
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

            try
            {
                MessageBoxResult res = MessageBox.Show("Вы уверены, что хотите удалить данную запись?", "Внимание!",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res != MessageBoxResult.No)
                {
                    await new SqlCommand(
                        $@"DELETE FROM market WHERE ID_Market = '{dt.Rows[dataGrid.SelectedIndex].ItemArray[0]}'",
                        Globals.connection).ExecuteNonQueryAsync();
                    FillGrid();
                }
            }
            catch
            {
                MessageBox.Show("Какая печаль, где-то еще есть запись о маркете");
            }
        }

        private async void SellPlayer(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem is null)
            {
                MessageBox.Show("Выберите игрока для продажи!", "Ошибка!");
                return;
            }

            var items = ((DataRowView) dataGrid.SelectedItem).Row;

            if (items[6].ToString() != "Игрок клуба")
            {
                MessageBox.Show("Это игрок не вашего клуба");
                return;
            }
            
            var dialog = new MarketSellDialog();
            var result = await dialog .ShowAsync(); //                await new SqlCommand($"INSERT INTO playerlist(surname, name, patronymic, dateofbirth, position,  nationality,  phone, team, login, pass) VALUES (N'{((DataRowView)selectedPlayers[i]).Row.ItemArray[2]}', N'{((DataRowView)selectedPlayers[i]).Row.ItemArray[3]}', N'{((DataRowView)selectedPlayers[i]).Row.ItemArray[4]}', N'{((DataRowView)selectedPlayers[i]).Row.ItemArray[7]}', N'{((DataRowView)selectedPlayers[i]).Row.ItemArray[9]}', N'{((DataRowView)selectedPlayers[i]).Row.ItemArray[8]}', N'{((DataRowView)selectedPlayers[i]).Row.ItemArray[10]}' , N'Игрок клуба', N'{dialog.Login}', N'{dialog.Pass}')", Globals.connection).ExecuteNonQueryAsync();

            if (result != ContentDialogResult.Primary) return;
            await new SqlCommand($@"DELETE FROM market WHERE ID_Market = '{items[0]}'", Globals.connection).ExecuteNonQueryAsync();
            await OperationsPlayers.AddO(items[2].ToString(), (decimal) dialog.Price);
            Globals.AddOperation(DateTime.Now, "Продажа игрока", Globals.Balance + (decimal)dialog.Price, (decimal)dialog.Price);
            Globals.Balance += (decimal)dialog.Price;
            FillGrid();
            MessageBox.Show("Игрок продан");
        }

        private async void OutOrder(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem is null)
            {
                MessageBox.Show("Выберите игрока которого хотите вернуть", "Ошибка!");
                return;
            }

            var items = ((DataRowView) dataGrid.SelectedItem).Row;

            if (items[6].ToString() != "Игрок клуба")
            {
                MessageBox.Show("Это игрок не вашего клуба");
                return;
            }
            
            var dialog = new MarketRegisterDialog();
            var result = await dialog.ShowAsync();

            if (result != ContentDialogResult.Primary)
            {
                MessageBox.Show("Укажите логин и пароль");
                return;
            }
                
            await new SqlCommand(
                $"INSERT INTO playerlist(surname, name, patronymic, dateofbirth, position,  nationality,  phone, team, login, pass) VALUES (N'{items[2]}', N'{items[3]}', N'{items[4]}', N'{items[7]}', N'{items[9]}', N'{items[8]}', N'{items[10]}' , N'Игрок клуба', N'{dialog.Login}',N'{dialog.Pass}')",
                Globals.connection).ExecuteNonQueryAsync();
            await new SqlCommand($@"DELETE FROM market WHERE ID_Market = '{items[0]}'", Globals.connection).ExecuteNonQueryAsync();
            FillGrid();
            MessageBox.Show("Игрок снят с продажи");
        }
    }
}
