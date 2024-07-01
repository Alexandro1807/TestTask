namespace WpfTestTask.Models
{
    public class Genre
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
