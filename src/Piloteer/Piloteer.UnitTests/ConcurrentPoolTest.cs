using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Piloteer.UnitTests
{
    public class ConcurrentPoolTests
    {
        [Fact]
        public void AddItem_AddsItemToPool()
        {
            // Arrange
            var pool = new ConcurrentPool<string, int>(id => Task.FromResult(0));

            // Act
            pool.AddItem("A", 42);
            pool.AddItem("b", 20);

            // Assert
            var items = pool.GetAllItems().ToList();
            Assert.Equal(2, items.Count);
            Assert.True(items.Contains(42));
            Assert.True(items.Contains(20));
        }

        [Fact]
        public async Task GetItemAsync_ReturnsExistingItem()
        {
            // Arrange
            var pool = new ConcurrentPool<string, int>(id => Task.FromResult(99));
            pool.AddItem("B", 123);

            // Act
            var (item, newSession) = await pool.GetItemAsync("B");

            // Assert
            Assert.Equal(123, item);
            Assert.False(newSession);
        }

        [Fact]
        public async Task GetItemAsync_CreatesNewItemIfNoneAvailable()
        {
            // Arrange
            var pool = new ConcurrentPool<string, int>(id => Task.FromResult(77));

            // Act
            var (item, newSession) = await pool.GetItemAsync("C");

            // Assert
            Assert.Equal(77, item);
            Assert.True(newSession);
        }

        [Fact]
        public void GetAllItems_ReturnsAllItemsAndEmptiesPool()
        {
            // Arrange
            var pool = new ConcurrentPool<string, int>(id => Task.FromResult(0));
            pool.AddItem("X", 1);
            pool.AddItem("X", 2);
            pool.AddItem("Y", 3);

            // Act
            var items = pool.GetAllItems().OrderBy(i => i).ToList();

            // Assert
            Assert.Equal(3, items.Count);
            Assert.Contains(1, items);
            Assert.Contains(2, items);
            Assert.Contains(3, items);

            // Pool should now be empty
            var itemsAfter = pool.GetAllItems().ToList();
            Assert.Empty(itemsAfter);
        }

        [Fact]
        public async Task GetItemAsync_ConcurrentAccess()
        {
            // Arrange
            var pool = new ConcurrentPool<string, int>(id => Task.FromResult(555));
            pool.AddItem("Z", 10);

            // Act
            var tasks = new[]
            {
                pool.GetItemAsync("Z"),
                pool.GetItemAsync("Z")
            };
            var results = await Task.WhenAll(tasks);

            // Assert
            Assert.Contains(results, r => r.Item == 10 && !r.NewItem);
            Assert.Contains(results, r => r.Item == 555 && r.NewItem);
        }
    }
}
