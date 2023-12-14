using System;
using Library.FindingAid.API.DataAccess;
using Library.FindingAid.API.ViewModel;
using Library.FindingAid.API.ViewModel.ReturnModel;
using Microsoft.EntityFrameworkCore;
using Library.FindingAid.API.Models;
using Library.FindingAid.API.Utils;
using static Library.FindingAid.API.Repository.LibraryRepository;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Library.FindingAid.API.Repository
{
    public class LibraryRepository : ILibraryRepository
    {

        private readonly IApplicationDbContext dbContext;

        public LibraryRepository(IApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// This Method takes the Filter Response as input and formats the entire thing to add more Boxes/Folders or remove based on the selection. 
        /// </summary>
        /// <param name="data"></param>
        /// <returns>
        ///   Returns 3 lists, Collections, Boxes and Folders. All the elements of the Lists have an attribute isChecked which will determine if that entity needs
        ///   to be checked on the Client side. 
        /// </returns>
        public FilterResponse ReFormatFilterModel(FilterResponse? data = null)
        {

            try
            {

                /* First Condition : If the data object is null
                * return the response object with all Collection, Only. */

                if (Object.Equals(data, null) || (data.Boxes.Count == 0 && data.Collections.Count == 0 && data.Folder.Count == 0))

                {
                    FilterResponse response = new();

                    response.Collections = dbContext.Collection.Select(a => new DropDownBase
                    {
                        Id = a.AccessionNumber,
                        Title = a.CollectionTitle ?? string.Empty
                    })
                            .ToList();

                    return response;
                }
                else
                {
                    #region removing uncheked Boxes and Folder

                    List<int> uncheckedCollections = data
                    .Collections
                    .Where(q => !q.isChecked)
                    .Select(w => w.Id)
                    .ToList();


                    List<int> boxesToRemove = data.Boxes.Where(a =>
                    uncheckedCollections.Contains(a.ParentId))
                            .SelectMany(a => a.Content.Select(q => q.Id))
                            .ToList();


                    List<int> uncheckedBoxes = new();
                    data
                    .Boxes
                    .ForEach(a =>
                    {
                        uncheckedBoxes.AddRange(a.Content.Where(q => !q.isChecked).Select(q => q.Id).ToList());
                    });


                    data.Folder.RemoveAll(a => boxesToRemove.Contains(a.ParentId) || uncheckedBoxes.Contains(a.ParentId));

                    data.Boxes.RemoveAll(a => uncheckedCollections
                    .Contains(a.ParentId));

                    #endregion

                    #region Build the Rest

                    #region Build the Boxes

                    var checkedAccessionNumbers = data
                            .Collections
                            .Where(a => a.isChecked)
                            .Select(a => a.Id)
                            .ToList();

                    var alreadyExistingBoxCollections = data.Boxes
                                           .Select(q => q.ParentId)
                                           .ToList();


                    var newBoxes = dbContext
                            .Box
                            .Where(a => checkedAccessionNumbers
                                    .Except(alreadyExistingBoxCollections)
                                    .Contains(a.AccessionNumber))
                            .ToLookup(a => a.AccessionNumber);


                    foreach (var item in newBoxes)
                    {
                        Collection parentCollection = dbContext
                                .Collection
                                .FirstOrDefault(a => a.AccessionNumber == item.Key) ?? new Collection();

                        data.Boxes.Add(new ChildBase
                        {
                            ParentId = parentCollection.AccessionNumber,
                            ParentTitle = parentCollection.CollectionTitle ?? string.Empty,
                            Content = item.Select(a => new DropDownBase
                            {
                                Title = a.BoxNumber.ToString(),
                                Id = a.BoxNumber,
                                isChecked = true
                            }).ToList()
                        });
                    }

                    #endregion

                    #region Build the Folders

                    List<int> checkedBoxIds = new();



                    var alreadyExistingFolderCollections = data.Folder
                                           .Select(q => q.ParentId)
                                           .ToList();


                    List<Folder> newFolders = new();

                    data
                            .Boxes
                            .ForEach(a =>
                            {

                                List<int> boxIds = a.Content
                                                          .Where(a => a.isChecked)
                                                          .Select(a => a.Id)
                                                          .ToList();

                                newFolders.AddRange(
                                                           dbContext.Folder.Where(q => q.AccessionNumber == a.ParentId && boxIds.Contains(q.BoxNumber))

                                                          );

                            });






                    foreach (var item in newFolders.Select(a => a.AccessionNumber).Distinct().ToList())
                    {
                        newFolders.Where(a => a.AccessionNumber == item).Select(a => a.BoxNumber).Distinct().ToList().ForEach(a =>
                        {
                            data.Folder.Add(new ChildBase
                            {
                                ParentId = a,
                                ParentTitle = a.ToString(),
                                SuperParentId = item,
                                Content = newFolders.Where(a => a.AccessionNumber == item).Where(a => a.BoxNumber == a.BoxNumber).Select(a => new DropDownBase
                                {
                                    Id = a.FolderNumber,
                                    isChecked = true,
                                    Title = a.FolderNumber.ToString()
                                }).ToList()
                            });

                        });


                    }

                    #endregion

                    #endregion

                    return data;

                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

        }

        /// <summary>
        /// This method accepts a filter parameter that has a selection from the drop down, Search Params, Paging Params and also a Sorting Order
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>
        /// 1. If the filter is empty or Null, The Method returns all the items in the database.
        /// 2. If the Page Size is 0, the API Returns all the items as per the filter. 
        ///   Returns a list of Items based on the filter
        /// </returns>
        public List<Item> GetItems(FilterParams? filter = null)
        {
            try
            {
                IQueryable<Item> itemQuerable = dbContext.Item;

                if (filter == null || filter.Selections == null)
                {
                    return itemQuerable.ToList();
                }

                // If Selections are present, filter based on them
                List<ChildBase>? foldersThatAreSelected = filter.Selections?.Folder?.Where(a => a.Content.Any(q => q.isChecked)).ToList();

                var predicate = PredicateBuilder.False<Item>();


                if (foldersThatAreSelected?.Any() == true)
                {
                    foldersThatAreSelected.ForEach(folder =>
                    {
                        folder.Content.Where(a => a.isChecked).ToList().ForEach(q =>
                        {
                            predicate = predicate.Or(a => a.AccessionNumber == folder.SuperParentId && a.BoxNumber == folder.ParentId && a.FolderNumber == q.Id);

                        });

                    });

                    itemQuerable = itemQuerable.Where(predicate);

                }

                itemQuerable = itemQuerable.Where(item => item.IsDeleted == false);

                // Search For SubString
                if (!string.IsNullOrEmpty(filter.searchString))
                {
                    itemQuerable = itemQuerable.Where(item =>
                    item.ItemName.ToLower().Contains(filter.searchString.ToLower()) ||
                    (item.ItemDescription != null && item.ItemDescription.ToLower().Contains(filter.searchString.ToLower()))
    );
                }

                // Ordering
                itemQuerable = filter.Asc ? itemQuerable.OrderBy(item => item.ItemName) : itemQuerable.OrderByDescending(item => item.ItemName);

                // Apply Paging Params
                if (filter.PageSize > 0 && filter.PageNumber > 0)
                {
                    return itemQuerable
                            .Skip(filter.PageSize * (filter.PageNumber - 1))
                            .Take(filter.PageSize)
                            .ToList();
                }

                List<Item> items = itemQuerable.ToList();

                ILookup<int, Collection> collections = dbContext.Collection
                    .Where(a => items.Select(a => a.AccessionNumber).Distinct().ToList().Contains(a.AccessionNumber))
                    .ToLookup(a => a.AccessionNumber);

                items.ForEach(a =>
                {

                    Collection? collection = collections[a.AccessionNumber].FirstOrDefault();

                    a.ShelvingLocation = collection?.ShelvingLocation ?? string.Empty;
                    a.CollectionTitle = collection?.CollectionTitle ?? string.Empty;
                });

                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<Item> GetItemByAccessionBoxAndItemNumbers(int accessionNumber, int folderNumber, int boxNumber, int itemNumber)
        {
            return await dbContext.Item.FirstOrDefaultAsync(s =>
                s.AccessionNumber == accessionNumber &&
                s.FolderNumber == folderNumber &&
                s.BoxNumber == boxNumber &&
                s.ItemNumber == itemNumber);
        }

        public async Task<string> DeleteItemAsync(int accessionNumber, int folderNumber, int boxNumber, int itemNumber)
        {
            try
            {
                var item = await GetItemByAccessionBoxAndItemNumbers(accessionNumber, folderNumber, boxNumber, itemNumber);

                if (item != null)
                {
                    item.IsDeleted = true;
                    dbContext.Item.Remove(item);
                    await dbContext.SaveChanges();
                    return "success";
                }
                return "Error Deleting";

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> CreateItem(CreateAsync request)
        {
            try
            {
                // Collection Validation
                Collection existingCollection = dbContext.Collection
            .FirstOrDefault(c => c.AccessionNumber == request.AccessionNumber || c.CollectionTitle == request.CollectionTitle);

                if (existingCollection == null)
                {
                    // Create a new Collection if it doesn't exist
                    Collection newCollection = new Collection
                    {
                        AccessionNumber = request.AccessionNumber,
                        CollectionTitle = request.CollectionTitle,
                        ShelvingLocation = request.ShelvingLocation.ToUpper(),
                        CreatedDate = DateTime.Now,
                        LastUpdatedDate = DateTime.Now,
                        IsDeleted = false
                    };

                    dbContext.Collection.Add(newCollection);
                    await dbContext.SaveChanges();

                    existingCollection = newCollection;
                }

                // Box Validation
                Box existingBox = dbContext.Box
                    .FirstOrDefault(b => b.AccessionNumber == existingCollection.AccessionNumber && b.BoxNumber == request.BoxNumber);

                if (existingBox == null)
                {
                    // Create a new Box if it doesn't exist
                    Box newBox = new Box
                    {
                        AccessionNumber = existingCollection.AccessionNumber,
                        BoxNumber = request.BoxNumber
                    };

                    dbContext.Box.Add(newBox);
                    await dbContext.SaveChanges();

                    existingBox = newBox;

                    // Associate the Box with the Collection
                    existingCollection.Boxes.Add(existingBox);
                    await dbContext.SaveChanges();
                }

                // Folder Validation
                Folder existingFolder = dbContext.Folder
                    .FirstOrDefault(f => f.AccessionNumber == existingCollection.AccessionNumber && f.BoxNumber == existingBox.BoxNumber && f.FolderNumber == request.FolderNumber);

                if (existingFolder == null)
                {
                    // Create a new Folder if it doesn't exist
                    Folder newFolder = new Folder
                    {
                        AccessionNumber = existingCollection.AccessionNumber,
                        BoxNumber = existingBox.BoxNumber,
                        FolderNumber = request.FolderNumber
                    };

                    dbContext.Folder.Add(newFolder);
                    await dbContext.SaveChanges();

                    existingFolder = newFolder;

                    // Associate the Folder with the Box
                    existingBox.Folders.Add(existingFolder);
                    await dbContext.SaveChanges();
                }

                // Item Validation
                bool existingItem = dbContext.Item
                    .Any(i => i.AccessionNumber == existingCollection.AccessionNumber &&
                    i.BoxNumber == existingBox.BoxNumber &&
                    i.FolderNumber == existingFolder.FolderNumber &&
                    i.ItemNumber == request.ItemNumber);


                if (existingItem)
                {

                    return "Item Number already exists";
                }
                else
                {
                    // Create a new Item if it doesn't exist
                    Item newItem = new Item
                    {
                        AccessionNumber = existingCollection.AccessionNumber,
                        BoxNumber = existingBox.BoxNumber,
                        FolderNumber = existingFolder.FolderNumber,
                        ItemName = request.ItemName,
                        ItemNumber = request.ItemNumber,
                        ItemDescription = request.ItemDescription,
                        ItemYear = request.ItemYear,
                        FromDate = request.FromDate,
                        ToDate = request.ToDate,
                        CreatedDate = DateTime.UtcNow,
                        LastUpdatedDate = DateTime.UtcNow
                    };


                    dbContext.Item.Add(newItem);
                    await dbContext.SaveChanges();
                }

                return "Success"; // Return the existing or newly created Collection
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }


    }
}
