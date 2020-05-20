using System.Net;
using FluentResults;

namespace AGLTest.Common.Response
{
    public class ErrResponse : ResponseValue<Error>
    {
        public ErrResponse(Error error) : base(HttpStatusCode.BadRequest, error)
        {
        }

        public ErrResponse(HttpStatusCode httpStatusCode, Error error) : base(httpStatusCode, error)
        {
        }
    }
}
