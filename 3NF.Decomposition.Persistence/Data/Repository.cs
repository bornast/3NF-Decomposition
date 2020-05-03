using _3NF.Decomposition.Core.Entities;
using _3NF.Decomposition.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        public async Task<IEnumerable<Relation>> GetRelations()
        {
            return await _context.Relations.ToListAsync();
        }

        public void Add<T>(T entity) where T: class
        {
            _context.Add(entity);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }        
    }
}
