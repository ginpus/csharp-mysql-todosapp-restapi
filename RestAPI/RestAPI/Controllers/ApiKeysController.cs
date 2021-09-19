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
using Contracts.Models.RequestModels;
using Contracts.Models.ResponseModels;

namespace RestAPI.Controllers
{
    [ApiController]
    [Route("apiKeys")]
    public class ApiKeysController : ControllerBase
    {
        private readonly IApiKeysRepository _apiKeysRepository;
        private readonly IUsersRepository _userRepository;

        public ApiKeysController(IApiKeysRepository apiKeysRepository, IUsersRepository userRepository)
        {
            _apiKeysRepository = apiKeysRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        [ApiKey]
        //[Route("apikey")]
        public async Task<IEnumerable<ApiKeyResponse>> GetAllApiKeysAsync() // Useris gali peržiūrėti savo ApiKeys
        {
            var userId = (Guid)HttpContext.Items["userId"];

            var apiKeys = (await _apiKeysRepository.GetAllApiKeyAsync(userId))
                        .Select(apiKey => apiKey.AsDto());

            return apiKeys;
        }

        [HttpPost]
        public async Task<ActionResult<ApiKeyResponse>> CreateApiKey(ApiKeyRequest request)
        {
            var user = await _userRepository.GetUserAsync(request.UserName, request.Password);

            if (user is null)
            {
                return Unauthorized("Wrong username or password");
            }

            var key = new byte[32];
            using (var generator = RandomNumberGenerator.Create())
                generator.GetBytes(key);
            var generatedApiKey = Convert.ToBase64String(key);

            var newApiKey = new ApiKeyModel
            {
                Id = Guid.NewGuid(),
                ApiKey = generatedApiKey.ToString(),
                UserId = user.UserId,
                IsActive = true,
                DateCreated = DateTime.Now,
                ExpirationDate = DateTime.Now.AddHours(4.00)
            };

            await _apiKeysRepository.SaveApiKeyAsync(newApiKey.AsDto());

            return newApiKey.AsDto();
        }
    }
}