using ClassLibrary.Classes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Frontend.Pages;

public class LoginModel : PageModel
{
    public User User { get; private set; }

    [BindProperty]
    public string Password { get; private set; }
    public void OnGet()
    {
        
    }
}