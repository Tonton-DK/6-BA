using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Frontend.Pages.Shared;

public class BurgerModel : PageModel
{
    private readonly ILogger<BurgerModel> _logger;
    public Dictionary<Type, bool> ServiceStatus { get; private set; }
    private readonly IUserService _userService;
    
    public User? Client { get; private set; }
    public const string SessionNameKey = "_Name";  
    public const string SessionAgeKey = "_Age";
    public const string SessionLoggedInKey = "_LoggedIn";
    public string? SessionName;
    public bool SessionLoggedIn;
    
    public BurgerModel(ILogger<BurgerModel> logger,
        IUserService userService)
    {
        _logger = logger;
        _userService = userService;
        ServiceStatus = new Dictionary<Type, bool>();
    }
    
    public IActionResult OnGet()
    {
        if (HttpContext.Session.Get(SessionLoggedInKey) == null) return Page();
        SessionLoggedIn = BitConverter.ToBoolean(HttpContext.Session.Get(SessionLoggedInKey) ?? BitConverter.GetBytes(false), 0);
        SessionName = HttpContext.Session.GetString(SessionNameKey);
        return Page();
    }
}