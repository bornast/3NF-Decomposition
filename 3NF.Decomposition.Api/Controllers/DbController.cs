using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _3NF.Decomposition.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        public IActionResult DecomposeToThirdNormalForm(int id)
        {
            _dbService.DecomposeToThirdNormalForm(id);
            return Ok();
        }
    }
}
