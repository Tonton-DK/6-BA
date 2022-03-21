using ClassLibrary;
using Frontend.Data_Providers;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Frontend.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly UserService _userService;

    public IEnumerable<User> Users { get; private set; }
    
    public IndexModel(ILogger<IndexModel> logger, UserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    public void OnGet()
    {
        Users = _userService.GetUsers();
        if (Users == null) return;
    }
}
