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
        public static BindingList<Book> SelectDataBooks() //Получение данных таблиц Books, GenresOfbook, Covers
        {
            string command = "SELECT b.\"Id\", b.\"LastModified\", b.\"Name\", b.\"FirstName\", b.\"LastName\", b.\"MiddleName\", b.\"YearOfProduction\", b.\"ISBN\", b.\"Shortcut\", (SELECT cover.\"CoverText\" FROM public.\"Covers\" cover WHERE cover.\"BookId\" = b.\"Id\" ORDER BY cover.\"LastModified\" DESC LIMIT 1), (SELECT cover.\"Cover\" FROM public.\"Covers\" cover WHERE cover.\"BookId\" = b.\"Id\" ORDER BY cover.\"LastModified\" DESC LIMIT 1) FROM public.\"Books\" b";
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

                string genres = GenreOfBookController.SelectGenresOfBook(id);

                string coverText = CoverController.SelectCoverText(id);
                byte[] cover = CoverController.SelectCover(id);

                books.Add(new Book(id, lastModified, name, firstName, lastName, middleName, yearOfProduction, isbn, shortcut, genres, coverText, cover));
            }
            return books;
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
