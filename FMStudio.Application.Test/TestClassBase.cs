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
        private SolutionDocumentController solutionDocumentController;

        public TestClassBase()
        {
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new TypeCatalog(
                typeof(ApplicationController), typeof(FileController), typeof(SolutionDocumentController),
                typeof(FileService), typeof(ShellService), typeof(MockEnvironmentService),
                typeof(MainViewModel), typeof(ShellViewModel), typeof(StartViewModel)
            ));
            catalog.Catalogs.Add(new TypeCatalog(
                typeof(MockPresentationService),
                typeof(MockMessageService), typeof(MockFileDialogService),
                typeof(MockShellView), typeof(MockMainView), typeof(MockSolutionView),
                typeof(MockNewDocumentDialogView), typeof(MockNewSolutionDialogView), typeof(MockSaveChangesDialogView),
                typeof(MockStartView)
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
            solutionDocumentController = Container.GetExportedValue<SolutionDocumentController>();
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
