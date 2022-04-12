namespace ClassLibrary.Classes;

public class Category
{
    public Category(Guid id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}