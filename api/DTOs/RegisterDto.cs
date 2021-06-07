using System.ComponentModel.DataAnnotations;
namespace api.DTOs
{
    public class RegisterDto
    {
        [Required]
        [StringLength(8, MinimumLength=4)] //this will throw error if length condition is satisfied
        public string Username {get;set;}
        [Required]
        [StringLength(8, MinimumLength=4)] //this will throw error if length condition is satisfied
        public string Password {get;set;}
    }
}