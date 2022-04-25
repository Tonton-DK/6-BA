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
            HttpContext.Session.SetString(SessionIdKey, Client.Id.ToString());
            HttpContext.Session.SetString(SessionNameKey, Client.FirstName);
            HttpContext.Session.SetString(SessionProfilePictureKey, Client.ProfilePicture);
            HttpContext.Session.SetInt32(SessionLoggedInKey, 1);
            return RedirectToPage("Index");
        }
        ViewData["LoginStatus"] = "Wrong email or password";
        return Page();
    }
}