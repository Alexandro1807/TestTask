using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTestTask.Models
{
    class Book
    {
        public Guid Id { get; set; }
        public DateTime LastModified { get; set; } = DateTime.Now;
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public int YearOfProduction { get; set; }
        public string ISBN { get; set; }
        public string Cover { get; set; }
        public string Shortcut { get; set; }
        public string Genres { get; set; }
        public Book(Guid id, DateTime lastModified, string name, string firstName, string lastName, string middleName, int yearOfProduction, string isbn, string cover, string shortCut, string genres)
        {
            Id = id;
            LastModified = lastModified;
            Name = name;
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            YearOfProduction = yearOfProduction;
            ISBN = isbn;
            Cover = cover;
            Shortcut = shortCut;
            Genres = genres;
        }
    }
}
