using System;
using System.Collections.Generic;

namespace BeerRatings.Models
{
    public partial class BeerStyle
    {
        public BeerStyle()
        {
            Beer = new HashSet<Beer>();
        }

        public int Id { get; set; }
        public string Style { get; set; }

        public ICollection<Beer> Beer { get; set; }
    }
}
