using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentResults;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AGLTest.Service
{
    public class ApiResponseHandler : IApiResponseHandler
    {
        private readonly ILogger<ApiResponseHandler> _logger;

        public ApiResponseHandler(ILogger<ApiResponseHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result<TResult>> HandleResponse<TResult>(HttpResponseMessage response)
        {
            // Handle if Status Code is not Success
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage =
                    $"Error - API response retrieved with error code '{response.StatusCode}' from - '{response.RequestMessage.RequestUri}'";
                _logger.LogError(errorMessage);

                return Results.Fail<TResult>(new Error(errorMessage));
            }

            // Handle if no content found
            if (response.Content == null)
            {
                var errorMessage =
                    $"Error - API '{response.RequestMessage.RequestUri}' response retrieved with no content";

                _logger.LogError(errorMessage);

                return Results.Fail<TResult>(new Error(errorMessage));
            }

            try
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TResult>(jsonContent);

                _logger.LogTrace("Handled the response returned by '{RequestUri}'", response.RequestMessage.RequestUri);

                return Results.Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception,
                    "Error in the response returned by '{RequestUri}'. Falling back to the error handler.",
                    response.RequestMessage.RequestUri);

                return Results.Fail<TResult>(new Error(exception.Message));
            }
        }
    }
}
