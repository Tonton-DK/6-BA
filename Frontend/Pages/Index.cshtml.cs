using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Frontend.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IUserService _userService;
    private readonly IJobService _jobService;

    public IEnumerable<User> Users { get; private set; }
    public IEnumerable<Category> Categories { get; private set; }
    public IEnumerable<Job> Jobs { get; private set; }
    
    public IndexModel(ILogger<IndexModel> logger, IUserService userService, IJobService jobService)
    {
        _logger = logger;
        _userService = userService;
        _jobService = jobService;
    }

    public void OnGet()
    {
        Users = _userService.Get();
        Categories = _jobService.ListCategories();
        var data = new Filter(
            Guid.Parse("0ebbe367-300a-4c86-9070-d6e106d7e4b9"), 
            DateTime.Now.AddDays(-7), 
            DateTime.Now.AddDays(7), 
            "5000",
            "");
        Jobs = _jobService.ListJobs(data);
    }
}
