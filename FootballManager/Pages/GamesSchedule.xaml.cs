using FootballManager;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
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
    public partial class GamesSchedule
    {
        private CollectionViewSource _cvs;

        public GamesSchedule()
        {
            Loaded += OnLoaded;

            InitializeComponent();

            _cvs = new CollectionViewSource();
        }

        public void FillGrid() 
        {
            SqlCommand cmdSel = new SqlCommand("SELECT ID_game_shedule, date, team, stadium, Tournaments.name, result, revenue, ticket_count FROM Gamesschedule join Tournaments ON (Gamesschedule.ID_tournament = Tournaments.ID_tournament)", Globals.connection);
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

        private void ToggleTheme(object sender, RoutedEventArgs e)
        {
            this.ToggleTheme();
        }

        private void GroupingToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            _cvs.GroupDescriptions.Clear();
        }
    }
}
