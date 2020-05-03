using System.Threading.Tasks;
using _3NF.Decomposition.Core.Dtos;
using _3NF.Decomposition.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace _3NF.Decomposition.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DbController: ControllerBase
    {
        private readonly IDbService _dbService;

        public DbController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRelation(int id)
        {
            var result = await _dbService.GetRelation(id);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetRelations()
        {
            var result = await _dbService.GetRelations();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> DecomposeToThirdNormalForm(int id)
        {
            var result = await _dbService.DecomposeToThirdNormalForm(id);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRelation([FromBody]RelationForCreationDto relationForCreation)
        {
            await _dbService.CreateRelation(relationForCreation);

            return Ok();
        }        

    }
}
