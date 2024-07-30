using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ecommerce.Controllers
{
    public class ContactController : Controller
    {
        private readonly EmailService _emailService;

        public ContactController(EmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task<IActionResult> SendTestEmail()
        {
            try
            {
                await _emailService.SendEmailAsync("ahmadalzoubi01999@gmail.com", "any think", "<p>This is a test email.</p>");
                return View("Success"); // Create a Success.cshtml view to show a success message
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return View("Error", ex.Message); // Create an Error.cshtml view to show the error message
            }
        }
    }
}
