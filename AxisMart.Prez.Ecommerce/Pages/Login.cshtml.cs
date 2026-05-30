using AxisMart.Application.Ecommerce.User.Customer.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace AxisMart.Prez.Ecommerce.Pages;

public class LoginModel : PageModel
{
    private readonly IMediator _sender;

    public LoginModel(IMediator sender) => _sender = sender;

    [BindProperty]
    public LoginViewModel LoginVm { get; set; } = new();

    public string? MessageSam { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var command = new LoginCustomerCommand(LoginVm.Phone, LoginVm.Password, true);
        var result = await _sender.Send(command);
        
        if (!result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, "Ridi");
            return Page();
        }

        // Store the generated OTP in TempData or cache (for the verify step)
        //TempData["OtpCode"] = 23;
        //TempData["EmailOrPhone"] = LoginVm.EmailOrPhone;

        // Redirect to verify page
        return RedirectToPage("/my-account/Index");
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "لطفا شماره موبایل را وارد کنید")]
        public string Phone { get; set; } = "";

        [Required(ErrorMessage = "رمز عبور خود را وارد کنید")]
        public string Password { get; set; } = "";
    }
}