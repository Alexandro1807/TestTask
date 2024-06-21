using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTestTask.Controllers
{
    static class GenreController
    {
        #region Выборка данных
        public static List<(Guid, string)> SelectGenres()
        {
            string command = $"SELECT genres.\"Id\", genres.\"Genre\" FROM public.\"Genres\" genres ORDER BY genres.\"Id\"";
            DataTable dataTable = PSqlConnection.SelectData(command);
            List<(Guid, string)> genres = new List<(Guid, string)>();
            foreach (DataRow row in dataTable.Rows)
                genres.Add((Guid.Parse(row["Id"].ToString()), row["Genre"].ToString()));
            return genres;
        }
        #endregion

        #region Добавление данных

        #endregion
    }
}
