using System;
using System.Collections.Generic;

namespace BeerRatings.Models
{
    public partial class User
    {
        public User()
        {
            Ratings = new HashSet<Ratings>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public int RoleId { get; set; }

        public UserRole Role { get; set; }
        public ICollection<Ratings> Ratings { get; set; }
    }
}
