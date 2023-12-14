using System.ComponentModel.DataAnnotations;

namespace Library.FindingAid.API.Models
{
    public class Collection : Base
    {
        public int AccessionNumber { get; set; }

        [Required]
        [MaxLength(255)]
        public string? CollectionTitle { get; set; }

        [MaxLength(255)]
        public string? ShelvingLocation { get; set; }

        public ICollection<Box> Boxes { get; set; }
        public ICollection<Folder> Folders { get; set; }
        public ICollection<Item> Items { get; set; }
    }
}
