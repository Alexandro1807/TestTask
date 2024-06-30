using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTestTask.Models
{
    public class Book
    {
        public Guid Id { get; set; } = Guid.Empty;
        public DateTime LastModified { get; set; } = DateTime.Now;
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public int YearOfProduction { get; set; } = DateTime.Now.Year;
        public string ISBN { get; set; }
        public string Shortcut { get; set; }
        public List<GenreOfBook> GenresOfBook {  get; set; }
        public string GenresOnRow { get; set; }
        public string CoverText { get; set; }
        public byte[] Cover { get; set; }
        public Book(Guid id, DateTime lastModified, string name, string lastName, string firstName, string middleName, int yearOfProduction, string isbn, string shortCut, List<GenreOfBook> genresOfBook = null, string genresOnRow = null, string coverText = null, byte[] cover = null)
        {
            Id = id;
            LastModified = lastModified;
            Name = name == "" ? "undefined" : name;
            LastName = lastName == "" ? "undefined" : lastName;
            FirstName = firstName == "" ? "undefined" : firstName;
            MiddleName = middleName == "" ? "undefined" : middleName;
            YearOfProduction = yearOfProduction;
            ISBN = isbn == "" ? "undefined" : isbn;
            Shortcut = shortCut == "" ? "undefined" : shortCut;
            GenresOfBook = genresOfBook;
            GenresOnRow = genresOnRow;
            CoverText = coverText == "" ? "undefined" : coverText;
            Cover = cover;
        }
    }
}
