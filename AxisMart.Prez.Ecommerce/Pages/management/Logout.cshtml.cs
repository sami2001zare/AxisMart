using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AxisMart.Prez.Ecommerce.Pages.management;

public class LogoutModel : PageModel
{
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        // 2. Delete the JWT cookie
        Response.Cookies.Delete("jwt_token");

        // 3. Sign out from the cookie authentication scheme (if you also use it)
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        // 4. Redirect to the login page (or home page)
        return RedirectToPage("../management/Login");
    }
}
