using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AGLTest.Common.Domain.People;
using AGLTest.Common.Settings;
using FluentResults;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AGLTest.Service.Services.People
{
    public class PeopleService : HttpServiceBase, IPeopleService
    {
        private readonly string _peopleServiceUrl;
        private readonly ILogger<PeopleService> _logger;
        private readonly IApiResponseHandler _apiResponseHandler;

        public PeopleService(HttpClient httpClient, IOptions<AppSettings> settings,
            IApiResponseHandler apiResponseHandler,
            ILogger<PeopleService> logger) : base(httpClient, logger)
        {
            _apiResponseHandler = apiResponseHandler ?? throw new ArgumentNullException(nameof(apiResponseHandler));
            _peopleServiceUrl = settings?.Value?.PeopleURL ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result<IEnumerable<Person>>> GetPeople(CancellationToken cancellationToken = default)
        {
            var response = await GetAsync(_peopleServiceUrl, cancellationToken);
            var result = await _apiResponseHandler.HandleResponse<IEnumerable<Person>>(response);

            _logger.LogTrace("Handled Person API Call");

            return result;
        }
    }
}
