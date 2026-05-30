using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AxisMart.Prez.Ecommerce.Pages;

public class IndexModel : PageModel
{
    public IActionResult OnPostNewsletter(string email)
    {
        // Save email to database or send to API
        TempData["Success"] = "عضویت شما با موفقیت ثبت شد.";
        return RedirectToPage();
    }
}