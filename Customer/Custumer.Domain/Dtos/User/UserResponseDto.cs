using Customer.Domain.Enums;

namespace Customer.Domain.Dtos.User;

public class UserResponseDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Document { get; set; }
    public DocumentTypeEnum DocumentType { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool Active { get; set; }
    public bool Approved { get; set; }
}

