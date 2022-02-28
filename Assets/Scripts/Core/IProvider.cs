using System.Threading;
using System.Threading.Tasks;

namespace Banner
{
    public interface IProvider<T>
    {
        Task<T> ProvideAsync(CancellationToken cancellationToken);
    }
}
