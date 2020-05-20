using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AGLTest.Common.Domain.People;
using AGLTest.Common.Response;
using AGLTest.CQRS;
using AGLTest.CQRS.Cats.Models;
using AGLTest.CQRS.Cats.Queries.GetCats;
using FluentResults;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace AGL.Test.Tests.CQRS
{
    public class GetCatsHandlerTest
    {
        [Fact]
        public async Task Should_GetCountries_SortedByLabel()
        {
            // Arrange
            var peopleService = Substitute.For<IPeopleService>();

            var getCountriesResult = Results.Ok<IEnumerable<Person>>(new[]
            {
                new Person()
                {
                    Gender = "Male", Pets = new[]
                    {
                        new Pet() {Type = "Cat", Name = "Cat1"},
                    }
                },
                new Person()
                {
                    Gender = "Female", Pets = new[]
                    {
                        new Pet() {Type = "Cat", Name = "Cat2"},
                    }
                },
                new Person()
                {
                    Gender = "Male", Pets = new[]
                    {
                        new Pet() {Type = "Dog", Name = "Dog1"},
                    }
                },
            });

            peopleService.GetPeople().Returns(getCountriesResult);

            var handler = new GetCatsHandler(peopleService, Substitute.For<IErrorResponseFactory>(),
                Substitute.For<ILogger<GetCatsHandler>>());

            // Act
            var response = await handler.Handle(new GetCatsQuery(), new System.Threading.CancellationToken());

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.HttpStatusCode);

            Assert.IsType<ResponseValue<IEnumerable<CatsByOwnerGenderViewModel>>>(response);

            var valueResponse = (ResponseValue<IEnumerable<CatsByOwnerGenderViewModel>>)response;

            Assert.Equal(2, valueResponse.Value.Count() );
            Assert.Equal("Male", valueResponse.Value.FirstOrDefault()?.OwnerGender);
            Assert.Equal(1, valueResponse.Value.FirstOrDefault()?.CatNames.Count());
        }
    }
}
