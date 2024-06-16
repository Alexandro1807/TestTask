using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Npgsql;
using System.Data;
using System.Data.Common;

namespace WpfTestTask
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string sqlConnectionString = "Server=localhost;Port=5432;Database=test;User Id=postgres;Password=pass1234TestTask";
        NpgsqlConnection sqlConnection;
        NpgsqlCommand sqlCommand;
        public MainWindow()
        {
            InitializeComponent();
            SqlConnectionReader();
        }

        private void PSqlConnectionOpen()
        {
            sqlConnection = new NpgsqlConnection(sqlConnectionString);
            if (sqlConnection.State == ConnectionState.Closed) sqlConnection.Open();
        }

        private void PSqlConnectionClosed()
        {
            sqlConnection.Close();
        }

        public DataTable GetData(string command)
        {
            DataTable dataTable = new DataTable();
            PSqlConnectionOpen();
            PSqlCommand(command);
            NpgsqlDataReader dataReader = sqlCommand.ExecuteReader();
            dataTable.Load(dataReader);
            return dataTable;
        }

        private void PSqlCommand(string command)
        {
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = command;
        }

        private void SqlConnectionReader()
        {
            //using NpgsqlDataSource dataSource = NpgsqlDataSource.Create(sqlConnectionString);
            //string commandQuery = "SELECT * FROM \"Books\"";
            //using NpgsqlCommand command = dataSource.CreateCommand(commandQuery);
            //using NpgsqlDataReader reader = command.ExecuteReader();
            //while (reader.Read())
            //{
            //    if (reader.HasRows)
            //    {
            //        this.DataGridBooks.ItemsSource = reader;
            //    }
            //}

            /*
            await using NpgsqlDataSource dataSource = NpgsqlDataSource.Create(sqlConnectionString);
            string commandQuery = "SELECT * FROM \"Books\"";
            await using NpgsqlCommand command = dataSource.CreateCommand(commandQuery);
            await using NpgsqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                if (reader.HasRows)
                {
                    this.DataGridBooks.ItemsSource = reader;
                }
            }
            */
            //using (NpgsqlConnection sqlConnection = new NpgsqlConnection(sqlConnectionString))
            //{
            //    using (NpgsqlCommand command = new NpgsqlCommand() { Connection = sqlConnection, CommandType = CommandType.Text })
            //    {
            //        command.CommandText = "SELECT * FROM \"Books\"";
            //        sqlConnection.Open();
            //        NpgsqlDataReader dataReader = command.ExecuteReader();
            //        while (dataReader.Read())
            //        {
            //            if (dataReader.HasRows)
            //            {
            //                DataGridBooks.ItemsSource = dataReader;
            //            }
            //        }
            //    }
            //}
        }

        private void ButtonGetBooks_Click(object sender, RoutedEventArgs e)
        {
            DataTable dataBooks = GetData("SELECT * FROM \"Books\"");
            //Перенести код на вторую форму
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Открыть новое окно по кнопке
        }
    }
}