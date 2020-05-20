using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;

namespace AGLTest.Common.Domain.People
{
    public interface IPeopleService
    {
        Task<Result<IEnumerable<Person>>> GetPeople(CancellationToken cancellationToken = default(CancellationToken));
    }
}
