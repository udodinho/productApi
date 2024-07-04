using System.ComponentModel.DataAnnotations;

namespace Product.Application.Dtos
{
    public record RegisterDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }

    public record LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public record LoginResponse : RegisterResponse
    {

    }

    public record RegisterResponse
    {
        public string Token { get; set; }
        public string UserId { get; set; }
    }

}
