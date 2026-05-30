using AxisMart.Application.Ecommerce.User.Manager.Login;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace AxisMart.Prez.Ecommerce.Pages.management;

public class LoginModel : PageModel
{
    private readonly IMediator _sender;

    public LoginModel(IMediator sender) => _sender = sender;

    [BindProperty]
    public AdminLoginViewModel LoginVm { get; set; } = new();

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var command = new LoginManagerCommand(new Core.Ecommerce.User.ValueObjects.Phone(LoginVm.Phone), LoginVm.Password, true);
        var result = await _sender.Send(command);

        if (!result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, "Ridi");
            return Page();
        }


        // Set JWT as HttpOnly cookie
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddHours(8)
        };
        Response.Cookies.Append("jwt_token", result.Value.Token, cookieOptions);

        // Optionally, create ClaimsPrincipal for immediate User access
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Role, "Manager")
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

        return RedirectToPage("/management/Index");
    }

    public class AdminLoginViewModel
    {
        //[Required(ErrorMessage = "نام کاربری را وارد کنید")]
        //public string Username { get; set; } = "";

        [Required(ErrorMessage = "شماره همراه خود را وارد کنید")]
        public string Phone { get; set; } = "";

        [Required(ErrorMessage = "رمز عبور را وارد کنید")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";
    }
}
