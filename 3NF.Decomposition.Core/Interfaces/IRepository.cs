using _3NF.Decomposition.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace _3NF.Decomposition.Core.Interfaces
{
    public interface IRepository
    {
        Task<Relation> GetRelation(int relationId);
    }
}
