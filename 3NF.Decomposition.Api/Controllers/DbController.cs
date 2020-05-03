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
        private readonly IDecompositionService _decompositionService;

        public DbController(IDbService dbService, IDecompositionService decompositionService)
        {
            _dbService = dbService;
            _decompositionService = decompositionService;
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

        [HttpPost]
        public async Task<IActionResult> CreateRelation([FromBody]RelationForCreationDto relationForCreation)
        {
            await _dbService.CreateRelation(relationForCreation);

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> DecomposeToThirdNormalForm(int id)
        {
            var result = await _decompositionService.DecomposeToThirdNormalForm(id);

            return Ok(result);
        }               

    }
}
