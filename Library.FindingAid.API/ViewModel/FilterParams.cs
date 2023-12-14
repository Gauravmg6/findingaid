using System;
using Library.FindingAid.API.ViewModel.ReturnModel;

namespace Library.FindingAid.API.ViewModel
{
	public class FilterParams
	{
		public FilterResponse? Selections { get; set; }
		public string searchString { get; set; }
		public int PageNumber { get; set; }
		public int PageSize { get; set; } = 0;
		public bool Asc { get; set; } = false;
	}
}

