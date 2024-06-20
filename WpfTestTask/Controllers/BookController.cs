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
        public static BindingList<Book> GetDataBooks() //Получение данных таблиц Books, GenresOfbook, Covers
        {
            string command1 = "SELECT b.\"Id\", b.\"LastModified\", b.\"Name\", b.\"FirstName\", b.\"LastName\", b.\"MiddleName\", b.\"YearOfProduction\", b.\"ISBN\", b.\"Shortcut\", (SELECT cover.\"CoverText\" FROM public.\"Covers\" cover WHERE cover.\"BookId\" = b.\"Id\" ORDER BY cover.\"LastModified\" DESC LIMIT 1), (SELECT cover.\"Cover\" FROM public.\"Covers\" cover WHERE cover.\"BookId\" = b.\"Id\" ORDER BY cover.\"LastModified\" DESC LIMIT 1) FROM public.\"Books\" b";
            DataTable dataTable = PSqlConnection.GetData(command1);
            BindingList<Book> books = new BindingList<Book>();
            foreach (DataRow row in dataTable.Rows)
            {
                Guid id = Guid.Empty;
                if (!Guid.TryParse(row["Id"].ToString(), out id)) continue;
                DateTime lastModified = DateTime.Now;
                if (!DateTime.TryParse(row["LastModified"].ToString(), out lastModified)) continue;
                int yearOfProduction = 0;
                if (!int.TryParse(row["YearOfProduction"].ToString(), out yearOfProduction)) continue;

                string command2 = $"SELECT genre.\"Genre\" FROM public.\"Genres\" genre JOIN public.\"GenresOfBook\" gb ON gb.\"GenreId\" = genre.\"Id\" JOIN public.\"Books\" book ON gb.\"BookId\" = book.\"Id\" WHERE gb.\"BookId\" = '{id}'";
                DataTable dataTable2 = PSqlConnection.GetData(command2);
                string name, firstName, lastName, middleName, isbn, shortcut, genres, coverText;
                //byte[] cover;
                genres = string.Empty;
                foreach (DataRow row2 in dataTable2.Rows)
                {
                    string genre = row2["Genre"].ToString();
                    genres += (genre + ", ");
                }
                if (genres.Length > 0) genres = genres.Remove(genres.Length - 2);

                name = row["Name"].ToString();
                firstName = row["FirstName"].ToString();
                lastName = row["LastName"].ToString();
                middleName = row["MiddleName"].ToString();
                isbn = row["ISBN"].ToString();
                shortcut = row["Shortcut"].ToString();
                coverText = row["CoverText"].ToString();
                var temp = row["Cover"];
                var cover = temp is DBNull ? Array.Empty<byte>() : (byte[])row["Cover"];
                books.Add(new Book(id, lastModified, name, firstName, lastName, middleName, yearOfProduction, isbn, shortcut, genres, coverText, cover));
            }
            return books;
        }

        public static void SaveDataBooks(Book book) //Сохранение данных в таблицы Books, GenresOfBook, Covers
        {
            string command1 = "INSERT INTO public.\"Books\"(\"Id\", \"LastModified\", \"Name\", \"FirstName\", \"LastName\", \"MiddleName\", \"YearOfProduction\", \"ISBN\", \"Shortcut\")\tVALUES (" +
                $"'{book.Id}', " +
                $"'{book.LastModified}', " +
                $"'{book.Name}', " +
                $"'{book.FirstName}', " +
                $"'{book.LastName}', " +
                $"'{book.MiddleName}', " +
                $"'{book.YearOfProduction}', " +
                $"'{book.ISBN}', " +
                $"'{book.Shortcut}');";
            //PSqlConnection.SaveData(command1); //После вызова исключения отдельным потоком РАСКОММЕНТИРОВАТЬ

            foreach (string genre in book.Genres.Replace(" ", "").Split(','))
            {
                string command2 = "INSERT INTO public.\"GenresOfBook\"(\"Id\", \"LastModified\", \"BookId\", \"GenreId\") VALUES (" +
                    $"'{Guid.NewGuid()}', " +
                    $"'{book.LastModified}', " +
                    $"'{book.Id}', " +
                    $"(SELECT genre.\"Id\" FROM public.\"Genres\" genre WHERE genre.\"Genre\" = {genre}));";
                PSqlConnection.SaveData(command2);
            }

            string command3 = "INSERT INTO public.\"Covers\"(\"Id\", \"LastModified\", \"BookId\", \"Cover\", \"CoverText\") VALUES (" +
                $"'{Guid.NewGuid()}', " +
                $"'{book.LastModified}', " +
                $"'{book.Id}', " +
                $"'{book.Cover}', " +
                $"'{book.CoverText}');";
            PSqlConnection.SaveData(command3);
        }
    }
}
