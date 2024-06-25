using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfTestTask.Models;
using WpfTestTask.Database;
using System.Xml.Linq;

namespace WpfTestTask.Controllers
{
    static class GenreController
    {
        #region Выборка данных
        public static List<Genre> SelectGenresToList()
        {
            string command = $"SELECT genres.\"Id\", genres.\"Genre\" FROM public.\"Genres\" genres WHERE genres.\"Id\" != '{Guid.Empty}' ORDER BY genres.\"Id\"";
            DataTable dataTable = PSqlConnection.SelectData(command);
            List<Genre> genres = new List<Genre>();
            foreach (DataRow row in dataTable.Rows)
                genres.Add(new Genre(Guid.Parse(row["Id"].ToString()), row["Genre"].ToString()));
            return genres;
        }

        public static List<Genre> SelectGenresWithUndefinedToList()
        {
            string command = $"SELECT genres.\"Id\", genres.\"Genre\" FROM public.\"Genres\" genres ORDER BY genres.\"Id\"";
            DataTable dataTable = PSqlConnection.SelectData(command);
            List<Genre> genres = new List<Genre>();
            foreach (DataRow row in dataTable.Rows)
                genres.Add(new Genre(Guid.Parse(row["Id"].ToString()), row["Genre"].ToString()));
            return genres;
        }
        #endregion

        #region Добавление данных

        #endregion
    }
}
