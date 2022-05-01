using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Frontend.Pages.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Pages;

public class CreateUserModel : LayoutModel
{
    private readonly ILogger<CreateUserModel> _logger;
    
    private readonly IUserService _userService;

    [BindProperty] 
    public UserCreator UserCreator { get; set; } = new();

    public CreateUserModel(ILogger<CreateUserModel> logger,
        IUserService userService)
    {
        _logger = logger;
        _userService = userService;
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