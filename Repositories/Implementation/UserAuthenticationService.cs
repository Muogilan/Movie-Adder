using Microsoft.AspNetCore.Identity;
using MovieStore.Models.Domain;
using MovieStore.Models.DTO;
using MovieStore.Repositories.Abstract;
using System.Security.Claims;

namespace MovieStore.Repositories.Implementation
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public UserAuthenticationService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
            
        }
        public async Task<Status> LoginAsync(Login model)
        {
            var status = new Status();
            var user = await userManager.FindByNameAsync(model.Username);
            if (user == null) 
            {
                status.StatusCode = 0;
                status.Message = "Invalid UserName";  
                return status;
            }
            //We will match our Password
            if (!await userManager.CheckPasswordAsync(user,model.Password))
            {
                status.StatusCode = 0;
                status.Message = "Invalid Password";
                return status;
            }
            var signInResult = await signInManager.PasswordSignInAsync(user, model.Password, false, true);   
            if (signInResult.Succeeded)
            {
                //Add our roles in here
                var userRoles = await userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName)
                };
                foreach(var userRole in  userRoles)
                {
                    authClaims.Add(new(ClaimTypes.Role, userRole));
                }
                status.StatusCode = 1;
                status.Message = "Logged in Sucessfully";
                return status;
            }
            else if (signInResult.IsLockedOut)
            {
                status.StatusCode = 0;
                status.Message = "User Locked out";
                return status;
            }
            else
            {
                status.StatusCode = 0;
                status.Message = "Error in Logging in";
                return status;
            }
        }

        public async Task LogoutAsync()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<Status> RegiatrationAsync(Registration model)
        {
            var Status = new Status();
            var UserExists = await userManager.FindByNameAsync(model.UserName);
            if (UserExists != null)
            {
                Status.StatusCode = 0;
                Status.Message = "User already exits!";
                return Status;
            }
            ApplicationUser user = new ApplicationUser
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                Name = model.Name,
                Email = model.Email,
                UserName = model.UserName,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,

            };

            var result  = await userManager.CreateAsync(user,model.Password);
            if (!result.Succeeded)
            {
                Status.StatusCode = 0;
                Status.Message = "User Creation Failed";
                return Status;
            }

            //Role Management
            if(!await roleManager.RoleExistsAsync(model.Role))
                await roleManager.CreateAsync(new IdentityRole(model.Role));

            if (await roleManager.RoleExistsAsync(model.Role))
            {
                await userManager.AddToRoleAsync(user, model.Role);
            }

            Status.StatusCode = 1;
            Status.Message = "User has registered sucessfully ";
            return Status;
        }
    }
}
