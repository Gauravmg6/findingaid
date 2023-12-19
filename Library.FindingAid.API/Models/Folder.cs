using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.FindingAid.API.Models
{
    public class Folder : Base
    {
        public string FolderNumber { get; set; }

        [ForeignKey("Box")]
        public string BoxNumber { get; set; }

        [ForeignKey("Collection")]
        public string AccessionNumber { get; set; }

        public ICollection<Item> Items { get; set; }
    }
}
