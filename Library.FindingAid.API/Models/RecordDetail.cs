using System.ComponentModel.DataAnnotations;

namespace Library.FindingAid.API.Models
{
    public class RecordDetail
    {
        [Key]
        public int Id { get; set; }
        public string? Key { get; set; }
        public string? Value { get; set; }
        public int RecordId { get; set; }
    }
}