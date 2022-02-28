using System.Threading;
using System.Threading.Tasks;

using UnityEngine;

namespace Banner
{
    public class InspectorProvider<T> : MonoBehaviour, IProvider<T>
        where T : UnityEngine.Object
    {
        [SerializeField] private T[] items;

        private int i;

        private void OnValidate()
        {
            const int MinLength = 1;
            if (items is null || items.Length < MinLength)
            {
                items = new T[MinLength];
            }
        }

        private void Start()
        {
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
