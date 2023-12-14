using Library.FindingAid.API.DataAccess;
using Library.FindingAid.API.Models;
using Library.FindingAid.API.Models.External;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.FindingAid.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {
        #region PRIVATE FIELDS
        private readonly ILogger<SearchController> _logger;
        private readonly IApplicationDbContext dbContext;
        #endregion

        #region CONSTRUCTOR
        /// <summary>
        /// Contructor for CollectionController
        /// </summary>
        /// <param name="logger">logger instance</param>
        /// <param name="dbContext">db instance</param>
        public SearchController(ILogger<SearchController> logger, IApplicationDbContext dbContext)
        {
            _logger = logger;
            this.dbContext = dbContext;
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> QueryAsync(string? query = null, int offset = 0, int limit = 10, bool asc = false)
        {
            try
            {
                query = query != null ? query.ToLower() : string.Empty;
                IEnumerable<Record> data;

                if (asc)
                    data = await dbContext.Record.Include(s => s.Details)
                                            .Where(s => s.Details != null && s.Details.Any(s => s.Value.Contains(query)))
                                            .OrderBy(s => s.LastUpdatedDate)
                                            .Skip(offset)
                                            .Take(limit)
                                            .ToListAsync();
                else
                    data = await dbContext.Record.Include(s => s.Details)
                                            .Where(s => s.Details != null && s.Details.Any(s => s.Value.Contains(query)))
                                            .OrderByDescending(s => s.LastUpdatedDate)
                                            .Skip(offset)
                                            .Take(limit)
                                            .ToListAsync();

                var total = await dbContext.Record.Include(s => s.Details).CountAsync(s => s.Details != null && s.Details.Any(s => s.Value.Contains(query)));

                return Ok(new RecordExternal { Result = data, Total = total });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"an error has occured {ex.Message}");
                throw;
            }
        }
    }
}
