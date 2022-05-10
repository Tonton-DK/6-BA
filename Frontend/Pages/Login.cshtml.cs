using System.Net;
using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Frontend.Pages.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Pages;

[IgnoreAntiforgeryToken]
public class LoginModel : LayoutModel
{
    private readonly ILogger<LoginModel> _logger;
    private readonly IUserService _userService;
    
    public User? Client { get; private set; }

    public LoginModel(ILogger<LoginModel> logger,
        IUserService userService)
    {
        _logger = logger;
        _userService = userService;
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
            SetUser(Client);
            return RedirectToPage("Index");
        }
        ViewData["LoginStatus"] = "Wrong email or password";
        return Page();
    }
}