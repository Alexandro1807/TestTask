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
    static class GenreOfBookController
    {
        #region Выборка данных
        public static string SelectGenresOfBook(Guid id)
        {
            string command = $"SELECT genre.\"Genre\" FROM public.\"Genres\" genre JOIN public.\"GenresOfBook\" gb ON gb.\"GenreId\" = genre.\"Id\" JOIN public.\"Books\" book ON gb.\"BookId\" = book.\"Id\" WHERE gb.\"BookId\" = '{id}'";
            DataTable dataTable = PSqlConnection.SelectData(command);
            string genres = string.Empty;
            foreach (DataRow row in dataTable.Rows)
            {
                string genre = row["Genre"].ToString();
                genres += (genre + ", ");
            }
            if (genres.Length > 0) genres = genres.Remove(genres.Length - 2);
            return genres;
        }
        #endregion

        #region Добавление данных
        public static void InsertGenresOfBook(Book book)
        {
            foreach (string genre in book.Genres.Replace(" ", "").Split(','))
            {
                string command = "INSERT INTO public.\"GenresOfBook\"(\"Id\", \"LastModified\", \"BookId\", \"GenreId\") VALUES (" +
                    $"'{Guid.NewGuid()}', " +
                    $"'{book.LastModified}', " +
                    $"'{book.Id}', " +
                    $"(SELECT genre.\"Id\" FROM public.\"Genres\" genre WHERE genre.\"Genre\" = {genre}));";
                PSqlConnection.InsertData(command);
            }
        }
        #endregion
    }
}
