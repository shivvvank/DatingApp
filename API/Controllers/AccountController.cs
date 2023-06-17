using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _dbContext;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext dbContext, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _dbContext = dbContext;
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerdto)
        {
            if(await UserExists(registerdto.Username.ToLower()))
                return BadRequest("Username is taken");
            
            using var hmac = new HMACSHA512();
            var user = new AppUser{
                UserName = registerdto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerdto.Password)),
                PasswordSalt = hmac.Key
            };
            _dbContext.Add(user);
            await _dbContext.SaveChangesAsync();

            return new UserDTO{
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO logindto)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.UserName == logindto.Username);
            if(user==null) return Unauthorized("Invalid username");

            var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(logindto.Password));
            for(int i = 0;i<computedHash.Length;i++)
            {
                if(computedHash[i]!=user.PasswordHash[i])
                    return Unauthorized("Incorrect password");
            }
            return new UserDTO{
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }
        public async Task<bool> UserExists(string username)
        {
            return await _dbContext.Users.AnyAsync(x => x.UserName==username.ToLower());
        }

    }
}