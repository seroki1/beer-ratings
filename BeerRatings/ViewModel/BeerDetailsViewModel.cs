using BeerRatings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeerRatings.ViewModel
{
    public class BeerDetailsViewModel
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int BreweryId { get; set; }
        public decimal Abv { get; set; }
        public int StyleId { get; set; }
        public string Notes { get; set; }
        public double AverageRating { get; set; }

        public Brewery Brewery { get; set; }
        public BeerStyle Style { get; set; }
    }
}
