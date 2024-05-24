using BookManagment.Core.Interfaces;
using BookManagment.Core.Models.JWTModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookManagmentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
       
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser(RegisterModel registerModel)
        {
            var result = await _authService.RegisterAsync(registerModel);
            // Check if the User Can Be Added
            return result.IsAuthenticated ? Ok(result) : BadRequest(result.Message);
        }
        [HttpPost("GetUserToken")]
        public async Task< IActionResult> GetUserToken(TokenRequestModel Model)
        {
            var result = await _authService.GetTokenAsync(Model);
            return result.IsAuthenticated ? Ok(result) : BadRequest(result.Message);

        }
        [HttpPost("AddUserToRole")]
        // Allow only Admin To Add Roles To any User
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUserToRole(AddRoleModel Model)
        {
            string result = await _authService.AddRoleAsync(Model);
            return !String.IsNullOrEmpty(result) ? BadRequest(result) : Ok(Model);
        }
    }
}
