﻿using FootballManager;
using System.Data.SqlClient;
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

namespace FootballManager.Pages
{
    public partial class TrainingSchedule
    {
        private CollectionViewSource _cvs;

        public TrainingSchedule()
        {
            Loaded += OnLoaded;

            InitializeComponent();

            _cvs = new CollectionViewSource();
        }

        public void FillGrid()
        {
            SqlCommand cmdSel = new SqlCommand("SELECT * FROM trainingschedule ORDER BY ID_training_schedule ASC", Globals.connection);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmdSel);
            da.Fill(dt);
            dataGrid.ItemsSource = dt.DefaultView;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;
            FillGrid();
        }
    }
}
