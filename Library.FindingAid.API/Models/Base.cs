namespace Library.FindingAid.API.Models
{
    public class Base
    {
        public DateTime CreatedDate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
