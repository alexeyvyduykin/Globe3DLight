using System.Threading.Tasks;

namespace Globe3DLight.Models.Data
{
    public interface IDatabaseProvider : IDataProvider
    {
        Task Save();
    }
}
