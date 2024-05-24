using BookManagment.Core.Models;
using BookManagment.Core.Models.JWTModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagment.Core.Interfaces
{
    public interface IAuthService
    {
        Task<AuthModel> RegisterAsync(RegisterModel model);
        Task<AuthModel> GetTokenAsync(TokenRequestModel model);
        Task<string> AddRoleAsync(AddRoleModel model);
        Task<ApplicationUser> GetUserByIdAsync(string id);
        Task<bool> IsUserAdmin(string userId, String Role);
        Task<List<string>> GetRolesAsync(string userId);
     }
}
