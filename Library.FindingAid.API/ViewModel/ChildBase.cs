using Microsoft.Data.SqlClient.DataClassification;
using System;
namespace Library.FindingAid.API.ViewModel
{
	public class ChildBase
	{
		public string ParentTitle { get; set; }
		public int ParentId { get; set; }

		public int SuperParentId { get; set; }
		public List<DropDownBase> Content { get; set; }
	}
}

