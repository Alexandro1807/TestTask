using System.Data;
using System.IO;
using WpfTestTask.Database;
using WpfTestTask.Models;

namespace WpfTestTask.Controllers
{
    static class CoverController
    {
        #region Выборка данных
        public static string SelectDataCoverText(Guid bookId)
        {
            string command = $"SELECT cover.\"CoverText\" FROM public.\"Covers\" cover WHERE cover.\"BookId\" = '{bookId}' ORDER BY cover.\"LastModified\" DESC LIMIT 1";
            DataTable dataTable = PSqlConnection.SelectData(command);
            string coverText = string.Empty;
            foreach (DataRow row in dataTable.Rows)
                coverText = row["CoverText"].ToString();
            return coverText;
        }

        public static byte[] SelectDataCover(Guid id)
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
        public static void InsertDataCover(Book book)
        {
            string command = "INSERT INTO public.\"Covers\"(\"Id\", \"LastModified\", \"BookId\", \"Cover\", \"CoverText\") VALUES (" +
                $"'{Guid.NewGuid()}', " +
                $"'{book.LastModified}', " +
                $"'{book.Id}', " +
                $"'{book.Cover}', " +
                $"'{book.CoverText}');";
            PSqlConnection.ExecuteData(command);
        }

        public static void InsertDataCoverWithoutImage(Book book)
        {
            Guid coverId = Guid.NewGuid();
            book.CoverText = CopyCoverFileToDatabaseFolder(book, coverId);
            string command = "INSERT INTO public.\"Covers\"(\"Id\", \"LastModified\", \"BookId\", \"Cover\", \"CoverText\") VALUES (" +
                $"'{coverId}', " +
                $"'{book.LastModified}', " +
                $"'{book.Id}', " +
                $"null, " +
                $"'{book.CoverText}');";
            PSqlConnection.ExecuteData(command);
        }
        #endregion

        #region Модификация данных
        public static string CopyCoverFileToDatabaseFolder(Book book, Guid coverId)
        {
            string serverFolderPath = "C:\\temp_pgsql";
            if (!Directory.Exists(serverFolderPath)) Directory.CreateDirectory(serverFolderPath);
            string fileExtension = book.CoverText.Remove(0, book.CoverText.LastIndexOf("."));
            string destinationFolder = serverFolderPath + $"\\{book.Id}";
            if (!Directory.Exists(destinationFolder)) Directory.CreateDirectory(destinationFolder);
            string destinationPath = destinationFolder + $"\\{coverId}{fileExtension}";
            File.Copy(book.CoverText, destinationPath, true);
            return destinationPath;
        }
        #endregion

        #region Удаление данных
        public static void DeleteDataCoverWithoutImage(Guid bookId)
        {
            string command = $"DELETE FROM public.\"Covers\" WHERE \"BookId\" = '{bookId}';";
            PSqlConnection.ExecuteData(command);
        }
        #endregion
    }
}
