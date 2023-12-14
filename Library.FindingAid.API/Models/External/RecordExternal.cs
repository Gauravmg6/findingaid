using System.Linq;

namespace Library.FindingAid.API.Models.External
{
    public class RecordExternal
    {
        public IEnumerable<Record>? Result { get; set; }
        public int Count { get { return Result == null ? 0 : Result.Count(); } }
        public int Total { get; set; }
    }
}
