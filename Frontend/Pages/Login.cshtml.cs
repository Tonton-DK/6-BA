using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Frontend.Pages;

public class LoginModel : PageModel
{
    private readonly ILogger<LoginModel> _logger;
    
    private readonly IUserService _userService;
    
    public Dictionary<Type, bool> ServiceStatus { get; private set; }

    public User? Client { get; private set; }

    public LoginModel(ILogger<LoginModel> logger,
        IUserService userService)
    {
        _logger = logger;
        _userService = userService;
        ServiceStatus = new Dictionary<Type, bool>();
    }
    
    [BindProperty]
    public LoginRequest LoginRequest { get; set; } = new();

    public IActionResult OnGet()
    {
        return Page();
    }

    public IActionResult OnPost()
    {
        Client = _userService.ValidateUser(new LoginRequest(LoginRequest.Email, LoginRequest.Password));
        if (Client != null)
        {
            // Set Session ID
            return RedirectToPage("Index");
        }
        ViewData["LoginStatus"] = "Wrong email or password";
        return Page();
    }
}