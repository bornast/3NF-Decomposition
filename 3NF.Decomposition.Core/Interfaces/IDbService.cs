using System;
using System.Collections.Generic;
using System.Text;

namespace _3NF.Decomposition.Core.Interfaces
{
    public interface IDbService
    {
        void DecomposeToThirdNormalForm(int relationId);
    }
}
