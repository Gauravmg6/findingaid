using System.ComponentModel.DataAnnotations;

namespace Library.FindingAid.API.Models
{
    public class Record : Base
    {
        [Key]
        public int Id { get; set; }
        public int RecordId { get; set; }
        public RecordType RecordType { get; set; }

        public virtual ICollection<RecordDetail>? Details { get; set; }

    }

    public enum RecordType
    {
        Collection,
        Box,
        Folder,
        Item
    }
}
