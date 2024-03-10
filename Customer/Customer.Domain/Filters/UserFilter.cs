namespace Customer.Domain.Filters;

public class UserFilter : _BaseFilter
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Document { get; set; }
    public bool? Active { get; set; }
    public bool? Approved { get; set; }
}

