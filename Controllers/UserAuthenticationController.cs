using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieStore.Models.DTO;
using MovieStore.Repositories.Abstract;
using MovieStore.Repositories.Implementation;

namespace MovieStore.Controllers
{
    public class UserAuthenticationController : Controller
    {
        private readonly IUserAuthenticationService _service;

        public UserAuthenticationController(IUserAuthenticationService service)
        {
            this._service = service;
        }

        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registration(Registration model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            model.Role = "user";
            var result = await this._service.RegiatrationAsync(model);
            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(Registration));
        }


        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>Login(Login model) 
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _service.LoginAsync(model);
            if(result.StatusCode == 1)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["msg"] = result.Message;
                return RedirectToAction(nameof(Login));
            }
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _service.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }
        //public async Task<IActionResult> Reg()
        //{
        //    var model = new Registration
        //    {
        //        UserName = "Admin",
        //        Name = "DotNet",
        //        Email = "DotNet@gmail.com",
        //        Password = "Admin@123",
        //        PasswordConfirm = "Admin@123"
        //    };  
        //    model.Role = "Admin";
        //    var result = await _service.RegiatrationAsync(model);
        //    return Ok(result);
            
        //}
    }
}
