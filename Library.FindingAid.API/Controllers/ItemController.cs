using Library.FindingAid.API.DataAccess;
using Library.FindingAid.API.Models;
using Library.FindingAid.API.Repository;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.FindingAid.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemController : ControllerBase
    {
        #region PRIVATE FIELDS
        private readonly ILogger<ItemController> _logger;
        private readonly IApplicationDbContext dbContext;
        private readonly ILibraryRepository libraryRepository;
        #endregion

        #region CONSTRUCTOR
        /// <summary>
        /// Contructor for ItemController
        /// </summary>
        /// <param name="logger">logger instance</param>
        /// <param name="dbContext">db instance</param>
        public ItemController(ILogger<ItemController> logger, IApplicationDbContext dbContext, ILibraryRepository libraryRepository)
        {
            _logger = logger;
            this.dbContext = dbContext;
            this.libraryRepository = libraryRepository;
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> GetAllAsync(string folderId)
        {
            try
            {
                var response = await dbContext.Item.Where(s => !s.IsDeleted && s.FolderNumber == folderId).ToListAsync();
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
        public async Task<IActionResult> GetByIDAsync(int id)
        {
            try
            {
                var response = await dbContext.Item.FirstAsync(s => s.ItemNumber == id && !s.IsDeleted);
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
        public async Task<IActionResult> CreateAsync(Item item)
        {
            try
            {
                item.IsDeleted = false;
                item.CreatedDate = DateTime.Now;
                item.LastUpdatedDate = DateTime.Now;
                var response = await dbContext.Item.AddAsync(item);
                await dbContext.SaveChanges();

                if (response == null)
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(item);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"an error has occured {ex.Message}");
                throw;
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> UpdateAsync(Item item)
        //{
        //    try
        //    {
        //        item.IsDeleted = false;
        //        item.LastUpdatedDate = DateTime.Now;
        //        var response = dbContext.Item.Update(item);
        //        await dbContext.SaveChanges();

        //        if (response == null)
        //        {
        //            return BadRequest();
        //        }
        //        else
        //        {
        //            return Ok(item);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"an error has occured {ex.Message}");
        //        throw;
        //    }
        //}

        [HttpGet("{accessionNumber}/{boxNumber}/{folderNumber}/{itemNumber}")]
        public async Task<IActionResult> DeleteAsync(string accessionNumber, string folderNumber, string boxNumber, int itemNumber)
        {
            try
            {
                await libraryRepository.DeleteItemAsync(accessionNumber, folderNumber, boxNumber, itemNumber);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error has occurred: {ex.Message}");
                return BadRequest();
            }
        }

    }
}
