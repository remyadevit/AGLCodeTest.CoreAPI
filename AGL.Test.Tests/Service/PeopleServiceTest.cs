using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AGLTest.Common.Settings;
using AGLTest.Service;
using AGLTest.Service.Services.People;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using RichardSzalay.MockHttp;
using Xunit;

namespace AGL.Test.Tests.Service
{
    public class PeopleServiceTest
    {
        private PeopleService _sut;
        private readonly IOptions<AppSettings> _appSettings;

        public PeopleServiceTest()
        {
            _appSettings = Options.Create(new AppSettings() { PeopleURL = "http://PeopleServiceUrl" });
        }

        [Fact]
        public async Task ShouldBeSuccess_GetPeople()
        {
            // Arrange
            var token = CancellationToken.None;
            const string responseJSON = "[{'name':'Bob','gender':'Male','age':23,'pets':[{'name':'Garfield','type':'Cat'},{'name':'Fido','type':'Dog'}]}]";

            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When(_appSettings.Value.PeopleURL)
                    .Respond("application/json", responseJSON);

            var client = mockHttp.ToHttpClient();

            _sut = new PeopleService(
                client,
                _appSettings,
                new ApiResponseHandler(Substitute.For<ILogger<ApiResponseHandler>>()),
            Substitute.For<ILogger<PeopleService>>());

            // Act
            var peopleResponse = await _sut.GetPeople(token);
            var owners = peopleResponse.Value.ToList();

            // Assert
            Assert.True(peopleResponse.IsSuccess);
            Assert.True(owners.Any());
            Assert.Equal("Bob", owners.FirstOrDefault()?.Name);
        }

        [Fact]
        public async Task ShouldFail_GetPeople()
        {
            // Arrange
            var token = CancellationToken.None;
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When(_appSettings.Value.PeopleURL)
                .Respond(req => new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.InternalServerError
                });

            var client = mockHttp.ToHttpClient();

            _sut = new PeopleService(
                client,
                _appSettings,
                new ApiResponseHandler(Substitute.For<ILogger<ApiResponseHandler>>()),
                Substitute.For<ILogger<PeopleService>>());

            // Act
            var peopleResponse = await _sut.GetPeople(token);

            // Assert
            Assert.True(peopleResponse.IsFailed);
        }
    }
}
