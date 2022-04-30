using System.ComponentModel.DataAnnotations;

namespace ClassLibrary.Classes;

public class Filter
{
    public Filter(Guid? categoryId, DateTime? startDate, DateTime? endDate, string? zip, string? searchQuery)
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
        Zip = string.Empty;
        SearchQuery = string.Empty;
    }

    public Guid? CategoryId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    [DisplayFormat(ConvertEmptyStringToNull = false)] public string Zip { get; set; }
    [DisplayFormat(ConvertEmptyStringToNull = false)] public string SearchQuery { get; set; }
}