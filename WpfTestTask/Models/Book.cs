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
        public string Genres { get; set; }
        public string CoverText { get; set; }
        public byte[] Cover { get; set; }
        public Book(Guid id, DateTime lastModified, string name, string firstName, string lastName, string middleName, int yearOfProduction, string isbn, string shortCut, string genres, string coverText, byte[] cover)
        {
            Id = id;
            LastModified = lastModified;
            Name = name == "" ? "underfined" : name;
            FirstName = firstName == "" ? "underfined" : firstName;
            LastName = lastName == "" ? "underfined" : lastName;
            MiddleName = middleName == "" ? "underfined" : middleName;
            YearOfProduction = yearOfProduction;
            ISBN = isbn == "" ? "underfined" : isbn;
            Shortcut = shortCut == "" ? "underfined" : shortCut;
            Genres = genres == "" ? "underfined" : genres;
            CoverText = coverText == "" ? "underfined" : coverText;
            Cover = cover;
        }
    }
}
