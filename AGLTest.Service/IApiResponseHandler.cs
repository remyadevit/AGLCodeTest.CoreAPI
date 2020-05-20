using System.Net.Http;
using System.Threading.Tasks;
using FluentResults;

namespace AGLTest.Service
{
    public interface IApiResponseHandler
    {
        Task<Result<TResult>> HandleResponse<TResult>(HttpResponseMessage response);
    }
}