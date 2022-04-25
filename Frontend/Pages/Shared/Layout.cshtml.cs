using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Frontend.Pages.Shared;

public class LayoutModel : PageModel
{
    public const string SessionIdKey = "_Id";
    public const string SessionNameKey = "_Name";
    public const string SessionLoggedInKey = "_LoggedIn";
    public const string SessionAgeKey = "_Age";
    
    public string? SessionName;
    public bool SessionLoggedIn;

    public void Instantiate()
    {
        SessionLoggedIn = HttpContext.Session.GetInt32(SessionLoggedInKey) == 1;
        SessionName = HttpContext.Session.GetString(SessionNameKey);
    }
}