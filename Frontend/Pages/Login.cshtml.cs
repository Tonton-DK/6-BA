using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Frontend.Pages;

public class LoginModel : PageModel
{
    private readonly ILogger<LoginModel> _logger;
    public Dictionary<Type, bool> ServiceStatus { get; private set; }
    private readonly IUserService _userService;
    
    public User? Client { get; private set; }
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
        ViewData[SessionName] = HttpContext.Session.GetString(SessionName);
        if(HttpContext.Session.Get(SessionLoggedIn) != null)
        {
            ViewData[SessionLoggedIn] = BitConverter.ToBoolean(HttpContext.Session.Get(SessionLoggedIn) ?? BitConverter.GetBytes(false), 0);
        }
        
        return Page();
    }

    public IActionResult OnPost()
    {
        Client = _userService.ValidateUser(new LoginRequest(LoginRequest.Email, LoginRequest.Password));
        if (Client != null)
        {
            // Set Session ID
            HttpContext.Session.SetString(SessionName, Client.Id.ToString());
            HttpContext.Session.SetInt32(SessionAge, 24);
            HttpContext.Session.Set(SessionLoggedIn, BitConverter.GetBytes(true));
            return RedirectToPage("Index");
        }
        ViewData["LoginStatus"] = "Wrong email or password";
        return Page();
    }
}