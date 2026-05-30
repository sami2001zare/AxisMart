using AxisMart.Application.Ecommerce.User.Customer.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace AxisMart.Prez.Ecommerce.Pages.register;

public class IndexModel : PageModel
{
    private readonly IMediator _sender;

    public IndexModel(IMediator sender)
    {
        _sender = sender;
    }

    [BindProperty]
    public RegisterViewModel RegisterVm { get; set; } = new();

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var command = new RegisterCustomerCommand(
            new Core.Ecommerce.User.ValueObjects.FirstName(RegisterVm.FirstName),
            new Core.Ecommerce.User.ValueObjects.LastName(RegisterVm.LastName),
            new Core.Ecommerce.User.ValueObjects.Email(RegisterVm.Email),
            new Core.Ecommerce.User.ValueObjects.Phone(RegisterVm.PhoneNumber),
            RegisterVm.Password);

        var result = await _sender.Send(command);
        if (!result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, "Not Registerd");
            return Page();
        }

        //// Store TempRegistrationId in TempData for the next step
        //TempData["TempRegistrationId"] = result.TempRegistrationId;
        HttpContext.Session.SetString("PhoneNumber", RegisterVm.PhoneNumber);
        // for display on OTP page

        return RedirectToPage("/Register/VerifyRegistrationOtp");
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "ایمیل الزامی است")]
        [EmailAddress(ErrorMessage = "فرمت ایمیل صحیح نیست")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "نام الزامی است")]
        [StringLength(50)]
        public string FirstName { get; set; } = "";

        [Required(ErrorMessage = "نام خانوادگی الزامی است")]
        [StringLength(50)]
        public string LastName { get; set; } = "";

        [Required(ErrorMessage = "شماره موبایل الزامی است")]
        [RegularExpression(@"^09[0-9]{9}$", ErrorMessage = "شماره موبایل نامعتبر است")]
        public string PhoneNumber { get; set; } = "";

        [Required(ErrorMessage = "رمز عبور الزامی است")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "تکرار رمز عبور الزامی است")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "رمز عبور و تکرار آن مطابقت ندارند")]
        public string ConfirmPassword { get; set; } = "";
    }
}
