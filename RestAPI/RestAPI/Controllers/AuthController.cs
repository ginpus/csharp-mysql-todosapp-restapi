using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence.Models;
using Persistence.Models.ReadModels;
using Persistence.Repositories;
using RestAPI.Attributes;
using RestAPI.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace RestAPI.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUsersRepository _userRepository;

        public AuthController(IUsersRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        [Route("signUp")]
        //[ApiKey] // as this is new user, API key does not exist at all
        public async Task<ActionResult<UserDto>> CreateUser(AddUserDto user) // Useris gali susikurti account'ą
        {
            //var userId = (Guid)HttpContext.Items["userId"];

            var currentUser = await _userRepository.GetUserAsync(user.UserName);

            if (currentUser is not null)
            {
                return Conflict($"User with Username: '{user.UserName}' already exists!");
            }

            var newUser = new User
            {
                UserId = Guid.NewGuid(),
                UserName = user.UserName,
                Password = user.Password,
                DateCreated = DateTime.Now
            };

            await _userRepository.CreateUserAysnc(newUser);

            return newUser.AsDto();
            //return CreatedAtAction(nameof(GetTodoItemByIdAsync), new { Id = todoItem.Id }, todoItem.AsDto());
        }
    }
}