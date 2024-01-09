using System.ComponentModel.DataAnnotations;

public class Song
{

    public Song(string author, string title)
    {
        Author = author;
        Title = title;
    }

    public Song()
    {
    }

    [Key]
    public int Id { get; set; }

    public String Author { get; set; }
    public String Title { get; set; }
    public String PlayListName { get; set; }


    public override String ToString()
    {
        return Author + " - " + Title;
    }
    public bool Equals(Song another)
    {
        return this.Author.Equals(another.Author, StringComparison.InvariantCultureIgnoreCase) &
               this.Title.Equals(another.Title, StringComparison.InvariantCultureIgnoreCase);
    }
}