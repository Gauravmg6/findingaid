using Microsoft.Data.SqlClient.DataClassification;
using System;
namespace Library.FindingAid.API.ViewModel
{
	public class ChildBase
	{
		public string ParentTitle { get; set; }
		public string ParentId { get; set; }

		public string? SuperParentId { get; set; }
		public List<DropDownBase> Content { get; set; }
	}
}

