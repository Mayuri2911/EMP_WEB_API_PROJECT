using BAL.Services.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static DAL.Model.EmpRegister.EmpRegister;
using static DAL.Model.Login.Login;

namespace EMP_WEB_API_PROJECT.Controllers.Login
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class LoginController : ControllerBase
    {
        private ILoginService _loginService;
        public LoginController(ILoginService loginService)
        {

            _loginService = loginService;
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            LoginResponse response = new LoginResponse();
            try
            {
                await _loginService.Login(loginRequest, response);
                return Ok(response);
            }
            catch (System.Exception ex)
            {
                response.isSuccess = false;
                response.message = ex.Message.ToString();

            }
            return Ok(response);
        }


    }
}