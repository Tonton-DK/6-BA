using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Frontend.Pages;

public class LoginModel : PageModel
{
    private readonly ILogger<CreateUserModel> _logger;
    
    private readonly IUserService _userService;
    
    public User? Client { get; private set; }

    [BindProperty]
    public LoginRequest LoginRequest { get; set; }
    public IActionResult OnGet()
    {
        return Page();
    }
    
    public IActionResult OnPost()
    {
        Client = _userService.ValidateUser(LoginRequest);
        if (Client != null)
        {
            
        }
        else
        {
            ViewData["LoginStatus"] = "Wrong email or password";
        }
        return RedirectToPage("Login");
    }
}