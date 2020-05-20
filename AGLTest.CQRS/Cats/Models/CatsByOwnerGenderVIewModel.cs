using System.Collections.Generic;

namespace AGLTest.CQRS.Cats.Models
{
    public class CatsByOwnerGenderViewModel
    {
        public string OwnerGender { get; set; }
        public IEnumerable<string> CatNames { get; set; }
    }
}
