using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.FindingAid.API.Models
{
    public class Box: Base
    {
        public string BoxNumber { get; set; }

 
        [ForeignKey("Collection")]
        public string AccessionNumber { get; set; }

        public ICollection<Folder> Folders { get; set; }
    }
}
