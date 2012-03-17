using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using FMStudio.Application.Controllers;
using FMStudio.Application.Services;
using FMStudio.Application.Test.Services;
using FMStudio.Application.Test.Views;
using FMStudio.Application.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
                typeof(FileController), 
                typeof(FileService), typeof(ShellService)
            ));
            catalog.Catalogs.Add(new TypeCatalog(
                typeof(MockMessageService), typeof(MockFileDialogService),
                typeof(MockDialogView)
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
            Container.GetExportedValue<FileController>().Initialize();

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
