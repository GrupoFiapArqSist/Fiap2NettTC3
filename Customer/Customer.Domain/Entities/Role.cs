using Microsoft.AspNetCore.Identity;
using TicketNow.Domain.Interfaces.Entities;

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