using Customer.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using TicketNow.Domain.Interfaces.Entities;

namespace Customer.Domain.Entities;

public class User : IdentityUser<int>, IEntity<int>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Document { get; set; }
    public DocumentTypeEnum DocumentType { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool Active { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    public string PhotoUrl { get; set; }
    public bool Approved { get; set; }    
}
