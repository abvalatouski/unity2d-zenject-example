using System.Threading;
using System.Threading.Tasks;

using UnityEngine;

namespace Banner
{
    public class ResourcesProvider<T> : IProvider<T>
        where T : UnityEngine.Object
    {
        private readonly T[] items;
        private int i;

        public ResourcesProvider(string path)
        {
            items = Resources.LoadAll<T>(path);
            i = 0;
        }

        public async Task<T> ProvideAsync(CancellationToken cancellationToken)
        {
            T item = items[i];
            i = (i + 1) % items.Length;
            return await Task.FromResult(item);
        }
    }
}
