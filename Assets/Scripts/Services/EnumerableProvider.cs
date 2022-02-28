using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Banner
{
    public class EnumerableProvider<T> : IProvider<T>
    {
        private readonly IEnumerator<T> items;

        public EnumerableProvider(IEnumerable<T> items)
        {
            this.items = items.GetEnumerator();
        }

        public EnumerableProvider(params T[] items) : this((IEnumerable<T>)items)
        {
        }

        public async Task<T> ProvideAsync(CancellationToken cancellationToken)
        {
            if (items.MoveNext())
            {
                return await Task.FromResult(items.Current);
            }

            items.Reset();
            if (!items.MoveNext())
            {
                throw new InvalidOperationException("Expected at least one item.");
            }

            return await Task.FromResult(items.Current);
        }
    }
}
