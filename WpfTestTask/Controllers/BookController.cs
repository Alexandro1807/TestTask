using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfTestTask.Models;

namespace WpfTestTask.Controllers
{
    class BookController
    {
        public static BindingList<Book> GetDataBooks() //Получение данных таблицы Books
        {
            string command1 = "SELECT * FROM \"Books\"";
            DataTable dataTable = PSqlConnection.GetData(command1);
            BindingList<Book> books = new BindingList<Book>();
            foreach (DataRow row in dataTable.Rows)
            {
                Guid id = Guid.Empty;
                if (!Guid.TryParse(row["Id"].ToString(), out id)) continue;

                string command2 = $"SELECT genre.\"Genre\" FROM public.\"Genres\" genre JOIN public.\"GenresOfBook\" gb ON gb.\"GenreId\" = genre.\"Id\" JOIN public.\"Books\" book ON gb.\"BookId\" = book.\"Id\" WHERE gb.\"BookId\" = '{id}'";
                DataTable dataTable2 = PSqlConnection.GetData(command2);
                string genres = string.Empty;
                foreach (DataRow row2 in dataTable2.Rows)
                {
                    string genre = row2["Genre"].ToString();
                    genres += (genre + ", ");
                }
                genres = genres.Remove(genres.Length - 2);

                DateTime lastModified = DateTime.Now;
                if (!DateTime.TryParse(row["LastModified"].ToString(), out lastModified)) continue;
                int yearOfProduction = 0;
                if (!int.TryParse(row["YearOfProduction"].ToString(), out yearOfProduction)) continue;
                string name, firstName, lastName, middleName, isbn, cover, shortcut;
                name = row["Name"].ToString();
                firstName = row["FirstName"].ToString();
                lastName = row["LastName"].ToString();
                middleName = row["MiddleName"].ToString();
                isbn = row["ISBN"].ToString();
                cover = row["Cover"].ToString();
                shortcut = row["Shortcut"].ToString();
                books.Add(new Book(id, lastModified, name, firstName, lastName, middleName, yearOfProduction, isbn, cover, shortcut, genres));
            }
            return books;
        }
    }
}
