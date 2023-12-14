using System;
namespace Library.FindingAid.API.ViewModel
{
	public class DropDownBase
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public bool isChecked { get; set; } = false;
	}
}

