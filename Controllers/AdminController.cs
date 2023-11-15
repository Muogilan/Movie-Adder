using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MovieStore.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        public IActionResult Display()
        {
            return View();
        }
    }
}
