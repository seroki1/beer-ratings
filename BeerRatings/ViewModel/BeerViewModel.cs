using BeerRatings.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeerRatings.ViewModel
{
    public class BeerViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int BreweryId { get; set; }
        [Required]
        [Range(0.01,99.99)]
        public decimal Abv { get; set; }
        public int StyleId { get; set; }
        
        public string Notes { get; set; }
        public IEnumerable<SelectListItem> BreweryList { get; set; }
        public IEnumerable<SelectListItem> StyleList { get; set; }

        public Brewery Brewery { get; set; }
        public BeerStyle Style { get; set; }
    }
}
