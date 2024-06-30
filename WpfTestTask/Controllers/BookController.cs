using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfTestTask.Database;
using WpfTestTask.Models;

namespace WpfTestTask.Controllers
{
    static class BookController
    {
        #region Выборка данных
        public static BindingList<Book> SelectDataBooks(string nameFilter, string authorFilter, string genreFilter, int yearOfProductionFilter, int limit, int offset, out int rowFilterCount) //Получение данных таблиц Books, GenresOfbook, Covers через функцию
        {
            //в будущем изменить функцию на BookFilter здесь и в базе данных
            string command = $"SELECT * FROM BookFilterFinish('{nameFilter}', '{authorFilter}', '{genreFilter}', {yearOfProductionFilter}, {limit}, {offset});";
            DataTable dataTable = PSqlConnection.SelectData(command);
            BindingList<Book> books = new BindingList<Book>();
            foreach (DataRow row in dataTable.Rows)
            {
                if (!Guid.TryParse(row["Id"].ToString(), out Guid id)) continue;
                if (!DateTime.TryParse(row["LastModified"].ToString(), out DateTime lastModified)) continue;
                if (!int.TryParse(row["YearOfProduction"].ToString(), out int yearOfProduction)) continue;
                string name, firstName, lastName, middleName, isbn, shortcut;
                name = row["Name"].ToString();
                firstName = row["FirstName"].ToString();
                lastName = row["LastName"].ToString();
                middleName = row["MiddleName"].ToString();
                isbn = row["ISBN"].ToString();
                shortcut = row["Shortcut"].ToString();

                List<GenreOfBook> genresOfBook = GenreOfBookController.SelectDataGenresOfBook(id);
                string genresOnRow = GenreOfBookController.ConvertGenresOfBookToGenresOnRow(genresOfBook);

                string coverText = CoverController.SelectDataCoverText(id);
                byte[] cover = CoverController.SelectDataCover(id);

                books.Add(new Book(id, lastModified, name, lastName, firstName, middleName, yearOfProduction, isbn, shortcut, genresOfBook, genresOnRow, coverText, cover));
            }
            rowFilterCount = books.Count;
            return books;
        }

        public static int SelectBooksCount(string nameFilter, string authorFilter, string genreFilter, int yearOfProductionFilter) //Получение всех строк таблицы Books
        {
            string command = $"SELECT COUNT(*) FROM BookFilterCount('{nameFilter}', '{authorFilter}', '{genreFilter}', {yearOfProductionFilter});";
            DataTable dataTable = PSqlConnection.SelectData(command);
            int booksCount = 0;
            foreach (DataRow row in dataTable.Rows)
                booksCount = int.Parse(row["count"].ToString());
            return booksCount;
        }
        #endregion

        #region Добавление данных
        public static void InsertDataBook(Book book) //Сохранение данных в таблицу Books
        {
            string command = "INSERT INTO public.\"Books\"(\"Id\", \"LastModified\", \"Name\", \"FirstName\", \"LastName\", \"MiddleName\", \"YearOfProduction\", \"ISBN\", \"Shortcut\")\tVALUES (" +
                $"'{book.Id}', " +
                $"'{book.LastModified}', " +
                $"'{book.Name}', " +
                $"'{book.FirstName}', " +
                $"'{book.LastName}', " +
                $"'{book.MiddleName}', " +
                $"'{book.YearOfProduction}', " +
                $"'{book.ISBN}', " +
                $"'{book.Shortcut}');";
            PSqlConnection.ExecuteData(command);       
        }
        #endregion
        #region Модификация данных
        #endregion
        #region Удаление данных
        #endregion
        public static void UpdateDataBooks(Book book) //Сохранение данных в таблицу Books
        {
            string command = "UPDATE public.\"Books\" b SET" +
                $"b.\"LastModified\" = '{book.LastModified}', " +
                $"b.\"Name\" = '{book.Name}', " +
                $"b.\"FirstName\" = '{book.FirstName}', " +
                $"b.\"LastName\" = '{book.LastName}', " +
                $"b.\"MiddleName\" = '{book.MiddleName}', " +
                $"b.\"YearOfProduction\" = '{book.YearOfProduction}', " +
                $"b.\"ISBN\" = '{book.ISBN}', " +
                $"b.\"Shortcut\" = '{book.Shortcut}' " +
                $"WHERE b.\"Id\" = '{book.Id}'";
            PSqlConnection.ExecuteData(command);
        }

        public static void DeleteDataBook(Guid id)
        {
            string command = $"DELETE FROM public.\"Books\" WHERE \"Id\" = '{id}';";
            PSqlConnection.ExecuteData(command);
        }
    }
}
