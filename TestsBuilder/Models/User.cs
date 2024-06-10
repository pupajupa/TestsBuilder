using System.ComponentModel.DataAnnotations;

namespace TestsBuilder.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; } // Primary key
        
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string Login { get; set; }
        
        [Required]
        public string Password { get; set; }

        [Required]
        public string LastName { get; set; }
        public byte[] Image { get; set; }
    }

}
