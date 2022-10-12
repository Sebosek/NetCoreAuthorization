using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

[Authorize]
public class Protected : PageModel
{
    public string AccessToken { get; private set; } = string.Empty;
    
    public string IdentityToken { get; private set; } = string.Empty;
    
    public async Task OnGetAsync()
    {
        AccessToken = await HttpContext.GetTokenAsync("access_token") ?? "No access token";
        IdentityToken = await HttpContext.GetTokenAsync("id_token") ?? "No identity token";
    }
}