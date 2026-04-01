using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagementApi.Data;
using TaskManagementApi.Data.Repositories;
using TaskManagementApi.Data.Repositories.Interfaces;
using TaskManagementApi.DTOs;
using TaskManagementApi.models;

namespace TaskManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthRepository authRepository) : ControllerBase
    {


        [HttpPost("register")]
        [ProducesResponseType( StatusCodes.Status200OK)]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {

            var user = new User
            {
                Name = registerDto.UserName,
                Email = registerDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                CreatedAt = DateTime.UtcNow,


            };
            await authRepository.RegisterAsync(user);

            return Ok("User registered successfully");
        }


        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {

            var user =await authRepository.GetUserByEmailAsync(loginDto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                return Unauthorized("Invalid credentials");
            }
            
            var token =  authRepository.CreateToken(user);

            return Ok(new { token });



        }

        [HttpGet("profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetProfile()
        {


            System.IO.File.ReadAllText(@"c:\just for testing the error must be in global scheme");
            return Ok();
        }
    }
}
