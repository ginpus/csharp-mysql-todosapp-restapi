using Contracts.Models.RequestModels;
using Contracts.Models.ResponseModels;
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
        private readonly ISessionRepository _sessionRepository;

        public AuthController(IUsersRepository userRepository, ISessionRepository sessionRepository)
        {
            _userRepository = userRepository;
            _sessionRepository = sessionRepository;
        }

        [HttpPost]
        [Route("signUp")]
        //[ApiKey] // as this is new user, API key does not exist at all
        public async Task<ActionResult<UserResponse>> CreateUser(AddUserRequest user) // Useris gali susikurti account'ą
        {
            //var userId = (Guid)HttpContext.Items["userId"];

            var currentUser = await _userRepository.GetUserAsync(user.UserName);

            if (currentUser is not null)
            {
                return Conflict($"User with Username: '{user.UserName}' already exists!");
            }

            var newUser = new UserWriteModel
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

        [HttpPost]
        [Route("signIn")]
        public async Task<ActionResult<SessionKeyResponse>> CreateUserSession(string username, string password) // Useris gali susikurti sesija
        {
            var user = await _userRepository.GetUserAsync(username, password);

            if (user is null)
            {
                return BadRequest("Wrong username or password");
            }

            var key = new byte[32];
            using (var generator = RandomNumberGenerator.Create())
                generator.GetBytes(key);
            var generatedSessionKey = Convert.ToBase64String(key);

            var newSessionKey = new SessionKeyWriteModel
            {
                SessionId = Guid.NewGuid(),
                SessionKey = generatedSessionKey,
                UserId = user.UserId,
                IsActive = true,
                DateCreated = DateTime.Now,
                ExpirationDate = DateTime.Now.AddMinutes(15.00)
            };

            await _sessionRepository.SaveSessionKeyAsync(newSessionKey);

            return newSessionKey.AsDto();
            //return CreatedAtAction(nameof(GetTodoItemByIdAsync), new { Id = todoItem.Id }, todoItem.AsDto());
        }
    }
}