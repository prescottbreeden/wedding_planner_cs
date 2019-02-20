using System.ComponentModel.DataAnnotations;

namespace EventPlanner.Models
{
  public class LoginUser
    {
        [Key]
        [Required]
        [EmailAddress (ErrorMessage = "Enter a valid email")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)] 
        public string Password { get; set; }
    }
}
