using Npgsql;
using System.Data;

namespace WpfTestTask.Database
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
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = command;
        }

        private static void PSqlCommandOnStoredProcedure(string command) //Инициализация команды для вызова хранимой процедуры
        {
            sqlCommand = new NpgsqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = command;
        }

        public static DataTable SelectData(string command) //Получение данных и возврат в виде DataTable
        {
            DataTable dataTable = new DataTable();
            try
            {
                PSqlConnectionOpen();
                PSqlCommand(command);
                NpgsqlDataReader dataReader = sqlCommand.ExecuteReader();
                dataTable.Load(dataReader);
                return dataTable;
            }
            finally
            {
                PSqlConnectionClosed();
            }
        }

        public static DataTable ExecuteStoredProcedure(string command) //Вызов хранимой процедуры
        {
            DataTable dataTable = new DataTable();
            try
            {
                PSqlConnectionOpen();
                PSqlCommand(command);
                NpgsqlDataReader dataReader = sqlCommand.ExecuteReader();
                dataTable.Load(dataReader);
                return dataTable;
            }
            finally
            {
                PSqlConnectionClosed();
            }
        }

        public static bool ExecuteData(string command) //Вызов команды (добавление, изменение, удаление данных)
        {
            try
            {
                PSqlConnectionOpen();
                PSqlCommand(command);
                if (sqlCommand.ExecuteNonQuery() == 0) return false;
                return true;
            }
            finally
            {
                PSqlConnectionClosed();
            }
        }

        public static void ExecuteData(string command, out int count) //Вызов команды (добавление, изменение, удаление данных) с возвратом количества строк
        {
            try
            {
                PSqlConnectionOpen();
                PSqlCommand(command);
                count = sqlCommand.ExecuteNonQuery();
            }
            finally
            {
                PSqlConnectionClosed();
            }
        }
    }
}
