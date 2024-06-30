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
        public static List<GenreOfBook> SelectGenresOfBookData(Guid bookId)
        {
            string command = $"SELECT gb.\"Id\", gb.\"LastModified\", gb.\"BookId\", gb.\"GenreId\" FROM public.\"GenresOfBook\" gb JOIN public.\"Books\" book ON gb.\"BookId\" = book.\"Id\" WHERE gb.\"BookId\" = '{bookId}'";
            DataTable dataTable = PSqlConnection.SelectData(command);
            List<GenreOfBook> genresOfBook = new List<GenreOfBook>();
            foreach (DataRow row in dataTable.Rows)
            {
                if (!Guid.TryParse(row["Id"].ToString(), out Guid id)) continue;
                if (!DateTime.TryParse(row["LastModified"].ToString(), out DateTime lastModified)) continue;
                if (!Guid.TryParse(row["BookId"].ToString(), out Guid bookDd)) continue;
                if (!Guid.TryParse(row["GenreId"].ToString(), out Guid genreId)) continue;
                genresOfBook.Add(new GenreOfBook(id, lastModified, bookDd, genreId));
            }
            return genresOfBook;
        }

        public static GenreOfBook SelectGenreOfBookData(Guid bookId, Guid genreId)
        {
            string command = $"SELECT gb.\"Id\", gb.\"LastModified\" FROM public.\"GenresOfBook\" gb JOIN public.\"Books\" book ON gb.\"BookId\" = book.\"Id\" WHERE gb.\"BookId\" = '{bookId}' AND gb.\"GenreId\" = '{genreId}' LIMIT 1";
            DataTable dataTable = PSqlConnection.SelectData(command);
            GenreOfBook genresOfBook = null;
            foreach (DataRow row in dataTable.Rows)
            {
                if (!Guid.TryParse(row["Id"].ToString(), out Guid id)) continue;
                if (!DateTime.TryParse(row["LastModified"].ToString(), out DateTime lastModified)) continue;
                genresOfBook = new GenreOfBook(id, lastModified, bookId, genreId);
            }
            return genresOfBook;
        }

        public static List<GenreOfBook> SelectGenresOfBookFromListBox(ItemCollection items)
        {
            return (from GenreOfBook genreOfBook in items select genreOfBook).ToList();
        }

        public static string ConvertGenresOfBookToGenresOnRow(List<GenreOfBook> genresOfBook)
        {
            string genresOnRow = string.Empty;
            foreach (GenreOfBook genreOfBook in genresOfBook)
            {
                Genre genre = GenreController.SelectGenreData(genreOfBook.GenreId);
                genresOnRow += (genre.Name + ", ");
            }
            if (genresOnRow.Length > 0) genresOnRow = genresOnRow.Remove(genresOnRow.Length - 2);
            return genresOnRow;
        }
        #endregion

        #region Добавление данных
        public static void InsertGenresOfBookData(List<GenreOfBook> genresOfBook)
        {
            string command = "INSERT INTO public.\"GenresOfBook\"(\"Id\", \"LastModified\", \"BookId\", \"GenreId\") VALUES ";
            foreach (GenreOfBook genreOfBook in genresOfBook)
            {
                command += "(" +
                    $"'{genreOfBook.Id}', " +
                    $"'{genreOfBook.LastModified}', " +
                    $"'{genreOfBook.BookId}', " +
                    $"'{genreOfBook.GenreId}'), ";
            }
            command = command.Remove(command.LastIndexOf(", ")) + ";";
            PSqlConnection.InsertData(command);
        }
        #endregion

        public static void UpdateGenresOfBookData(List<GenreOfBook> genresOfBook)
        {
            //ДЕЛАТЬ АПДЕЙТ ТОЛЬКО У ТЕХ, КТО СУЩЕСТВУЕТ
            foreach (GenreOfBook genreOfBook in genresOfBook)
            {
                string command = "UPDATE public.\"GenresOfBook\" genreOfBook SET" +
                    $"genreOfBook.LastModified = '{genreOfBook.LastModified}', " +
                    $"genreOfBook.BookId = '{genreOfBook.BookId}', " +
                    $"genreOfBook.GenreId = '{genreOfBook.GenreId}') " +
                    $"WHERE genreOfBook.\"Id\" = '{genreOfBook.Id}'";
                PSqlConnection.InsertData(command);
            }
        }
    }
}
