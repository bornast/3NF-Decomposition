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
        private readonly IRelationService _relationService;
        private readonly IDecompositionService _decompositionService;

        public DbController(IRelationService dbService, IDecompositionService decompositionService)
        {
            _relationService = dbService;
            _decompositionService = decompositionService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRelation(int id)
        {
            var result = await _relationService.GetRelation(id);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetRelations()
        {
            var result = await _relationService.GetRelations();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRelation([FromBody]RelationForCreationDto relationForCreation)
        {
            await _relationService.CreateRelation(relationForCreation);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRelation(int id)
        {
            await _relationService.DeleteRelation(id);

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
