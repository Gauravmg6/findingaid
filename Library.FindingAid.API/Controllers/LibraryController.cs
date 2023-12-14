using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.FindingAid.API.Repository;
using Library.FindingAid.API.ViewModel;
using Library.FindingAid.API.ViewModel.ReturnModel;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Library.FindingAid.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LibraryController : Controller
    {
        private readonly ILibraryRepository libraryRepository;
        private readonly IMockDataCreation mockDataCreation;
           

        public LibraryController(ILibraryRepository libraryRepository, IMockDataCreation mockDataCreation)
        {
            this.mockDataCreation = mockDataCreation;
            this.libraryRepository = libraryRepository;
        }

        [HttpPost("GetDropDownContent")]
        public IActionResult GetDropDownContent(FilterResponse? data = null)
        {
            return Ok(libraryRepository.ReFormatFilterModel(data));
        }



        [HttpPost("GetItemsForSelection")]
        public IActionResult GetItemsForSelection(FilterParams? param = null)
        {
            var asd = 324;
            return Ok(libraryRepository.GetItems(param));
        }

        [HttpGet("GenerateData")]
        public async Task<IActionResult> GenerateData()
        {

            await mockDataCreation.GenerateMockData();

            return Ok();

        }
    }
}

