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
using RestAPI.Options;
using Microsoft.Extensions.Options;

namespace RestAPI.Controllers
{
    [ApiController]
    [Route("apiKeys")]
    public class ApiKeysController : ControllerBase
    {
        private readonly IApiKeysRepository _apiKeysRepository;
        private readonly ApiKeySettings _apiKeySettings; // inserting settings class (from appsettings.json file)

        public ApiKeysController(IApiKeysRepository apiKeysRepository, IOptions<ApiKeySettings> apiKeySettings)
        {
            _apiKeysRepository = apiKeysRepository;
            _apiKeySettings = apiKeySettings.Value; // injecting options value into controller
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
                ExpirationDate = DateTime.Now.AddMinutes(_apiKeySettings.ExpirationTimeInMinutes) // using parameter from appsettings.json
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