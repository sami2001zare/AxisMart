using AxisMart.Application.Ecommerce.User.Customer.ValidateRegistration;
using AxisMart.Core.Ecommerce.User.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace AxisMart.Prez.Ecommerce.Pages.Register;

public class VerifyRegistrationOtpModel : PageModel
{
    private readonly ISender _sender;

    public VerifyRegistrationOtpModel(ISender sender)
    {
        _sender = sender;
    }

    [BindProperty]
    [Required(ErrorMessage = "کد تایید الزامی است")]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "کد باید ۶ رقم باشد")]
    public string OtpCode { get; set; } = "";


    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        string otpCode = OtpCode;

        string? phoneNumber = HttpContext.Session.GetString("PhoneNumber");
        Phone phone = new(phoneNumber ?? string.Empty);

        var command = new CustomerVaidateRegisterationCommand(phone, OtpCode);
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
        Response.Cookies.Append("jwt_token", result.Value, cookieOptions);

        // Optionally, create ClaimsPrincipal for immediate User access
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Role, "Customer")
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

        return RedirectToPage("/my-account/Index");
    }

    public async Task<IActionResult> OnPostResendAsync()
    {
        var tempId = TempData["TempRegistrationId"] as string;
        if (string.IsNullOrEmpty(tempId))
        {
            ModelState.AddModelError(string.Empty, "لطفاً دوباره از صفحه ثبت‌نام اقدام کنید.");
            return Page();
        }

        // Call a resend command (similar to initiate but reuses the pending registration)
        // For brevity, assume we have a ResendOtpCommand that uses the stored pending data
        // You would need to implement a ResendRegistrationOtpCommand that re-generates OTP and updates cache.

        TempData["Message"] = "کد جدید ارسال شد.";
        return RedirectToPage();
    }
}