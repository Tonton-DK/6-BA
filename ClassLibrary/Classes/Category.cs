namespace ClassLibrary.Classes;

public class Category
{
    public Category(Guid id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
    
    public Category()
    {
        Id = Guid.Empty;
        Name = "";
        Description = "";
    }

    public Category()
    {
        Id = Guid.Empty;
        Name = String.Empty;
        Description = String.Empty;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}