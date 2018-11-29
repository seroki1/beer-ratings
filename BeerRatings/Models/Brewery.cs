using System;
using System.Collections.Generic;

namespace BeerRatings.Models
{
    public partial class Brewery
    {
        public Brewery()
        {
            Beer = new HashSet<Beer>();
        }

        public int Id { get; set; }
        public string BreweryName { get; set; }
        public string Location { get; set; }

        public ICollection<Beer> Beer { get; set; }
    }
}
