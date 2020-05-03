using System.Threading.Tasks;

namespace _3NF.Decomposition.Core.Interfaces
{
    public interface IDecompositionService
    {
        Task<string> DecomposeToThirdNormalForm(int relationId);
    }
}
