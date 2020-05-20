using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AGLTest.Common.Domain.People;
using AGLTest.Common.Response;
using AGLTest.CQRS.Cats.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AGLTest.CQRS.Cats.Queries.GetCats
{
    public class GetCatsHandler : IRequestHandler<GetCatsQuery, Response>
    {
        private const string ERROR_MESSAGE = "Error retrieving pets owner";
        private readonly IPeopleService _peopleService;
        private readonly IErrorResponseFactory _errorResponseFactory;
        private readonly ILogger<GetCatsHandler> _logger;

        public GetCatsHandler(IPeopleService peopleService,
            IErrorResponseFactory errorResponseFactory,
            ILogger<GetCatsHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _errorResponseFactory = errorResponseFactory ?? throw new ArgumentNullException(nameof(errorResponseFactory));
            _peopleService = peopleService ?? throw new ArgumentNullException(nameof(peopleService));
        }

        public async Task<Response> Handle(GetCatsQuery request, CancellationToken cancellationToken)
        {
            // Get People From PeopleAPI
            var peopleResult = await _peopleService.GetPeople(cancellationToken);

            // Handle if FluentResult Failed
            if (peopleResult.IsFailed)
            {
                var response = _errorResponseFactory.CreateErrorResponse(HttpStatusCode.InternalServerError, peopleResult);
                _logger.LogError(ERROR_MESSAGE);
                return response;
            }

            // Convert To List
            var ownerList = peopleResult.Value.ToList();

            // Handle if no people found
            if (!ownerList.Any())
            {
                var response = _errorResponseFactory.CreateErrorResponse(HttpStatusCode.NotFound, peopleResult);
                _logger.LogError(ERROR_MESSAGE);
                return response;
            }

            // Group cats by Owner Gender and sort alphabetically
            var genderWithCats = ownerList
                .Where(owner => owner.Pets != null && owner.Pets.Any())
                .GroupBy(owner => owner.Gender)
                .Select(
                    group => new CatsByOwnerGenderViewModel
                    {
                        OwnerGender = group.Key,
                        CatNames = group
                            .SelectMany(p => p.Pets)
                            .Where(p => p.Type.Equals("Cat", StringComparison.OrdinalIgnoreCase))
                            .Select(pet => pet.Name)
                            .OrderBy(name => name)
                    });


            // Log for Trace
            _logger.LogTrace("Successfully transformed to groups.");

            // Box result to ValueResponse
            return new ResponseValue<IEnumerable<CatsByOwnerGenderViewModel>>(HttpStatusCode.OK, genderWithCats);
        }
    }
}
