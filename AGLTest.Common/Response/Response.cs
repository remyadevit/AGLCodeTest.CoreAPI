using System.Net;

namespace AGLTest.Common.Response
{
    public class Response
    {
       public Response(HttpStatusCode httpStatusCode)
        {
            HttpStatusCode = httpStatusCode;
        }

        public HttpStatusCode HttpStatusCode { get; }

        public bool IsSuccessfulResponse => (int)HttpStatusCode / 100 == 2;
    }
}
