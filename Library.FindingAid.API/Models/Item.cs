using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.FindingAid.API.Models
{
    public class Item : Base
    {
        public int ItemNumber { get; set; }

        public string? ItemName { get; set; }

        public int ItemYear { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? ItemDescription { get; set; }


        [ForeignKey("Folder")]
        public string FolderNumber { get; set; }


        [ForeignKey("Box")]
        public string BoxNumber { get; set; }


        [ForeignKey("Collection")]
        public string AccessionNumber { get; set; }

        [NotMapped]
        public string? ShelvingLocation { get; set; }
        [NotMapped]
        public string? CollectionTitle { get; set; }
    }
}
