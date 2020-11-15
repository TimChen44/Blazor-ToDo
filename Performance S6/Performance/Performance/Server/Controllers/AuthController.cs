using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Performance.Entity;
using Performance.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Performance.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {

        public AuthController()
        {

        }

        [HttpPost]
        public ResultData<UserDto> Login(LoginDto dto)
        {
            //模拟获得Token
            var jwtToken = GetToken(dto.UserName);

            return new(new() { Name = dto.UserName, Token = jwtToken });
        }

        [HttpGet]
        public ResultData<UserDto> GetUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var name = User.Claims.First(x => x.Type == ClaimTypes.Name).Value;
                //模拟获得Token
                var jwtToken = GetToken(name);

                return new ResultData<UserDto>(new UserDto() { Name = name, Token = jwtToken });
            }
            else
            {
                return new ResultData<UserDto>(false, null);
            }

        }

        public string GetToken(string name)
        {
            //此处加入账号密码验证代码

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name,name),
                new Claim(ClaimTypes.Role,"Admin"),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("dd%88*377f6d&f£$$£$#$%#$%#$FF33fssDG^!3"));
            var expires = DateTime.Now.AddDays(30);
            var token = new JwtSecurityToken(
                issuer: "guetServer",
                audience: "guetClient",
                claims: claims,
                notBefore: DateTime.Now,
                expires: expires,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
