using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Persistence.Repositories;
using RestAPI.Models;
using RestAPI.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace RestAPI.Services
{
    public class ApiKeysService : IApiKeysService
    {

        private readonly IApiKeysRepository _apiKeysRepository;
        private readonly ApiKeySettings _apiKeySettings; // inserting settings class (from appsettings.json file)

        public ApiKeysService(IApiKeysRepository apiKeysRepository, IOptions<ApiKeySettings> apiKeySettings)
        {
            _apiKeysRepository = apiKeysRepository;
            _apiKeySettings = apiKeySettings.Value; // injecting options value into controller
        }

        public async Task<ApiKeyModel> CreateApiKey(Guid userId)
        {
            var apiKeys = await _apiKeysRepository.GetAllApiKeyAsync(userId);

            var countLimit = _apiKeySettings.MaxApiKeyNumber;

            if (apiKeys.Count() < countLimit) { 

            var key = new byte[32];
            using (var generator = RandomNumberGenerator.Create())
                generator.GetBytes(key);
            var generatedApiKey = Convert.ToBase64String(key);

            var newApiKey = new ApiKeyModel
            {
                Id = Guid.NewGuid(),
                ApiKey = generatedApiKey.ToString(),
                UserId = userId,
                IsActive = true,
                DateCreated = DateTime.Now,
                ExpirationDate = DateTime.Now.AddMinutes(_apiKeySettings.ExpirationTimeInMinutes) // using parameter from appsettings.json
            };

            await _apiKeysRepository.SaveApiKeyAsync(newApiKey.AsDto());

            return newApiKey;
            }
            else if (apiKeys.Count() == countLimit)
            {
                throw new BadHttpRequestException($"Api key count limit of '{countLimit}' is reached ", 400);
            } else
            {
                throw new BadHttpRequestException($"Api key count is more than the limit of '{countLimit}'", 400);

            }
        }

        public async Task<int> DeleteAllApiKeysAsync(Guid userId)
        {
            var rowsAffected = await _apiKeysRepository.DeleteAllAsync(userId);

            return rowsAffected;
        }

        public async Task<IEnumerable<ApiKeyModel>> GetApiKeys(Guid userId)
        {
            var apiKeys = (await _apiKeysRepository.GetAllApiKeyAsync(userId))
            .Select(apiKey => new ApiKeyModel
            {
                Id = apiKey.Id,
                ApiKey = apiKey.ApiKey,
                UserId = apiKey.UserId,
                IsActive = apiKey.IsActive,
                DateCreated = apiKey.DateCreated,
                ExpirationDate = apiKey.ExpirationDate
            });

            return apiKeys;
        }

        public async Task<ApiKeyModel> UpdateApiKeyStatus(Guid apiKeyId, bool state, Guid userId)
        {
            var apiKey = await _apiKeysRepository.GetApiKeyByIdAsync(apiKeyId);


            if (apiKey is null)
            {
                throw new BadHttpRequestException($"Api key with ID: '{apiKeyId}' does not exist", 400);
            }

            if (apiKey.UserId != userId)
            {
                throw new BadHttpRequestException($"Api key with ID: '{apiKeyId}' does not exist for your user", 404);
            }

            await _apiKeysRepository.UpdateIsActive(apiKeyId, state);

            return new ApiKeyModel
            {
                Id = apiKey.Id,
                ApiKey = apiKey.ApiKey,
                UserId = apiKey.UserId,
                IsActive = state,
                DateCreated = apiKey.DateCreated,
                ExpirationDate = apiKey.ExpirationDate
            };
        }

    }
}
