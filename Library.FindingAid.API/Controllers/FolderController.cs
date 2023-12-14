using Library.FindingAid.API.DataAccess;
using Library.FindingAid.API.Models;
using Library.FindingAid.API.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.FindingAid.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FolderController : ControllerBase
    {
        #region PRIVATE FIELDS
        private readonly ILogger<FolderController> _logger;
        private readonly IApplicationDbContext dbContext;
        #endregion

        #region CONSTRUCTOR
        /// <summary>
        /// Contructor for FolderController
        /// </summary>
        /// <param name="logger">logger instance</param>
        /// <param name="dbContext">db instance</param>
        public FolderController(ILogger<FolderController> logger, IApplicationDbContext dbContext)
        {
            _logger = logger;
            this.dbContext = dbContext;
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> GetAllAsync(int boxId)
        {
            try
            {
                var response = await dbContext.Folder.Where(s => !s.IsDeleted && s.BoxNumber == boxId).Include(s => s.Items.Where(s => !s.IsDeleted)).ToListAsync();
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
                var response = await dbContext.Folder.FirstAsync(s => s.FolderNumber == id && !s.IsDeleted);
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
        public async Task<IActionResult> CreateAsync(Folder Folder)
        {
            try
            {
                Folder.IsDeleted = false;
                Folder.CreatedDate = DateTime.Now;
                Folder.LastUpdatedDate = DateTime.Now;
                var response = await dbContext.Folder.AddAsync(Folder);
                await dbContext.SaveChanges();

                await RecordHelper.SaveRecordAsync(dbContext, Folder);

                if (response == null)
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(Folder);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"an error has occured {ex.Message}");
                throw;
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(Folder Folder)
        {
            try
            {
                Folder.IsDeleted = false;
                Folder.LastUpdatedDate = DateTime.Now;
                var response = dbContext.Folder.Update(Folder);
                await dbContext.SaveChanges();

                await RecordHelper.UpdateRecordAsync(dbContext, Folder);

                if (response == null)
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(Folder);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"an error has occured {ex.Message}");
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var item = await dbContext.Folder.FirstAsync(s => s.FolderNumber == id);
                item.IsDeleted = true;
                var response = dbContext.Folder.Update(item);
                await dbContext.SaveChanges();

                await RecordHelper.DeleteRecordAsync(dbContext, item);

                if (response == null)
                {
                    return BadRequest();
                }
                else
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"an error has occured {ex.Message}");
                throw;
            }
        }
    }
}
