using Krusty.Api.Infrastructure;

namespace Krusty.Api.UnitTests
{
    public class MemoryContextTests
    {
        public MemoryContextTests()
        {
            MemoryContext<FakeModel>.ClearBucket();
        }

        [Fact]
        public void AddElement_WhenCalled_ShouldAddOnBucket()
        {
            var model = new FakeModel(1);

            MemoryContext<FakeModel>.AddElement(model);
            var result = MemoryContext<FakeModel>.GetElements(s => s.Id == 1);

            Assert.Equal(model.Id, result.First().Id);
        }

        [Fact]
        public void AddElement_WhenMultipleObjects_ShouldStoreAllOfThem()
        {
            MemoryContext<FakeModel>.AddElement(new FakeModel(1));
            MemoryContext<FakeModel>.AddElement(new FakeModel(2));

            var result = MemoryContext<FakeModel>.GetElements();

            Assert.True(result.Count() == 2);
        }

        [Fact]
        public void RemoveElement_WhenExists_ShouldRemoveElement()
        {
            var model = new FakeModel(1);
            MemoryContext<FakeModel>.AddElement(model);
            MemoryContext<FakeModel>.RemoveElement(model);

            var elements = MemoryContext<FakeModel>.GetElements();

            Assert.True(elements.Count() == 0);
        }

        private sealed class FakeModel
        {
            public int Id { get; private set; }
            public FakeModel(int id) => Id = id;
        }
    }
}
