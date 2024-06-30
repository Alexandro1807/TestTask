using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfTestTask.Models;
using WpfTestTask.Database;
using System.Xml.Linq;
using System.Net;
using System.Windows.Controls;

namespace WpfTestTask.Controllers
{
    static class GenreController
    {
        #region Выборка данных
        public static List<Genre> SelectGenresData(bool isWithUndefined)
        {
            string withUndefined = isWithUndefined ? $"WHERE genres.\"Id\" != '{Guid.Empty}'" : "";
            string command = $"SELECT genres.\"Id\", genres.\"Name\" FROM public.\"Genres\" genres {withUndefined} ORDER BY genres.\"Id\"";
            DataTable dataTable = PSqlConnection.SelectData(command);
            List<Genre> genres = new List<Genre>();
            foreach (DataRow row in dataTable.Rows)
                genres.Add(new Genre(Guid.Parse(row["Id"].ToString()), row["Name"].ToString()));
            return genres;
        }

        public static List<Genre> SelectGenresFromGenresOfBookData(Guid bookId)
        {
            string command = $"SELECT genre.\"Id\", genre.\"Name\" FROM public.\"Genres\" genre WHERE genre.\"Id\" IN (";
            List<GenreOfBook> genresOfBook = GenreOfBookController.SelectGenresOfBookData(bookId);
            foreach (GenreOfBook genreOfBook in genresOfBook)
                command += $"'{genreOfBook.GenreId}', ";
            command = command.Remove(command.LastIndexOf(", ")) + ");";
            DataTable dataTable = PSqlConnection.SelectData(command);
            List<Genre> genres = new List<Genre>();
            foreach (DataRow row in dataTable.Rows)
            {
                if (!Guid.TryParse(row["Id"].ToString(), out Guid id)) continue;
                string name = row["Name"].ToString();
                genres.Add(new Genre(id, name));
            }
            return genres;
        }

        public static Genre SelectGenreData(Guid genreId)
        {
            string command = $"SELECT genre.\"Id\", genre.\"Name\" FROM public.\"Genres\" genre WHERE genre.\"Id\" = '{genreId}' LIMIT 1";
            DataTable dataTable = PSqlConnection.SelectData(command);
            Genre genre = null;
            foreach (DataRow row in dataTable.Rows)
            {
                if (!Guid.TryParse(row["Id"].ToString(), out Guid id)) continue;
                string name = row["Name"].ToString();
                genre = new Genre(id, name);
            }
            return genre;
        }

        public static List<Genre> SelectGenresFromListBox(ItemCollection items)
        {
            return (from Genre genre in items select genre).ToList();
        }
        #endregion
        public static string ConvertGenresToGenresOnRow(List<Genre> genres)
        {
            string genresOnRow = string.Empty;
            foreach (Genre genre in genres)
                genresOnRow += (genre.Name + ", ");
            if (genresOnRow.Length > 0) genresOnRow = genresOnRow.Remove(genresOnRow.Length - 2);
            return genresOnRow;
        }

        #region Добавление данных

        #endregion
    }
}
