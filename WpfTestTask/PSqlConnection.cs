using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WpfTestTask.Models;

namespace WpfTestTask
{
    static class PSqlConnection
    {
        static string sqlConnectionString = "Server=localhost;Port=5432;Database=test;User Id=postgres;Password=pass1234TestTask";
        static NpgsqlConnection sqlConnection;
        static NpgsqlCommand sqlCommand;

        private static void PSqlConnectionOpen() //Открытие соединения PostgreSQL
        {
            sqlConnection = new NpgsqlConnection(sqlConnectionString);
            if (sqlConnection.State == ConnectionState.Closed) sqlConnection.Open();
        }

        private static void PSqlConnectionClosed() //Закрытие соединения PostgreSQL
        {
            sqlConnection.Close();
        }

        private static void PSqlCommand(string command) //Инициализация команды
        {
            sqlCommand = new NpgsqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = command;
        }

        public static DataTable GetData(string command) //Простое получение данных и возврат в любом виде
        {
            PSqlConnectionOpen();
            PSqlCommand(command);
            NpgsqlDataReader dataReader = sqlCommand.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(dataReader);
            PSqlConnectionClosed();
            return dataTable;
        }
    }
}
