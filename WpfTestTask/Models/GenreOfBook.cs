namespace WpfTestTask.Models
{
    public class GenreOfBook
    {
        public Guid Id { get; set; } = Guid.Empty;
        public DateTime LastModified { get; set; }
        public Guid BookId { get; set; } = Guid.Empty;
        public Guid GenreId { get; set; } = Guid.Empty;
        public GenreOfBook(Guid id, DateTime lastModified, Guid bookId, Guid genreId)
        {
            Id = id;
            LastModified = lastModified;
            BookId = bookId;
            GenreId = genreId;
        }
    }
}
