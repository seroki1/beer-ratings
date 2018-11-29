using System;
using System.Collections.Generic;

namespace BeerRatings.Models
{
    public partial class Beer
    {
        public Beer()
        {
            Ratings = new HashSet<Ratings>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int BreweryId { get; set; }
        public decimal Abv { get; set; }
        public int StyleId { get; set; }
        public string Notes { get; set; }

        public Brewery Brewery { get; set; }
        public BeerStyle Style { get; set; }
        public ICollection<Ratings> Ratings { get; set; }
    }
}
