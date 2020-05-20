using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AGLTest.CQRS.Cats.Models;
using AGLTest.CQRS.Cats.Queries.GetCats;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AGLCodeTest.API.Controllers
{
    public class PetsController : HomeController
    {
        [HttpGet(Name = "getCats")]
        [ProducesResponseType(typeof(IEnumerable<CatsByOwnerGenderViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<CatsByOwnerGenderViewModel>>> GetCats(CancellationToken cancellationToken)
        {
            // Create Mediator request
            var results = await Mediator.Send(new GetCatsQuery(), cancellationToken);

            // Transform response to Type
            return results.ToActionResult<IEnumerable<CatsByOwnerGenderViewModel>>();
        }
    }
}
