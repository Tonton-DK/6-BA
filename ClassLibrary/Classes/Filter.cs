namespace ClassLibrary.Classes;

public class Filter
{
    public Filter(Guid? categoryId, DateTime? startDate, DateTime? endDate, string zip, string searchQuery)
    {
        CategoryId = categoryId;
        StartDate = startDate;
        EndDate = endDate;
        Zip = zip;
        SearchQuery = searchQuery;
    }

    public Filter()
    {
        CategoryId = null;
        StartDate = null;
        EndDate = null;
        Zip = "";
        SearchQuery = "";
    }

    public Guid? CategoryId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Zip { get; set; }
    public string SearchQuery { get; set; }
}