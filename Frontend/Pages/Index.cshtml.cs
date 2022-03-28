using ClassLibrary;
using ClassLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Frontend.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IUserService _userService;
    private readonly IJobService _jobService;

    public IEnumerable<User> Users { get; private set; }
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
        Jobs = _jobService.Get();
    }
}
