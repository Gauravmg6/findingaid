using Library.FindingAid.API.DataAccess;
using Library.FindingAid.API.Models;
using Library.FindingAid.API.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.FindingAid.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BoxController : ControllerBase
    {
        #region PRIVATE FIELDS
        private readonly ILogger<BoxController> _logger;
        private readonly IApplicationDbContext dbContext;
        #endregion

        #region CONSTRUCTOR
        /// <summary>
        /// Contructor for BoxController
        /// </summary>
        /// <param name="logger">logger instance</param>
        /// <param name="dbContext">db instance</param>
        public BoxController(ILogger<BoxController> logger, IApplicationDbContext dbContext)
        {
            _logger = logger;
            this.dbContext = dbContext;
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> GetAllAsync(int AccessionNumber)
        {
            try
            {
                var response = await dbContext.Box.Where(s => !s.IsDeleted && s.AccessionNumber == AccessionNumber).Include(s => s.Folders.Where(s => !s.IsDeleted)).ToListAsync();
                if (response == null)
                {
                    Console.WriteLine(response);
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
                var response = await dbContext.Box.FirstAsync(s => s.BoxNumber == id && !s.IsDeleted);
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
        public async Task<IActionResult> CreateAsync(Box Box)
        {
            try
            {
                Box.IsDeleted = false;
                Box.CreatedDate = DateTime.Now;
                Box.LastUpdatedDate = DateTime.Now;
                var response = await dbContext.Box.AddAsync(Box);
                await dbContext.SaveChanges();

                await RecordHelper.SaveRecordAsync(dbContext, Box);

                if (response == null)
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(Box);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"an error has occured {ex.Message}");
                throw;
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(Box Box)
        {
            try
            {
                Box.IsDeleted = false;
                Box.LastUpdatedDate = DateTime.Now;
                var response = dbContext.Box.Update(Box);
                await dbContext.SaveChanges();

                await RecordHelper.UpdateRecordAsync(dbContext, Box);

                if (response == null)
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(Box);
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
                var item = await dbContext.Box.FirstAsync(s => s.BoxNumber == id);
                item.IsDeleted = true;
                var response = dbContext.Box.Update(item);
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
