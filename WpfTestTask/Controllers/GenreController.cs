using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfTestTask.Additional;
using WpfTestTask.Database;

namespace WpfTestTask.Controllers
{
    static class GenreController
    {
        #region Выборка данных
        //Если не получится применить, то удалить
        public static List<(Guid, string)> SelectGenres()
        {
            string command = $"SELECT genres.\"Id\", genres.\"Genre\" FROM public.\"Genres\" genres WHERE genres.\"Id\" != '{Guid.Empty}' ORDER BY genres.\"Id\"";
            DataTable dataTable = PSqlConnection.SelectData(command);
            List<(Guid, string)> genres = new List<(Guid, string)>();
            foreach (DataRow row in dataTable.Rows)
                genres.Add((Guid.Parse(row["Id"].ToString()), row["Genre"].ToString()));
            return genres;
        }

        public static List<ListOfGuidAndString> SelectGenresToListOfGuidAndString()
        {
            string command = $"SELECT genres.\"Id\", genres.\"Genre\" FROM public.\"Genres\" genres WHERE genres.\"Id\" != '{Guid.Empty}' ORDER BY genres.\"Id\"";
            DataTable dataTable = PSqlConnection.SelectData(command);
            List<ListOfGuidAndString> genres = new List<ListOfGuidAndString>();
            foreach (DataRow row in dataTable.Rows)
                genres.Add(new ListOfGuidAndString() { Id = Guid.Parse(row["Id"].ToString()), Name = row["Genre"].ToString() });
            return genres;
        }

        #endregion

        #region Добавление данных

        #endregion
    }
}
