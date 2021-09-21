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
        [SessionKey]
        public async Task<IEnumerable<ApiKeyResponse>> GetAllApiKeysAsync() // Useris gali peržiūrėti savo ApiKeys
        {
            var userId = (Guid)HttpContext.Items["userId"];

            var apiKeys = (await _apiKeysRepository.GetAllApiKeyAsync(userId))
                        .Select(apiKey => apiKey.AsDto());

            return apiKeys;
        }

        [HttpPost]
        [SessionKey]
        public async Task<ActionResult<ApiKeyResponse>> CreateApiKey() // Sukurti API key
        {
            var userId = (Guid)HttpContext.Items["userId"];

            var key = new byte[32];
            using (var generator = RandomNumberGenerator.Create())
                generator.GetBytes(key);
            var generatedApiKey = Convert.ToBase64String(key);

            var newApiKey = new ApikeyReadModel
            {
                Id = Guid.NewGuid(),
                ApiKey = generatedApiKey.ToString(),
                UserId = userId,
                IsActive = true,
                DateCreated = DateTime.Now,
                ExpirationDate = DateTime.Now.AddHours(4.00)
            };

            await _apiKeysRepository.SaveApiKeyAsync(newApiKey.AsDto());

            return newApiKey.AsDto();
        }

        [HttpPut]
        [SessionKey]
        [Route("{apiKeyId}/isActive")]
        public async Task<ActionResult<ApiKeyResponse>> UpdateKeyStateAsync(Guid apiKeyId, UpdateApiKeyStateRequest request)
        {
            var apiKey = await _apiKeysRepository.GetApiKeyByIdAsync(apiKeyId);

            if (apiKey is null)
            {
                return NotFound($"Api key with ID: '{apiKeyId}' does not exist");
            }

            await _apiKeysRepository.UpdateIsActive(apiKeyId, request.IsActive);

            return new ApiKeyResponse { 
            Id = apiKey.Id,
            ApiKey = apiKey.ApiKey,
            UserId = apiKey.UserId,
            IsActive = request.IsActive,
            DateCreated = apiKey.DateCreated,
            ExpirationDate = apiKey.ExpirationDate
            };
        }
    }
}