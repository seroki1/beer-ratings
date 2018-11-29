using System;
using System.Collections.Generic;

namespace BeerRatings.Models
{
    public partial class Ratings
    {
        public int Id { get; set; }
        public int BeerId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public int UserId { get; set; }

        public Beer Beer { get; set; }
        public User User { get; set; }
    }
}
