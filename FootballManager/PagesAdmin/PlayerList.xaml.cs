using ClosedXML.Excel;
using Microsoft.Win32;
using ModernWpf.Controls;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace FootballManager.PagesAdmin
{
    [Serializable]
    public partial class PlayerList
    {
        private CollectionViewSource _cvs;
        DataTable dt;
        SqlDataAdapter adapter;

        public PlayerList()
        {
            Loaded += OnLoaded;

            InitializeComponent();

            _cvs = new CollectionViewSource();
            this.DataContext = this;
        }

        public void FillGrid()
        {
            SqlCommand cmdSel = new SqlCommand("SELECT * FROM playerlist ORDER BY ID_Player ASC", Globals.connection);
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

        private void Export(object sender, RoutedEventArgs e)
        {
            var wb = new XLWorkbook();

            var dtt = dt;
            dtt.Columns.Remove("ID_Player");
            dtt.TableName = "Data";
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

        private async void SellPlayer(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem is null)
            {
                MessageBox.Show("Выберите игрока для продажи!", "Ошибка!");
                return;
            }

            MessageBoxResult res = MessageBox.Show("Вы действительно хотите продать данного игрока?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res != MessageBoxResult.No)
            {
                decimal price = 0;

                if (!decimal.TryParse(Price.Text, out price))
                {
                    MessageBox.Show("Введите корректную сумму!", "Ошибка!");
                    return;
                }

                var selectedPlayer = (DataRowView)dataGrid.SelectedItem;
                {
                    /*await new SqlCommand(
                        $"INSERT INTO market(name, surname, patronymic, dateofbirth, nationality, position,phone,team) VALUES " +
                        $"(N'{selectedPlayer.Row.ItemArray[2]}',N'{selectedPlayer.Row.ItemArray[1]}',N'{selectedPlayer.Row.ItemArray[3]}',N'{selectedPlayer.Row.ItemArray[4]}',N'{selectedPlayer.Row.ItemArray[5]}','{price}',N'{selectedPlayer.Row.ItemArray[7]}',N'{selectedPlayer.Row.ItemArray[6]}',N'{selectedPlayer.Row.ItemArray[8]}')",
                        Globals.connection).ExecuteNonQueryAsync();*/
                    
                    await new SqlCommand(
                        $"INSERT INTO market(ID_Player, surname, name, patronymic, price, team, dateofbirth, nationality, position, phone) VALUES " +
                        $"((select ID_Player from PlayerList where surname = N'{selectedPlayer.Row.ItemArray[1]}'), N'{selectedPlayer.Row.ItemArray[1]}', N'{selectedPlayer.Row.ItemArray[2]}', N'{selectedPlayer.Row.ItemArray[3]}', '{price}', N'{selectedPlayer.Row.ItemArray[8]}', N'{selectedPlayer.Row.ItemArray[4]}', N'{selectedPlayer.Row.ItemArray[5]}', N'{selectedPlayer.Row.ItemArray[6]}',N'{selectedPlayer.Row.ItemArray[7]}')",
                        Globals.connection).ExecuteNonQueryAsync();

                    //To-Do
                    await OperationsPlayers.AddO(selectedPlayer.Row.ItemArray[1].ToString(), price);

                    /*await new SqlCommand(
                        $"DELETE FROM playerlist WHERE ID_Player = '{selectedPlayer.Row.ItemArray[0]}'",
                        Globals.connection).ExecuteNonQueryAsync();//Как-то удалять надо игрока

                    Globals.AddOperation(DateTime.Now, "Продажа игрока", Globals.Balance + price, price);
                    Globals.Balance += price;
                    FillGrid();*/

                    MessageBox.Show("Игрок успешно выставлен на продажу!");
                }
            }
        }

        private async void Add(object sender, RoutedEventArgs e)
        {
            PlayerListDialog dialog = new PlayerListDialog();
            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                try
                {
                    var r = dialog;
                  await new SqlCommand($@"INSERT INTO playerlist (surname, name, patronymic, dateofbirth, nationality, position, phone, team) VALUES (N'{dialog.Surname}', N'{dialog.MName}', N'{dialog.Patronymic}', N'{dialog.Dateofbirth}', N'{dialog.Nationality}', N'{dialog.Position}', N'{dialog.Phone}',N'{dialog.Team}')", Globals.connection).ExecuteNonQueryAsync();
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
                if (SearchUI.Text == string.Empty)
                    (dataGrid.ItemContainerGenerator.ContainerFromItem(dr) as DataGridRow).Visibility = Visibility.Visible;
                else if (!dr[1].ToString().ToLower().Contains(SearchUI.Text.ToLower()) && !dr[2].ToString().ToLower().Contains(SearchUI.Text.ToLower()) && !dr[3].ToString().ToLower().Contains(SearchUI.Text.ToLower()))
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

            PlayerListDialog dialog = new PlayerListDialog();
            object[] cells = dt.Rows[dataGrid.SelectedIndex].ItemArray;

            dialog.Surname = (string)cells[1];
            dialog.MName = (string)cells[2];
            dialog.Patronymic = (string)cells[3];
            dialog.Dateofbirth = (string)cells[4];
            dialog.Nationality = (string)cells[5];
            dialog.Position = (string)cells[6];
            dialog.Phone = (string)cells[7];
            dialog.Team = (string)cells[8];
            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                await new SqlCommand($@"UPDATE playerlist SET surname = N'{dialog.Surname}', name = N'{dialog.MName}', patronymic =  N'{dialog.Patronymic}', dateofbirth = N'{dialog.Dateofbirth}', nationality = N'{dialog.Nationality}', position = N'{dialog.Position}', phone = N'{dialog.Phone}', team = N'{dialog.Team}' WHERE ID_Player = '{cells[0]}'", Globals.connection).ExecuteNonQueryAsync();
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
                        $@"DELETE FROM playerlist WHERE ID_Player = '{dt.Rows[dataGrid.SelectedIndex].ItemArray[0]}'",
                        Globals.connection).ExecuteNonQueryAsync();
                    FillGrid();
                }
            }
            catch
            {
                MessageBox.Show("Упс, об этом игроке есть записи в других местах");
            }
        }
        private void CheckDigits(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            try
            {
                e.Handled = System.Text.RegularExpressions.Regex.IsMatch(e.Text, "[^0-9]"); ;
            }
            catch { }
        }
    }
}