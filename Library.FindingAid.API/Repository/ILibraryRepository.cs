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
        Task<Item> GetItemByAccessionBoxAndItemNumbers(int accessionNumber, int folderNumber, int boxNumber, int itemNumber);
        Task<string> DeleteItemAsync(int accessionNumber, int folderNumber, int boxNumber, int itemNumber);

        Task<string> CreateItem(CreateAsync request);
    }
}
