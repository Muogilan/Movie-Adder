using MovieStore.Models.DTO;

namespace MovieStore.Repositories.Abstract
{
    public interface IUserAuthenticationService
    {
        Task<Status> LoginAsync(Login model);

        Task<Status> RegiatrationAsync(Registration model);

        Task LogoutAsync();
    }
}
