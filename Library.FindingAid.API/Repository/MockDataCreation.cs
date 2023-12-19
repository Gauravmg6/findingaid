//using System;
//using Library.FindingAid.API.DataAccess;
//using Library.FindingAid.API.Models;

//namespace Library.FindingAid.API.Repository
//{
//	public class MockDataCreation : IMockDataCreation
//    {

//        private  readonly IApplicationDbContext dbContext;

//        public  MockDataCreation(IApplicationDbContext dbContext)
//        {
//            this.dbContext = dbContext;
//        }

//        public async Task GenerateMockData(int count = 4)
//        {
//            try
//            {
//                string accessionNumber = "1";
//                int cGeneratedCount = 1;
//                int bGeneratedCount = 1;
//                int fGeneratedCount = 1;
//                int iGeneratedCount = 1;


//                while (cGeneratedCount < count)
//                {
//                    //Generate Collections
//                    var newCollection = new Collection
//                    {
//                        AccessionNumber = accessionNumber,
//                        CollectionTitle = "C" + cGeneratedCount,
//                        CreatedDate = DateTime.Today,
//                        IsDeleted = false
//                    };

//                    await dbContext.Collection.AddAsync(newCollection);
//                    accessionNumber;
//                    while (bGeneratedCount < count)
//                    {
//                        //Generate Collections
//                        var newBox = new Box
//                        {
//                            BoxNumber = bGeneratedCount,
//                            CreatedDate = DateTime.Today,
//                            IsDeleted = false,
//                            AccessionNumber = newCollection.AccessionNumber
//                        };

//                        await dbContext.Box.AddAsync(newBox);

//                        while (fGeneratedCount < count)
//                        {
//                            //Generate Collections
//                            var newFolder = new Folder
//                            {
//                                FolderNumber = fGeneratedCount,
//                                CreatedDate = DateTime.Today,
//                                IsDeleted = false,
//                                BoxNumber = newBox.BoxNumber,
//                                AccessionNumber = newCollection.AccessionNumber
//                            };

//                            await dbContext.Folder.AddAsync(newFolder);

//                            while (iGeneratedCount < count)
//                            {
//                                //Generate Collections
//                                var newItem = new Item
//                                {
//                                    ItemNumber = iGeneratedCount,
//                                    CreatedDate = DateTime.Today,
//                                    IsDeleted = false,
//                                    FolderNumber = newFolder.FolderNumber,
//                                    BoxNumber = newBox.BoxNumber,
//                                    AccessionNumber = newCollection.AccessionNumber,
//                                    ItemName = "Item "+ newCollection.AccessionNumber.ToString()+" "+newBox.BoxNumber.ToString()+ " "+newFolder.FolderNumber.ToString()

//                                };

//                                await dbContext.Item.AddAsync(newItem);

//                                iGeneratedCount++;
//                            }
//                            iGeneratedCount = 1;
//                            fGeneratedCount++;
//                        }
//                        fGeneratedCount = 1;
//                        bGeneratedCount++;
//                    }
//                    bGeneratedCount = 1;
//                    cGeneratedCount++;
//                }

//                await dbContext.SaveChanges();


//            }
//            catch (Exception Ex)
//            {
//                throw Ex;
//            }
//        }

//	}
//}

