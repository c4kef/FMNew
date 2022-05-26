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
    public partial class Operations
    {
        private CollectionViewSource _cvs;
        DataTable dt;
        SqlDataAdapter adapter;

        public List<string> OList { get; set; }

        private string oval;
        public string OVal
        {
            get
            {
                return oval;
            }
            set
            {
                oval = value;

                foreach (DataRowView dr in dataGrid.ItemsSource)
                {
                    (dataGrid.ItemContainerGenerator.ContainerFromItem(dr) as DataGridRow).Visibility = Visibility.Visible;

                    if (value == "Без фильтра")
                        continue;
                    else if (!dr[1].ToString().ToLower().Contains(value.ToLower()))
                        (dataGrid.ItemContainerGenerator.ContainerFromItem(dr) as DataGridRow).Visibility = Visibility.Collapsed;
                }
            }
        }

        public Operations()
        {
            Loaded += OnLoaded;
            OList = new List<string>();
            OList.Add("Пополнение");
            OList.Add("Расход");
            OList.Add("Без фильтра");
            InitializeComponent();
            this.DataContext = this;
            _cvs = new CollectionViewSource();
        }

        public void FillGrid()
        {
            SqlCommand cmdSel = new SqlCommand("SELECT * FROM operations ORDER BY ID_Operation DESC", Globals.connection);
            adapter = new SqlDataAdapter(cmdSel);
            dt = new DataTable();
            adapter.Fill(dt);
            dataGrid.ItemsSource = dt.DefaultView;
            dataGrid.DataContext = this;
        }

        private void Export(object sender, RoutedEventArgs e)
        {
            var wb = new XLWorkbook();
            dt.Columns.Remove("ID_Operation");
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
                MessageBox.Show("Отчёт готов");
            }
        }
        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;
            FillGrid();
        }
        
        private void Calendar_OnSelectedDatesChanged(object sender, CalendarModeChangedEventArgs calendarModeChangedEventArgs)
        {
            if (dataGrid.ItemsSource is null)
                return;

            foreach (DataRowView dr in dataGrid.ItemsSource)
            {
                (dataGrid.ItemContainerGenerator.ContainerFromItem(dr) as DataGridRow).Visibility = Visibility.Visible;

                if (dteSelectedMonth.DisplayDate.Month != DateTime.Parse(dr[4].ToString()).Month)
                    (dataGrid.ItemContainerGenerator.ContainerFromItem(dr) as DataGridRow).Visibility = Visibility.Collapsed;
            }
            
            dteSelectedMonth.DisplayMode = CalendarMode.Year;
        }
    }
}
