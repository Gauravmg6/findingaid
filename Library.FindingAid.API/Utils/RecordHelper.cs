using Library.FindingAid.API.DataAccess;
using Library.FindingAid.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.FindingAid.API.Utils
{
    public static class RecordHelper
    {
        public static async Task SaveRecordAsync<T>(IApplicationDbContext dbContext, T data) where T : Base
        {
            Record record = new()
            {
                CreatedDate = data.CreatedDate,
                LastUpdatedDate = data.LastUpdatedDate,
                IsDeleted = false,
                RecordType = GetRecordType(data),
            };

            // assign a record id
            AssignRecordIdBasedOnType(data, record);

            await dbContext.Record.AddAsync(record);
            await dbContext.SaveChanges();

            // create record details based on type
            var recordDetails = CreateRecordDetailsBasedOnType(data, record);

            await dbContext.Details.AddRangeAsync(recordDetails);
            await dbContext.SaveChanges();
        }
        public static async Task UpdateRecordAsync<T>(IApplicationDbContext dbContext, T data) where T : Base
        {
            var record = await dbContext.Record.Include(s => s.Details).FirstAsync(s => s.RecordId == GetRecordIdBasedOnType(data));

            if (record.Details != null && record.Details.Any())
            {
                foreach (var recordDetail in record.Details)
                {
                    recordDetail.Value = GetPropValue(data, recordDetail.Key)?.ToString();
                }

                dbContext.Details.UpdateRange(record.Details);
                await dbContext.SaveChanges();
            }
        }
        public static async Task DeleteRecordAsync<T>(IApplicationDbContext dbContext, T data) where T : Base
        {
            var record = await dbContext.Record.Include(s => s.Details).FirstAsync(s => s.RecordId == GetRecordIdBasedOnType(data));

            if (record.Details != null && record.Details.Any()) dbContext.Details.RemoveRange(record.Details);
            dbContext.Record.Remove(record);
            await dbContext.SaveChanges();
        }

        private static RecordType GetRecordType<T>(T data) where T : Base
        {
            switch (typeof(T).Name)
            {
                case "Collection":
                    return RecordType.Collection;
                case "Box":
                    return RecordType.Box;
                case "Folder":
                    return RecordType.Folder;
                case "Item":
                    return RecordType.Item;
                default:
                    throw new InvalidDataException("Invalid type of Record!");
            }
        }
        private static int GetRecordIdBasedOnType<T>(T data) where T : Base
        {
            switch (typeof(T).Name)
            {
                case "Collection":
                    return (data as Collection).AccessionNumber;
                case "Box":
                    return (data as Box).BoxNumber;
                case "Folder":
                    return (data as Folder).FolderNumber;
                case "Item":
                    return (data as Item).ItemNumber;
                default:
                    throw new InvalidDataException("Invalid type of Record!");
            }
        }
        private static void AssignRecordIdBasedOnType<T>(T data, Record record) where T : Base
        {
            switch (typeof(T).Name)
            {
                case "Collection":
                    record.RecordId = (data as Collection).AccessionNumber;
                    break;
                case "Box":
                    record.RecordId = (data as Box).BoxNumber;
                    break;
                case "Folder":
                    record.RecordId = (data as Folder).FolderNumber;
                    break;
                case "Item":
                    record.RecordId = (data as Item).ItemNumber;
                    break;
                default:
                    throw new InvalidDataException("Invalid type of Record!");
            }
        }
        private static List<RecordDetail> CreateRecordDetailsBasedOnType<T>(T data, Record record)
        {
            switch (typeof(T).Name)
            {
                case "Collection":
                    return new List<RecordDetail>() {
                        new RecordDetail {Key = "CollectionTitle", RecordId = record.Id, Value = (data as Collection).CollectionTitle},
                    };
                case "Box":
                    return new List<RecordDetail>() {
                        new RecordDetail {Key = "BoxNumber", RecordId = record.Id, Value = (data as Box).BoxNumber.ToString()},
                    };
                case "Folder":
                    return new List<RecordDetail>() {
                        new RecordDetail {Key = "FolderNumber", RecordId = record.Id, Value = (data as Folder).FolderNumber.ToString()}
                    };
                case "Item":
                    return new List<RecordDetail>() {
                        new RecordDetail {Key = "ItemName", RecordId = record.Id, Value = (data as Item).ItemName},
                    };
                default:
                    throw new InvalidDataException("Invalid type of Record!");
            }
        }
        private static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
    }
}
