using System.ComponentModel.DataAnnotations;

namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class TokenValidationDto
    {
        [Required(ErrorMessage = "Token is required")]
        public string Token { get; set; }
    }
} 