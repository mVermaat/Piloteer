using System.Collections.Concurrent;
using Microsoft.Playwright;

namespace Piloteer
{
    internal class ConcurrentPool<TIdentifier, TItem>
        where TIdentifier : notnull
    {
        private ConcurrentDictionary<TIdentifier, ConcurrentQueue<TItem>> _pool;
        private readonly Func<TIdentifier, Task<TItem>> _createNewItemFunc;

        public ConcurrentPool(Func<TIdentifier, Task<TItem>> createNewItemFunc) 
        {
            _createNewItemFunc = createNewItemFunc;
            _pool = new();
        }

        public void AddItem(TIdentifier identifier, TItem item)
        {
            var queue = new ConcurrentQueue<TItem>();
            if (!_pool.TryAdd(identifier, queue))
            {
                queue = _pool[identifier];
            }
            queue.Enqueue(item);
        }

        public async Task<(TItem Item, bool NewItem)> GetItemAsync(TIdentifier identifier)
        {
            var queue = new ConcurrentQueue<TItem>();
            var newItem = false;
            if (!_pool.TryAdd(identifier, queue))
            {
                queue = _pool[identifier];
            }
            if(!queue.TryDequeue(out var itemToReturn))
            {
                itemToReturn = await _createNewItemFunc(identifier);
                newItem = true;
            }
            return (itemToReturn, newItem);
        }

        public IEnumerable<TItem> GetAllItems()
        {
            foreach (var queue in _pool.Values)
            {
                while (queue.TryDequeue(out var item))
                {
                    yield return item;
                }
            }
        }
    }
}
