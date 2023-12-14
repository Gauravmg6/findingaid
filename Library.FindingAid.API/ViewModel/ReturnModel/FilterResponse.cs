using System;
namespace Library.FindingAid.API.ViewModel.ReturnModel
{
	public class FilterResponse
	{
		public List<DropDownBase>? Collections { get; set; } = new();
		public List<ChildBase>? Boxes { get; set; } = new();
		public List<ChildBase>? Folder { get; set; } = new();
	}
}

