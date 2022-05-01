using ClassLibrary.Classes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Frontend.Pages.Shared;

public class LayoutModel : PageModel
{
    private const string SessionLoggedInKey = "_LoggedIn";
    private const string SessionIdKey = "_Id";
    private const string SessionNameKey = "_Name";
    private const string SessionProfilePictureKey = "_ProfilePicture";
    private const string SessionAgeKey = "_Age";

    public bool SessionLoggedIn => HttpContext.Session.GetInt32(SessionLoggedInKey) == 1;
    public Guid SessionId => Guid.Parse(HttpContext.Session.GetString(SessionIdKey));
    public string? SessionName => HttpContext.Session.GetString(SessionNameKey);
    public string? SessionProfilePicture => HttpContext.Session.GetString(SessionProfilePictureKey);
    public string? SessionAge => HttpContext.Session.GetString(SessionAgeKey);

    public void SetUser(User client)
    {
        HttpContext.Session.SetString(SessionIdKey, client.Id.ToString());
        HttpContext.Session.SetString(SessionNameKey, client.FirstName);
        HttpContext.Session.SetString(SessionProfilePictureKey, client.ProfilePicture);
        HttpContext.Session.SetInt32(SessionLoggedInKey, 1);
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