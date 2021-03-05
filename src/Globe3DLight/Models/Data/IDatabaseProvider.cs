using System.Threading.Tasks;

namespace Globe3DLight.Data
{
    public interface IDatabaseProvider : IDataProvider
    {
        Task Save();
    }
}
