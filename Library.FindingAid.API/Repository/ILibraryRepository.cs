using System;
using Library.FindingAid.API.Models;
using Library.FindingAid.API.ViewModel;
using Library.FindingAid.API.ViewModel.ReturnModel;

namespace Library.FindingAid.API.Repository
{
    public interface ILibraryRepository
    {
        FilterResponse ReFormatFilterModel(FilterResponse? data = null);
        List<Item> GetItems(FilterParams? filter = null);
        Task<Item> GetItemByAccessionBoxAndItemNumbers(string accessionNumber, string folderNumber, string boxNumber, int itemNumber);
        Task<string> DeleteItemAsync(string accessionNumber, string folderNumber, string boxNumber, int itemNumber);

        Task<string> CreateItem(CreateAsync request);
    }
}
