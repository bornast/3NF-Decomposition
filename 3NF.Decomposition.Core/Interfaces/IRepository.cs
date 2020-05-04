using _3NF.Decomposition.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _3NF.Decomposition.Core.Interfaces
{
    public interface IRepository
    {
        Task<Relation> GetRelation(int relationId);
        Task<IEnumerable<Relation>> GetRelations();
        void Add<T>(T entity) where T : class;
        void Remove<T>(T entity) where T : class;
        Task SaveAsync();
    }
}
