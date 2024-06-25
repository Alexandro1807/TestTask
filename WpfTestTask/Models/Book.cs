using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTestTask.Models
{
    class Book
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
        public List<Genre> Genres {  get; set; }
        public string GenresOnRow { get; set; }
        public string CoverText { get; set; }
        public byte[] Cover { get; set; }
        public Book(Guid id, DateTime lastModified, string name, string lastName, string firstName, string middleName, int yearOfProduction, string isbn, string shortCut, List<Genre> genres = null, string genresOnRow = null, string coverText = null, byte[] cover = null)
        {
            Id = id;
            LastModified = lastModified;
            Name = name == "" ? "underfined" : name;
            LastName = lastName == "" ? "underfined" : lastName;
            FirstName = firstName == "" ? "underfined" : firstName;
            MiddleName = middleName == "" ? "underfined" : middleName;
            YearOfProduction = yearOfProduction;
            ISBN = isbn == "" ? "underfined" : isbn;
            Shortcut = shortCut == "" ? "underfined" : shortCut;
            Genres = genres;
            GenresOnRow = genresOnRow;
            CoverText = coverText == "" ? "underfined" : coverText;
            Cover = cover;
        }
    }
}
