using _3NF.Decomposition.Core.Entities;
using _3NF.Decomposition.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace _3NF.Decomposition.Persistance.Data
{
    public class Repository: IRepository
    {
        private readonly DataContext _context;

        public Repository(DataContext context)
        {
            _context = context;
        }
        public async Task<Relation> GetRelation(int relationId)
        {
            var entity = await _context.Relations.FindAsync(relationId);

            return entity;
        }
    }
}
