using System.ComponentModel.DataAnnotations;

namespace Library.FindingAid.API.Models
{
    public class Admin
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
    }
}
