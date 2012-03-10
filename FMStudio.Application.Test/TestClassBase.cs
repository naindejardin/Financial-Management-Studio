using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FMStudio.Application.Services;

namespace FMStudio.Application.Test
{
    [TestClass]
    public abstract class TestClassBase
    {
        private readonly CompositionContainer container;


        public TestClassBase()
        {
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new TypeCatalog(
                typeof(FileService), typeof(ShellService)
            ));

            container = new CompositionContainer(catalog);
            CompositionBatch batch = new CompositionBatch();
            batch.AddExportedValue(container);
            container.Compose(batch);
        }


        protected CompositionContainer Container { get { return container; } }


        [TestInitialize]
        public void TestInitialize()
        {
            OnTestInitialize();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            OnTestCleanup();
        }

        protected virtual void OnTestInitialize() { }

        protected virtual void OnTestCleanup() { }
    }
}
