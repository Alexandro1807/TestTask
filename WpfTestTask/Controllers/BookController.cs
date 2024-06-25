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
        //Удалить, если не используется
        //public static BindingList<Book> SelectBooksData() //Получение данных таблиц Books, GenresOfbook, Covers
        //{
        //    string command = "SELECT b.\"Id\", b.\"LastModified\", b.\"Name\", b.\"FirstName\", b.\"LastName\", b.\"MiddleName\", b.\"YearOfProduction\", b.\"ISBN\", b.\"Shortcut\" FROM public.\"Books\" b";
        //    DataTable dataTable = PSqlConnection.SelectData(command);
        //    BindingList<Book> books = new BindingList<Book>();
        //    foreach (DataRow row in dataTable.Rows)
        //    {
        //        Guid id = Guid.Empty;
        //        if (!Guid.TryParse(row["Id"].ToString(), out id)) continue;
        //        DateTime lastModified = DateTime.Now;
        //        if (!DateTime.TryParse(row["LastModified"].ToString(), out lastModified)) continue;
        //        int yearOfProduction = 0;
        //        if (!int.TryParse(row["YearOfProduction"].ToString(), out yearOfProduction)) continue;
        //        string name, firstName, lastName, middleName, isbn, shortcut;
        //        name = row["Name"].ToString();
        //        firstName = row["FirstName"].ToString();
        //        lastName = row["LastName"].ToString();
        //        middleName = row["MiddleName"].ToString();
        //        isbn = row["ISBN"].ToString();
        //        shortcut = row["Shortcut"].ToString();

        //        List<Genre> genres = GenreOfBookController.SelectGenresOfBookData(id);
        //        string genresOnRow = GenreOfBookController.ConvertGenresToGenresOnRow(genres);

        //        string coverText = CoverController.SelectCoverTextData(id);
        //        byte[] cover = CoverController.SelectCoverData(id);

        //        books.Add(new Book(id, lastModified, name, lastName, firstName, middleName, yearOfProduction, isbn, shortcut, genres, genresOnRow, coverText, cover));
        //    }
        //    return books;
        //}

        public static BindingList<Book> SelectDataBooksWithFunction(int limit, int offset, out int rowCount) //Получение данных таблиц Books, GenresOfbook, Covers через функцию
        {
            string command = $"SELECT * FROM BookPageFilter({limit}, {offset});";
            DataTable dataTable = PSqlConnection.SelectData(command);
            BindingList<Book> books = new BindingList<Book>();
            foreach (DataRow row in dataTable.Rows)
            {
                Guid id = Guid.Empty;
                if (!Guid.TryParse(row["Id"].ToString(), out id)) continue;
                DateTime lastModified = DateTime.Now;
                if (!DateTime.TryParse(row["LastModified"].ToString(), out lastModified)) continue;
                int yearOfProduction = 0;
                if (!int.TryParse(row["YearOfProduction"].ToString(), out yearOfProduction)) continue;
                string name, firstName, lastName, middleName, isbn, shortcut;
                name = row["Name"].ToString();
                firstName = row["FirstName"].ToString();
                lastName = row["LastName"].ToString();
                middleName = row["MiddleName"].ToString();
                isbn = row["ISBN"].ToString();
                shortcut = row["Shortcut"].ToString();

                List<Genre> genres = GenreOfBookController.SelectGenresOfBookData(id);
                string genresOnRow = GenreOfBookController.ConvertGenresToGenresOnRow(genres);

                string coverText = CoverController.SelectCoverTextData(id);
                byte[] cover = CoverController.SelectCoverData(id);

                books.Add(new Book(id, lastModified, name, lastName, firstName, middleName, yearOfProduction, isbn, shortcut, genres, genresOnRow, coverText, cover));
            }
            rowCount = books.Count;
            return books;
        }

        public static int SelectBooksCount() //Получение всех строк таблицы Books
        {
            string command = $"SELECT COUNT(*) FROM public.\"Books\"";
            DataTable dataTable = PSqlConnection.SelectData(command);
            int booksCount = 0;
            foreach (DataRow row in dataTable.Rows)
                booksCount = int.Parse(row["count"].ToString());
            return booksCount;
        }
        #endregion

        #region Добавление данных
        public static void InsertDataBooks(Book book) //Сохранение данных в таблицу Books
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
            PSqlConnection.InsertData(command);       
        }
        #endregion
    }
}
