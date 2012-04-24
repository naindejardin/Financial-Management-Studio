using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using FMStudio.SolutionTemplate.Controllers;
using FMStudio.SolutionTemplate.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FMStudio.SolutionTemplate.Test
{
    [TestClass]
    public abstract class TestClassBase
    {
        private readonly CompositionContainer container;

        public TestClassBase()
        {
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new TypeCatalog(
                typeof(TemplateController),
                typeof(TemplateService)
            ));

            this.container = new CompositionContainer(catalog);
            CompositionBatch batch = new CompositionBatch();
            batch.AddExportedValue(container);
            container.Compose(batch);
        }


        protected CompositionContainer Container { get { return this.container; } }


        [TestInitialize]
        public void TestInitialize()
        {
            Container.GetExportedValue<TemplateController>().Initialize();

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
