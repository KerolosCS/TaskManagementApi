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
        public IActionResult GetProfile()
        {


            System.IO.File.ReadAllText(@"c:\lolo");
            return Ok();
        }
    }
}
