using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Frontend.Pages;

public class CreateUserModel : PageModel
{
    private readonly ILogger<CreateUserModel> _logger;
    
    private readonly IUserService _userService;
    
    public Dictionary<Type, bool> ServiceStatus { get; private set; }
    
    public User User { get; private set; }
    
    public CreateUserModel(ILogger<CreateUserModel> logger,
        IUserService userService)
    {
        _logger = logger;
        _userService = userService;
        ServiceStatus = new Dictionary<Type, bool>();
    }
    
    public void OnGet()
    {
        
    }
}