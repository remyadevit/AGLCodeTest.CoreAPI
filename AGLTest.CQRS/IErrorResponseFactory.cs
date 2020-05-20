using System.Net;
using AGLTest.Common.Response;
using FluentResults;

namespace AGLTest.CQRS
{
    public interface IErrorResponseFactory
    {
        Response CreateErrorResponse(HttpStatusCode httpStatusCode, ResultBase result);
    }
}