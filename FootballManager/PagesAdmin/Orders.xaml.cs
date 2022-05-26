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
    public partial class Orders
    {
        private DataTable dt_oe, dt_se;
        private SqlDataAdapter adapter_oe, adapter_se;

        public Orders()
        {
            Loaded += OnLoaded;

            InitializeComponent();
        }

        public void FillGrid()
        {
            dt_oe = new DataTable();
            dt_se = new DataTable();

            //orderequipment
            adapter_oe = new SqlDataAdapter(new SqlCommand("SELECT orders.ID_Order, orders.date_created, orders.date_ended, orders.status, employees.surname, sports_equipment.name, orders.count FROM employees INNER JOIN orders ON employees.ID_Employee = orders.ID_Employee INNER JOIN sports_equipment ON orders.ID_sports_equipment = sports_equipment.ID_sports_equipment", Globals.connection));
            adapter_oe.Fill(dt_oe);

            adapter_se = new SqlDataAdapter(new SqlCommand("SELECT * FROM sports_equipment ORDER BY ID_sports_equipment ASC", Globals.connection));
            adapter_se.Fill(dt_se);

            dataGridOrderEquipment.ItemsSource = dt_oe.DefaultView;
            dataGridSportEquipment.ItemsSource = dt_se.DefaultView;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;
            FillGrid();
            dt_oe.RowChanged += (o, oo) => { SaveGridOE(); };
            dt_oe.RowDeleted += (o, oo) => { SaveGridOE(); };

            dt_se.RowChanged += (o, oo) => { SaveGridSE(); };
            dt_se.RowDeleted += (o, oo) => { SaveGridSE(); };
        }
        private void ExportOE(object sender, RoutedEventArgs e)
        {
            var wb = new XLWorkbook();
            var dt_oe_t = dt_oe;
            dt_oe_t.Columns.Remove("ID_Order");
            dt_oe_t.TableName = "Data";
            
            for (int i = 1; i < dataGridOrderEquipment.Columns.Count; i++)
                dt_oe_t.Columns[i - 1].ColumnName = dataGridOrderEquipment.Columns[i].Header.ToString();
            
            wb.Worksheets.Add(dt_oe_t);
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


        private void ExportSE(object sender, RoutedEventArgs e)
        {
            var wb = new XLWorkbook();
            var dt_se_t = dt_se;
            dt_se_t.Columns.Remove("ID_sports_equipment");
            dt_se_t.TableName = "Data";
            
            for (int i = 1; i < dataGridSportEquipment.Columns.Count; i++)
                dt_se_t.Columns[i - 1].ColumnName = dataGridSportEquipment.Columns[i].Header.ToString();

            
            wb.Worksheets.Add(dt_se_t);
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
                adapter_oe.Update(dt_oe);
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
                adapter_se.Update(dt_se);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void AddSE(object sender, RoutedEventArgs e)
        {
            OrdersSEDialog dialog = new OrdersSEDialog();
            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                try
                {
                    await new SqlCommand($@"INSERT INTO sports_equipment (name, price) VALUES (N'{dialog.MName}', N'{dialog.Price}')", Globals.connection).ExecuteNonQueryAsync();
                    FillGrid();

                }
                catch
                {
                    MessageBox.Show("Вы заполнили не все поля, попробуйте еще раз");
                }
            }
        }

        private async void AddOE(object sender, RoutedEventArgs e)
        {
            OrderSOEDialog dialog = new OrderSOEDialog();

            dialog.Employees = new List<EmployeesData>();
            dialog.SporteQuipment = new List<SporteQuipmentData>();
            var dt_ed = new DataTable();
            new SqlDataAdapter(new SqlCommand("SELECT * FROM employees", Globals.connection)).Fill(dt_ed);

            var dt_sqd = new DataTable();
            new SqlDataAdapter(new SqlCommand("SELECT * FROM sports_equipment", Globals.connection)).Fill(dt_sqd);

            for (int i = 0; i < dt_ed.Rows.Count; i++)
                dialog.Employees.Add(new EmployeesData() { id = (int)dt_ed.Rows[i].ItemArray[0], surname = (string)dt_ed.Rows[i].ItemArray[2] });

            for (int i = 0; i < dt_sqd.Rows.Count; i++)
                dialog.SporteQuipment.Add(new SporteQuipmentData() { id = (int)dt_sqd.Rows[i].ItemArray[0], name = (string)dt_sqd.Rows[i].ItemArray[1], price = (decimal)dt_sqd.Rows[i].ItemArray[2] });
            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                if (Globals.Balance - (dialog.SporteQuipmentValue.price * dialog.Count) < 0)
                {
                    MessageBox.Show("Не достаточно средств!", "Ошибка!");
                    return;
                }

                await new SqlCommand(
                    $@"INSERT INTO orders (date_created, date_ended, status, count, ID_Employee, ID_sports_equipment) VALUES (N'{dialog.DateOrderCreated.Value.Date}', N'{dialog.DateOrderEnded.Value.Date}', N'{dialog.StatusValue}', '{dialog.Count}', (select ID_Employee from employees where surname = N'{dialog.EmployeesValue.surname}'),(select ID_sports_equipment from sports_equipment where name = N'{dialog.SporteQuipmentValue.name}'))",
                    Globals.connection).ExecuteNonQueryAsync();
                FillGrid();
                Globals.AddOperation(DateTime.Now, "Покупка инвентаря",
                    FootballManager.Globals.Balance - (decimal)(dialog.SporteQuipmentValue.price * dialog.Count),
                    (decimal)(dialog.SporteQuipmentValue.price * dialog.Count));
                Globals.Balance -= (decimal)(dialog.SporteQuipmentValue.price * dialog.Count);
            }
        }

        private void SearchSE(object sender, KeyEventArgs e)
        {
            foreach (DataRowView dr in dataGridSportEquipment.ItemsSource)
            {
                (dataGridSportEquipment.ItemContainerGenerator.ContainerFromItem(dr) as DataGridRow).Visibility = Visibility.Visible;

                if (SearchUISE.Text != string.Empty && !dr[1].ToString().ToLower().Contains(SearchUISE.Text.ToLower()))
                    (dataGridSportEquipment.ItemContainerGenerator.ContainerFromItem(dr) as DataGridRow).Visibility = Visibility.Collapsed;
            }
        }

        private async void EditOE(object sender, RoutedEventArgs e)
        {
            if (dataGridOrderEquipment.SelectedItem is null)
            {
                MessageBox.Show("Выберите запись для редактирования!", "Ошибка!");
                return;
            }

            OrderSOEDialog dialog = new OrderSOEDialog();
            object[] cells = dt_oe.Rows[dataGridOrderEquipment.SelectedIndex].ItemArray;

            dialog.DateOrderCreated = DateTime.Parse(cells[1].ToString());
            dialog.DateOrderEnded = DateTime.Parse(cells[2].ToString());
            dialog.StatusValue = (string)cells[3];
            dialog.Count = (int)cells[6];

            dialog.Employees = new List<EmployeesData>();
            dialog.SporteQuipment = new List<SporteQuipmentData>();
            var dt_ed = new DataTable();
            var dt_sqd = new DataTable();
            new SqlDataAdapter(new SqlCommand("SELECT * FROM employees", Globals.connection)).Fill(dt_ed);
            new SqlDataAdapter(new SqlCommand("SELECT * FROM sports_equipment", Globals.connection)).Fill(dt_sqd);

            for (int i = 0; i < dt_ed.Rows.Count; i++)
            {
                dialog.Employees.Add(new EmployeesData() { id = (int)dt_ed.Rows[i].ItemArray[0], surname = (string)dt_ed.Rows[i].ItemArray[2] });
                if ((string) dt_ed.Rows[i].ItemArray[2] == (string) cells[4])
                    dialog.EmployeesValue = new EmployeesData()
                        {id = (int) dt_ed.Rows[i].ItemArray[0], surname = (string) dt_ed.Rows[i].ItemArray[2]};
            }

            for (int i = 0; i < dt_sqd.Rows.Count; i++)
            {
                dialog.SporteQuipment.Add(new SporteQuipmentData()
                {
                    id = (int) dt_sqd.Rows[i].ItemArray[0], name = (string) dt_sqd.Rows[i].ItemArray[1],
                    price = (decimal) dt_sqd.Rows[i].ItemArray[2]
                });
                if ((string) dt_sqd.Rows[i].ItemArray[1] == (string) cells[5])
                    dialog.SporteQuipmentValue = new SporteQuipmentData()
                    {
                        id = (int) dt_sqd.Rows[i].ItemArray[0], name = (string) dt_sqd.Rows[i].ItemArray[1],
                        price = (decimal) dt_sqd.Rows[i].ItemArray[2]
                    };
            }

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                { 
                    //To-Do
                    await new SqlCommand(
                        $@"UPDATE orders SET date_created = N'{dialog.DateOrderCreated.Value.Date}', date_ended = N'{dialog.DateOrderEnded.Value.Date}', status = N'{dialog.StatusValue}', count = '{dialog.Count}', ID_Employee = (select ID_Employee from employees where surname = N'{dialog.EmployeesValue.surname}'), ID_sports_equipment = (select ID_sports_equipment from sports_equipment where name = N'{dialog.SporteQuipmentValue.name}') WHERE ID_Order = '{cells[0]}'",
                        Globals.connection).ExecuteNonQueryAsync();
                    FillGrid();

                }
            }
        }

        private async void RemoveOE(object sender, RoutedEventArgs e)
        {
            if (dataGridOrderEquipment.SelectedItem is null)
            {
                MessageBox.Show("Выберите запись для удаления!", "Ошибка!");
                return;
            }

            MessageBoxResult res = MessageBox.Show("Вы уверены, что хотите удалить данную запись?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res != MessageBoxResult.No)
            {
                await new SqlCommand($@"DELETE FROM orders WHERE ID_Order = '{dt_oe.Rows[dataGridOrderEquipment.SelectedIndex].ItemArray[0]}'", Globals.connection).ExecuteNonQueryAsync();
                FillGrid();

            }
        }

        private async void EditSE(object sender, RoutedEventArgs e)
        {
            if (dataGridSportEquipment.SelectedItem is null)
            {
                MessageBox.Show("Выберите запись для редактирования!", "Ошибка!");
                return;
            }

            OrdersSEDialog dialog = new OrdersSEDialog();
            object[] cells = dt_se.Rows[dataGridSportEquipment.SelectedIndex].ItemArray;

            dialog.MName = (string)cells[1];
            dialog.Price = (decimal)cells[2];

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                try
                {
                    await new SqlCommand($@"UPDATE sports_equipment SET name = N'{dialog.MName}', price = N'{dialog.Price}' WHERE ID_sports_equipment = '{cells[0]}'", Globals.connection).ExecuteNonQueryAsync();
                    FillGrid();

                }
                catch
                {
                    MessageBox.Show("Вы заполнили не все поля, попробуйте еще раз");
                }
            }
        }

        private async void RemoveSE(object sender, RoutedEventArgs e)
        {
            if (dataGridSportEquipment.SelectedItem is null)
            {
                MessageBox.Show("Выберите запись для удаления!", "Ошибка!");
                return;
            }

            MessageBoxResult res = MessageBox.Show("Вы уверены, что хотите удалить данную запись?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res != MessageBoxResult.No)
            {
                await new SqlCommand($@"DELETE FROM sports_equipment WHERE ID_sports_equipment = '{dt_se.Rows[dataGridSportEquipment.SelectedIndex].ItemArray[0]}'", Globals.connection).ExecuteNonQueryAsync();
                FillGrid();

            }
        }

        private void SearchOE(object sender, KeyEventArgs e)
        {
            foreach (DataRowView dr in dataGridOrderEquipment.ItemsSource)
            {
                (dataGridOrderEquipment.ItemContainerGenerator.ContainerFromItem(dr) as DataGridRow).Visibility = Visibility.Visible;

                if (SearchUIOE.Text != string.Empty && !dr[4].ToString().ToLower().Contains(SearchUIOE.Text.ToLower()) && !dr[5].ToString().ToLower().Contains(SearchUIOE.Text.ToLower()))
                    (dataGridOrderEquipment.ItemContainerGenerator.ContainerFromItem(dr) as DataGridRow).Visibility = Visibility.Collapsed;
            }
        }
        
        private void Calendar_OnSelectedDatesChanged(object sender, CalendarModeChangedEventArgs calendarModeChangedEventArgs)
        {
            if (dataGridOrderEquipment.ItemsSource is null)
                return;

            foreach (DataRowView dr in dataGridOrderEquipment.ItemsSource)
            {
                (dataGridOrderEquipment.ItemContainerGenerator.ContainerFromItem(dr) as DataGridRow).Visibility = Visibility.Visible;

                if (dteSelectedMonth.DisplayDate.Month != DateTime.Parse(dr[1].ToString()).Month)
                    (dataGridOrderEquipment.ItemContainerGenerator.ContainerFromItem(dr) as DataGridRow).Visibility = Visibility.Collapsed;
            }
            
            dteSelectedMonth.DisplayMode = CalendarMode.Year;
        }
    }
}
