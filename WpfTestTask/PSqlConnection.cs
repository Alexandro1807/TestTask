using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfTestTask
{
    class PSqlConnection
    {
        string sqlConnectionString = "Server=localhost;Port=5432;Database=test;User Id=postgres;Password=pass1234TestTask";
        NpgsqlConnection sqlConnection;
        NpgsqlCommand sqlCommand;

        private void PSqlConnectionOpen()
        {
            sqlConnection = new NpgsqlConnection(sqlConnectionString);
            if (sqlConnection.State == ConnectionState.Closed) sqlConnection.Open();
        }

        private void PSqlConnectionClosed()
        {
            sqlConnection.Close();
        }

        private void PSqlCommand(string command)
        {
            sqlCommand = new NpgsqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = command;
        }

        public ItemsControl GetData(string command) //Простое получение данных и возврат в любом виде
        {
            PSqlConnectionOpen();
            PSqlCommand(command);
            NpgsqlDataReader dataReader = sqlCommand.ExecuteReader();
            ItemsControl itemsControl = new ItemsControl() { ItemsSource = dataReader };
            dataReader.Close();
            PSqlConnectionClosed();
            return itemsControl;
        }

        public void GetDataBooks(string command) //Поменять на return BooksDBContext ?????
        {
            //Используем GetData для получения данных, здесь заполняем класс под нужный DataGrid
            //Сделать класс для сущности Books и возврата Books с дополнительными запросами
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
    }
}
