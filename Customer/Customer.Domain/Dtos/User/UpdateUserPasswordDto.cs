namespace Customer.Domain.Dtos.User;

    public class UpdateUserPasswordDto
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }

