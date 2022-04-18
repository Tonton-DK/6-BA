using ClassLibrary.Classes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Frontend.Pages;

public class LoginModel : PageModel
{
    public User User { get; private set; }

    [BindProperty]
    public LoginRequest LoginRequest { get; set; }
    public void OnGet()
    {
        
    }
}