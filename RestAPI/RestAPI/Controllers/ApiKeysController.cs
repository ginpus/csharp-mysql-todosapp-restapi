using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence.Models;
using Persistence.Models.ReadModels;
using Persistence.Repositories;
using RestAPI.Attributes;
using Contracts.Models.RequestModels;
using Contracts.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using RestAPI.Options;
using Microsoft.Extensions.Options;
using RestAPI.Services;
using RestAPI.Models;

namespace RestAPI.Controllers
{
    [ApiController]
    [Route("apiKeys")]
    public class ApiKeysController : ControllerBase
    {
        private readonly IApiKeysService _apiKeysService;
        
        public ApiKeysController(IApiKeysService apiKeysService)
        {
            _apiKeysService = apiKeysService;
        }

        [HttpGet]
        [SessionKey]
        public async Task<IEnumerable<ApiKeyResponse>> GetAllApiKeysAsync() // Useris gali peržiūrėti savo ApiKeys
        {
            var userId = (Guid)HttpContext.Items["userId"];

            var apiKeys = (await _apiKeysService.GetApiKeys(userId))
            .Select(apiKey => new ApiKeyResponse
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

        [HttpPost]
        [SessionKey]
        public async Task<ActionResult<ApiKeyResponse>> CreateApiKey() // Sukurti API key
        {
            var userId = (Guid)HttpContext.Items["userId"];

            try { 
            var newApiKey = await _apiKeysService.CreateApiKey(userId);

            return new ApiKeyResponse
            {
                Id = newApiKey.Id,
                ApiKey = newApiKey.ApiKey,
                UserId = newApiKey.UserId,
                IsActive = newApiKey.IsActive,
                DateCreated = newApiKey.DateCreated,
                ExpirationDate = newApiKey.ExpirationDate
            };
            }
            catch (BadHttpRequestException exception)
            {
                switch (exception.StatusCode)
                {
                    case 404:
                        return NotFound(exception.Message);
                    case 400:
                        return BadRequest(exception.Message);
                    default: throw;
                }
            }
        }

        [HttpPut]
        [SessionKey]
        [Route("{apiKeyId}/isActive")]
        public async Task<ActionResult<ApiKeyResponse>> UpdateKeyStateAsync(Guid apiKeyId, UpdateApiKeyStateRequest request)
        {
            var userId = (Guid)HttpContext.Items["userId"];

            try { 
            var updatedApiKey = await _apiKeysService.UpdateApiKeyStatus(apiKeyId, request.IsActive, userId);

            return new ApiKeyResponse
            {
                Id = updatedApiKey.Id,
                ApiKey = updatedApiKey.ApiKey,
                UserId = updatedApiKey.UserId,
                IsActive = updatedApiKey.IsActive,
                DateCreated = updatedApiKey.DateCreated,
                ExpirationDate = updatedApiKey.ExpirationDate
            };
            }
            catch (BadHttpRequestException exception)
            {
                switch (exception.StatusCode)
                {
                    case 404:
                        return NotFound(exception.Message);
                    case 400:
                        return BadRequest(exception.Message);
                    default: throw;
                }
            }
        }
    }
}