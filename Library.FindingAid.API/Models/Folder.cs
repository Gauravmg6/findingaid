using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.FindingAid.API.Models
{
    public class Folder : Base
    {
        public int FolderNumber { get; set; }

        [ForeignKey("Box")]
        public int BoxNumber { get; set; }

        [ForeignKey("Collection")]
        public int AccessionNumber { get; set; }

        public ICollection<Item> Items { get; set; }
    }
}
