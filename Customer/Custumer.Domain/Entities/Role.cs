using Customer.Domain.Interfaces.Entities;
using Microsoft.AspNetCore.Identity;

namespace Customer.Domain.Entities;

    public class Role : IdentityRole<int>, IEntity<int>
    {
        public Role(string roleName)
        {
            Name = roleName;
            NormalizedName = roleName;
        }
        public Role() { }
    }
