using BeerRatings.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeerRatings.ViewModel
{
    public class RatingViewModel
    {
        public int Id { get; set; }
        public int BeerId { get; set; }
        public string Comment { get; set; }
        [Required]
        [Range(1,5)]
        public int Rating { get; set; }
        public int UserId { get; set; }
        public int BreweryId { get; set; }
        public string BeerName { get; set; }
        public string BreweryName { get; set; }

        public Beer Beer { get; set; }
        public User User { get; set; }
    }
}
