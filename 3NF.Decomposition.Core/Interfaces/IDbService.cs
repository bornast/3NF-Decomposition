using _3NF.Decomposition.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace _3NF.Decomposition.Core.Interfaces
{
    public interface IDbService
    {
        void DecomposeToThirdNormalForm(int relationId);
        Task CreateRelation(RelationForCreationDto relationForCreation);
    }
}
