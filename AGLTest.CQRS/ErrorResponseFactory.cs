using System.Linq;
using System.Net;
using AGLTest.Common.Response;
using FluentResults;

namespace AGLTest.CQRS
{
    public class ErrorResponseFactory : IErrorResponseFactory
    {
        public Response CreateErrorResponse(HttpStatusCode httpStatusCode, ResultBase result)
        {
            return new ErrResponse(httpStatusCode, result.Errors?.FirstOrDefault());
        }
    }
}
