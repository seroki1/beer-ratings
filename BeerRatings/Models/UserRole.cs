using System;
using System.Collections.Generic;

namespace BeerRatings.Models
{
    public partial class UserRole
    {
        public UserRole()
        {
            User = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Role { get; set; }

        public ICollection<User> User { get; set; }
    }
}
