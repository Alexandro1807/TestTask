using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfTestTask.Database;
using WpfTestTask.Models;

namespace WpfTestTask.Controllers
{
    static class CoverController
    {
        #region Выборка данных
        public static string SelectCoverText(Guid id)
        {
            string command = $"SELECT cover.\"CoverText\" FROM public.\"Covers\" cover WHERE cover.\"BookId\" = '{id}' ORDER BY cover.\"LastModified\" DESC LIMIT 1";
            DataTable dataTable = PSqlConnection.SelectData(command);
            string coverText = string.Empty;
            foreach (DataRow row in dataTable.Rows)
                coverText = row["CoverText"].ToString();
            return coverText;
        }

        public static byte[] SelectCover(Guid id)
        {
            string command = $"SELECT cover.\"Cover\" FROM public.\"Covers\" cover WHERE cover.\"BookId\" = '{id}' ORDER BY cover.\"LastModified\" DESC LIMIT 1";
            DataTable dataTable = PSqlConnection.SelectData(command);
            byte[] cover = null;
            foreach (DataRow row in dataTable.Rows)
                cover = row["Cover"] is DBNull ? Array.Empty<byte>() : (byte[])row["Cover"];
            return cover;
        }
        #endregion

        #region Добавление данных
        public static void InsertCovers(Book book)
        {
            string command = "INSERT INTO public.\"Covers\"(\"Id\", \"LastModified\", \"BookId\", \"Cover\", \"CoverText\") VALUES (" +
                $"'{Guid.NewGuid()}', " +
                $"'{book.LastModified}', " +
                $"'{book.Id}', " +
                $"'{book.Cover}', " +
                $"'{book.CoverText}');";
            PSqlConnection.InsertData(command);
        }
        #endregion
    }
}
