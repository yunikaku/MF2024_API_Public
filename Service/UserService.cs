using MF2024_API.Models;
using Microsoft.AspNetCore.Identity;

namespace MF2024API.Service
{
    public class UserService
    {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IList<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("Invalid user ID");
            }

            var roles = await _userManager.GetRolesAsync(user);
            return roles;
        }
    }
}
