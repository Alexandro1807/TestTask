using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTestTask.Models
{
    class Genre
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; }
        public Genre(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
