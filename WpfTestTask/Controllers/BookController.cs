using System.ComponentModel;
using System.Data;
using WpfTestTask.Database;
using WpfTestTask.Models;

namespace WpfTestTask.Controllers
{
    static class BookController
    {
        #region Выборка данных
        public static BindingList<Book> SelectDataBooks(string nameFilter, string authorFilter, string genreFilter, int yearOfProductionFilter, int limit, int offset, out int rowFilterCount) //Получение данных таблиц Books, GenresOfbook, Covers через функцию
        {
            string command = $"SELECT * FROM BookFilter('{nameFilter}', '{authorFilter}', '{genreFilter}', {yearOfProductionFilter}, {limit}, {offset});";
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

        public static Book SelectDataBook(Guid id)
        {
            string command = $"SELECT * FROM public.\"Books\" WHERE \"Id\" = '{id}' LIMIT 1;";
            DataTable dataTable = PSqlConnection.SelectData(command);
            Book book = null;
            foreach (DataRow row in dataTable.Rows)
            {
                if (!Guid.TryParse(row["Id"].ToString(), out Guid bookId)) continue;
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

                book = new Book(id, lastModified, name, lastName, firstName, middleName, yearOfProduction, isbn, shortcut, genresOfBook, genresOnRow, coverText, cover);
            }
            return book;
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

        public static void InsertDataBooks(List<Book> books) //Сохранение множества данных в таблицу Books
        {
            string command = "INSERT INTO public.\"Books\"(\"Id\", \"LastModified\", \"Name\", \"FirstName\", \"LastName\", \"MiddleName\", \"YearOfProduction\", \"ISBN\", \"Shortcut\")\tVALUES (";
            foreach(Book book in books)
            {
                command += $"'{book.Id}', " +
                $"'{book.LastModified}', " +
                $"'{book.Name}', " +
                $"'{book.FirstName}', " +
                $"'{book.LastName}', " +
                $"'{book.MiddleName}', " +
                $"'{book.YearOfProduction}', " +
                $"'{book.ISBN}', " +
                $"'{book.Shortcut}'), ";
            }
            command = command.Remove(command.LastIndexOf(", ")) + ";";
            PSqlConnection.ExecuteData(command);
        }
        #endregion

        #region Модификация данных
        public static void UpdateDataBooks(Book book) //Сохранение данных в таблицу Books
        {
            string command = "UPDATE public.\"Books\" SET " +
                $"\"LastModified\" = '{book.LastModified}', " +
                $"\"Name\" = '{book.Name}', " +
                $"\"FirstName\" = '{book.FirstName}', " +
                $"\"LastName\" = '{book.LastName}', " +
                $"\"MiddleName\" = '{book.MiddleName}', " +
                $"\"YearOfProduction\" = '{book.YearOfProduction}', " +
                $"\"ISBN\" = '{book.ISBN}', " +
                $"\"Shortcut\" = '{book.Shortcut}' " +
                $"WHERE \"Id\" = '{book.Id}'";
            PSqlConnection.ExecuteData(command);
        }
        #endregion

        #region Удаление данных
        public static void DeleteDataBook(Guid id)
        {
            string command = $"DELETE FROM public.\"Books\" WHERE \"Id\" = '{id}';";
            PSqlConnection.ExecuteData(command);
        }
        #endregion
    }
}
