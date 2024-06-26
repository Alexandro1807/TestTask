﻿using System.Data;
using System.Windows.Controls;
using WpfTestTask.Database;
using WpfTestTask.Models;

namespace WpfTestTask.Controllers
{
    static class GenreController
    {
        #region Выборка данных
        public static List<Genre> SelectDataGenres(bool isWithUndefined)
        {
            string withUndefined = isWithUndefined ? $"WHERE genres.\"Id\" != '{Guid.Empty}'" : "";
            string command = $"SELECT genres.\"Id\", genres.\"Name\" FROM public.\"Genres\" genres {withUndefined} ORDER BY genres.\"Id\"";
            DataTable dataTable = PSqlConnection.SelectData(command);
            List<Genre> genres = new List<Genre>();
            foreach (DataRow row in dataTable.Rows)
                genres.Add(new Genre(Guid.Parse(row["Id"].ToString()), row["Name"].ToString()));
            return genres;
        }

        public static List<Genre> SelectDataGenresFromGenresOfBook(Guid bookId)
        {
            List<Genre> genres = new List<Genre>();
            List<GenreOfBook> genresOfBook = GenreOfBookController.SelectDataGenresOfBook(bookId);
            if (genresOfBook.Count != 0)
            {
                string command = $"SELECT genre.\"Id\", genre.\"Name\" FROM public.\"Genres\" genre WHERE genre.\"Id\" IN (";
                foreach (GenreOfBook genreOfBook in genresOfBook)
                    command += $"'{genreOfBook.GenreId}', ";
                command = command.Remove(command.LastIndexOf(", ")) + ");";
                DataTable dataTable = PSqlConnection.SelectData(command);
                foreach (DataRow row in dataTable.Rows)
                {
                    if (!Guid.TryParse(row["Id"].ToString(), out Guid id)) continue;
                    string name = row["Name"].ToString();
                    genres.Add(new Genre(id, name));
                }
            }
            return genres;
        }

        public static Genre SelectDataGenre(Guid genreId)
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

        #region Добавление данных

        #endregion

        #region Модификация данных
        public static string ConvertGenresToGenresOnRow(List<Genre> genres)
        {
            string genresOnRow = string.Empty;
            foreach (Genre genre in genres)
                genresOnRow += (genre.Name + ", ");
            if (genresOnRow.Length > 0) genresOnRow = genresOnRow.Remove(genresOnRow.Length - 2);
            return genresOnRow;
        }
        #endregion

        #region Удаление данных

        #endregion
    }
}
