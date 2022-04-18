using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Frontend.Pages;

public class CreateUserModel : PageModel
{
    private readonly ILogger<CreateUserModel> _logger;
    
    private readonly IUserService _userService;
    
    public Dictionary<Type, bool> ServiceStatus { get; private set; }


    [BindProperty] 
    public UserCreator UserCreator { get; set; } = new();

    public CreateUserModel(ILogger<CreateUserModel> logger,
        IUserService userService)
    {
        _logger = logger;
        _userService = userService;
        ServiceStatus = new Dictionary<Type, bool>();
    }
    
    public IActionResult OnGet()
    {
        return Page();
    }
    
    public IActionResult OnPost()
    {
        _userService.CreateUser(UserCreator);
        return RedirectToPage("Login");
    }
}