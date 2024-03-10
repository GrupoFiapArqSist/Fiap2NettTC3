using Customer.Domain.Enums;

namespace Customer.Domain.Dtos.Auth;

public class RegisterDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Document { get; set; }
    public DocumentTypeEnum DocumentType { get; set; }
}

