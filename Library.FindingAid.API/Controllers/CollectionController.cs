using Library.FindingAid.API.DataAccess;
using Library.FindingAid.API.Models;
using Library.FindingAid.API.Repository;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.FindingAid.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CollectionController : ControllerBase
    {
        #region PRIVATE FIELDS
        private readonly ILogger<CollectionController> _logger;
        private readonly IApplicationDbContext dbContext;
        private readonly ILibraryRepository libraryRepository;

        #endregion

        #region CONSTRUCTOR
        /// <summary>
        /// Contructor for CollectionController
        /// </summary>
        /// <param name="logger">logger instance</param>
        /// <param name="dbContext">db instance</param>
        public CollectionController(ILogger<CollectionController> logger, IApplicationDbContext dbContext, ILibraryRepository libraryRepository)
        {
            _logger = logger;
            this.dbContext = dbContext;
            this.libraryRepository = libraryRepository;
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var response = await dbContext.Collection.Where(s => !s.IsDeleted).Include(s => s.Boxes.Where(s => !s.IsDeleted)).ToListAsync();
                if (response == null)
                {
                    return StatusCode(204);
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"an error has occured {ex.Message}");
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIDAsync(string id)
        {
            try
            {
                var response = await dbContext.Collection.FirstAsync(s => s.AccessionNumber == id && !s.IsDeleted);
                if (response == null)
                {
                    return StatusCode(204);
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"an error has occured {ex.Message}");
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateAsync request)
        {
            try
            {

                var response = await libraryRepository.CreateItem(request);

                if (response == "Success")
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest(response);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error has occurred: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }


        [HttpPost("{AccessionNumber}/{BoxNumber}/{FolderNumber}/{ItemNumber}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] string AccessionNumber, [FromRoute] string BoxNumber, [FromRoute] string FolderNumber, [FromRoute] int ItemNumber, [FromBody] CreateAsync request)
        {
            try
            {

                var existingItem = dbContext.Item
                    .Where(a => a.AccessionNumber == AccessionNumber)
                    .Where(a => a.BoxNumber == BoxNumber)
                    .Where(a => a.FolderNumber == FolderNumber)
                    .Where(a => a.ItemNumber == ItemNumber)
                    .FirstOrDefault();

                if (existingItem != null)
                {
                    dbContext.Item.Remove(existingItem);

                    await dbContext.SaveChanges();
                    var response = await libraryRepository.CreateItem(request);


                    if (response == "Success")
                    {
                        return Ok(response);
                    }
                    else
                    {
                        return BadRequest(response);
                    }
                }

                return BadRequest("Item Already Success");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error has occurred: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }


        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteAsync(string id)
        //{
        //    try
        //    {
        //        var item = await dbContext.Collection.FirstAsync(s => s.AccessionNumber == id);
        //        item.IsDeleted = true;
        //        var response = dbContext.Collection.Update(item);
        //        await dbContext.SaveChanges();

        //        await RecordHelper.DeleteRecordAsync(dbContext, item);

        //        if (response == null)
        //        {
        //            return BadRequest();
        //        }
        //        else
        //        {
        //            return Ok();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"an error has occured {ex.Message}");
        //        throw;
        //    }
        //}
    }
}
