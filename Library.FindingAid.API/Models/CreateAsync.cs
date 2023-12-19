using System.ComponentModel.DataAnnotations;

namespace Library.FindingAid.API.Models
{
    public class CreateAsync
    {
        [Key]
        public string AccessionNumber { get; set; }

        [Required]
        [MaxLength(255)]
        public string? CollectionTitle { get; set; }

        [MaxLength(255)]
        public string? ShelvingLocation { get; set; }

        [Required]
        public string BoxNumber { get; set; }

        [Required]
        public string FolderNumber { get; set; }

        [Required]
        public int ItemNumber { get; set; }
        public string ItemName { get; set; }

        public string ItemDescription { get; set; }

        public int ItemYear { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime CreatedDate { get; internal set; }
        public DateTime LastUpdatedDate { get; internal set; }
        public bool IsDeleted { get; internal set; }
    }
}
