using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Frontend.Pages.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Pages;

public class LoginModel : LayoutModel
{
    private readonly ILogger<LoginModel> _logger;
    public Dictionary<Type, bool> ServiceStatus { get; private set; }
    private readonly IUserService _userService;
    
    public User? Client { get; private set; }
    public string SessionId = "_Id";
    public string SessionName = "_Name";
    public string SessionAge = "_Age";
    public string SessionLoggedIn = "_LoggedIn";
    
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
        Instantiate();
        return Page();
    }

    public IActionResult OnPost()
    {
        Client = _userService.ValidateUser(new LoginRequest(LoginRequest.Email, LoginRequest.Password));
        if (Client != null)
        {
            HttpContext.Session.SetString(SessionId, Client.Id.ToString());
            HttpContext.Session.SetString(SessionName, Client.FirstName);
            HttpContext.Session.SetInt32(SessionAge, 24);
            HttpContext.Session.SetInt32(SessionLoggedIn, 1);
            return RedirectToPage("Index");
        }
        ViewData["LoginStatus"] = "Wrong email or password";
        return Page();
    }
}