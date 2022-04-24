using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Frontend.Pages.Shared;

public class LayoutModel : PageModel
{
    private const string SessionNameKey = "_Name";
    private const string SessionLoggedInKey = "_LoggedIn";
    public string? SessionName;
    public bool SessionLoggedIn;

    public LayoutModel()
    {
        
    }

    public void Instantiate()
    {
        SessionLoggedIn = HttpContext.Session.GetInt32(SessionLoggedInKey) == 1;
        SessionName = HttpContext.Session.GetString(SessionNameKey);
    }
}