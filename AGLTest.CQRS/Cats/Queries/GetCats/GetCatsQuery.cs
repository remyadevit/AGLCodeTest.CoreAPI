using AGLTest.Common.Response;
using MediatR;

namespace AGLTest.CQRS.Cats.Queries.GetCats
{
    public class GetCatsQuery : IRequest<Response>
    {
    }
}
