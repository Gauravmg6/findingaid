using Library.FindingAid.API.DataAccess;
using Library.FindingAid.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Library.FindingAid.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository adminRepository;

        public AdminController(IAdminRepository adminRepository)
        {
            this.adminRepository = adminRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await adminRepository.GetAll());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("AddAdmin")]
        public async Task<IActionResult> AddAdmin(string UserId)
        {
            try
            {
                await adminRepository.AddAdmin(UserId);
                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete("RemoveAdmin")]
        public async Task<IActionResult> RemoveAdmin(string UserId)
        {
            try
            {
                await adminRepository.RemoveAdmin(UserId);
                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("IsAdmin")]
        public async Task<IActionResult> IsAdmin(string UserId)
        {
            try
            {
                return Ok(await adminRepository.IsAdmin(UserId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
