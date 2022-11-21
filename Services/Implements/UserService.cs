using AutoMapper;
using Data.Models;
using Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Contracts;
using Services.Contracts.Users;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Utils.Constants;

namespace Services.Implements
{
    public class UserService : IUserService
    {
        private readonly IRepository<Guid, User> _users;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IRepository<Guid, User> users, IConfiguration configuration, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _users = users;
            _configuration = configuration;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<UserLoginResponseDto>> LoginAsync(UserLoginRequestDto request)
        {
            var query = _users.GetQueryable();
            var user = await query.FirstOrDefaultAsync(u => request.UserName == u.UserName);
            ServiceResponse<UserLoginResponseDto> response = new ServiceResponse<UserLoginResponseDto>();
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                response.SetFailed("User or password is incorrect");
            }
            else
            {
                var issuer = _configuration["Jwt:Issuer"];
                var audience = _configuration["Jwt:Audience"];
                var key = _configuration["Jwt:Key"];
                var claims = new List<Claim>(new[] {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                });
                if (user.Roles != null)
                {
                    claims.AddRange(user.Roles.Split(',').Select(r => new Claim(ClaimTypes.Role, r)));
                }
                int expireInDays;
                if (request.Remember)
                {
                    expireInDays = int.Parse(_configuration["JWT:RememberExpireInDays"]);
                }
                else
                {
                    expireInDays = int.Parse(_configuration["JWT:ExpireInDays"]);
                }
                var expires = DateTime.Now.AddDays(expireInDays);
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
                var tokenDescriptor = new JwtSecurityToken(issuer, audience, claims, expires: expires, signingCredentials: credentials);
                var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
                var result = new UserLoginResponseDto
                {
                    User = _mapper.Map<User, UserDto>(user),
                    Expires = expires,
                    Token = token
                };
                response.SetValue(result);
            }
            return response;
        }

        public async Task<ServiceResponse<UserDto>> CreateAsync(CreateUpdateUserDto input)
        {
            var user = _mapper.Map<CreateUpdateUserDto, User>(input);
            user.Password = BCrypt.Net.BCrypt.HashPassword(input.Password);
            var response = new ServiceResponse<UserDto>();
            if (await _users.AddAsync(user) != null)
            {
                var dto = _mapper.Map<User, UserDto>(user);
                response.SetValue(dto);
            }
            else
            {
                response.SetFailed();
            }
            return response;
        }

        public async Task<ServiceResponse> DeleteAsync(Guid id)
        {
            if (await _users.DeleteAsync(id))
            {
                return ServiceResponse.CreateSuccess();
            }
            return ServiceResponse.CreateFailed();
        }

        public async Task<ServiceResponse> UpdateAsync(Guid id, CreateUpdateUserDto input)
        {
            var user = await _users.GetAsync(id);
            if (user == null) return ServiceResponse.CreateFailed();
            var isAdmin = _httpContextAccessor.HttpContext.User.IsInRole(Roles.Admin);
            string? roles = user.Roles;
            if (isAdmin)
            {
                input.Password = BCrypt.Net.BCrypt.HashPassword(input.Password);
            }
            else
            {
                input.Password = user.Password;
            }
            if (!isAdmin)
            {
                user.Roles = roles;
            }
            _mapper.Map(input, user);

            if (await _users.UpdateAsync(user) != null)
            {
                return ServiceResponse.CreateSuccess();
            }
            return ServiceResponse.CreateFailed();
        }

        public async Task<ServiceResponse> ChangePassword(ChangePasswordDto input)
        {
            var query = _users.GetQueryable();
            var user = await query.FirstOrDefaultAsync(u => input.UserName == u.UserName);
            if (user == null || !BCrypt.Net.BCrypt.Verify(input.OldPassword, user.Password))
            {
                return ServiceResponse.CreateFailed("Username or password is incorrect!");
            }
            else
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(input.NewPassword);
                await _users.UpdateAsync(user);
                return ServiceResponse.CreateSuccess();
            }
        }

        public async Task<ServiceResponse<UserDto>> GetAsync()
        {
            var response = new ServiceResponse<UserDto>();
            if (_httpContextAccessor.HttpContext.User.Identity != null)
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId != null)
                {
                    var user = await _users.GetAsync(Guid.Parse(userId));
                    if (user != null)
                    {
                        var userDto = _mapper.Map<User, UserDto>(user);
                        response.SetValue(userDto);
                        return response;
                    }
                }
            }
            response.SetFailed();
            return response;
        }
    }
}
