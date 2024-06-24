using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WpfTestTask.Database;
using WpfTestTask.Models;

namespace WpfTestTask.Controllers
{
    static class GenreOfBookController
    {
        #region Выборка данных
        public static string SelectGenresOfBookOnRow(Guid bookId)
        {
            string command = $"SELECT genre.\"Genre\" FROM public.\"Genres\" genre JOIN public.\"GenresOfBook\" gb ON gb.\"GenreId\" = genre.\"Id\" JOIN public.\"Books\" book ON gb.\"BookId\" = book.\"Id\" WHERE gb.\"BookId\" = '{bookId}'";
            DataTable dataTable = PSqlConnection.SelectData(command);
            string genresOnRow = string.Empty;
            foreach (DataRow row in dataTable.Rows)
            {
                string genre = row["Genre"].ToString();
                genresOnRow += (genre + ", ");
            }
            if (genresOnRow.Length > 0) genresOnRow = genresOnRow.Remove(genresOnRow.Length - 2);
            return genresOnRow;
        }

        public static List<Genre> SelectGenresOfBookData(Guid bookId)
        {
            string command = $"SELECT genre.\"Id\", genre.\"Genre\" FROM public.\"Genres\" genre JOIN public.\"GenresOfBook\" gb ON gb.\"GenreId\" = genre.\"Id\" JOIN public.\"Books\" book ON gb.\"BookId\" = book.\"Id\" WHERE gb.\"BookId\" = '{bookId}'";
            DataTable dataTable = PSqlConnection.SelectData(command);
            List<Genre> genres = new List<Genre>();
            foreach (DataRow row in dataTable.Rows)
            {
                Guid id = Guid.Empty;
                if (!Guid.TryParse(row["id"].ToString(), out id)) continue;
                string genre = row["Genre"].ToString();
                genres.Add(new Genre(id, genre));
            }
            return genres;
        }

        public static List<Genre> SelectGenresOfBookFromListBox(ItemCollection items)
        {
            return (from Genre genre in items select genre).ToList();
        }
        
        public static string ConvertGenresToGenresOnRow(List<Genre> genres)
        {
            string genresOnRow = string.Empty;
            foreach (Genre genre in genres)
                genresOnRow += (genre.Name + ", ");
            if (genresOnRow.Length > 0) genresOnRow = genresOnRow.Remove(genresOnRow.Length - 2);
            return genresOnRow;
        }
        #endregion

        #region Добавление данных
        public static void InsertGenresOfBook(Book book)
        {
            foreach (Genre genre in book.Genres)
            {
                string command = "INSERT INTO public.\"GenresOfBook\"(\"Id\", \"LastModified\", \"BookId\", \"GenreId\") VALUES (" +
                    $"'{Guid.NewGuid()}', " +
                    $"'{book.LastModified}', " +
                    $"'{book.Id}', " +
                    $"'{genre.Id}');";
                PSqlConnection.InsertData(command);
            }
        }
        #endregion
    }
}
