using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Frontend.Pages.Shared;

public class LayoutModel : PageModel
{
    public const string SessionIdKey = "_Id";
    public const string SessionNameKey = "_Name";
    public const string SessionProfilePictureKey = "_ProfilePicture";
    public const string SessionLoggedInKey = "_LoggedIn";
    public const string SessionAgeKey = "_Age";
    
    public string? SessionName;
    public string? SessionProfilePicture;
    public bool SessionLoggedIn;

    public void Instantiate()
    {
        SessionLoggedIn = HttpContext.Session.GetInt32(SessionLoggedInKey) == 1;
        SessionName = HttpContext.Session.GetString(SessionNameKey);
        SessionProfilePicture = HttpContext.Session.GetString(SessionProfilePictureKey);
    }

    public IActionResult OnPostLogout()
    {
        HttpContext.Session.SetString(SessionIdKey, "");
        HttpContext.Session.SetString(SessionNameKey, "");
        HttpContext.Session.SetString(SessionProfilePictureKey, "");
        HttpContext.Session.SetInt32(SessionLoggedInKey, 0);
        return RedirectToPage("Index");
    }
}