using System;
using System.Collections.Generic;
using System.Linq;
using BigEgg.Framework.Applications;
using BigEgg.Framework.UnitTesting;
using FMStudio.Applications.Controllers;
using FMStudio.Applications.Documents;
using FMStudio.Applications.Services;
using FMStudio.Applications.ViewModels;
using FMStudio.Applications.Views;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FMStudio.Applications.Test.Controllers
{
    [TestClass]
    public class SolutionDocumentControllerTest : TestClassBase
    {
        [TestMethod]
        public void SolutionDocumentControllerConstructorTest()
        {
            AssertHelper.ExpectedException<ArgumentNullException>(() =>
                new SolutionDocumentController(Container, null, null, Container.GetExportedValue<MainViewModel>()));
        }


        [TestMethod]
        public void AddAndRemoveDocumentViewTest()
        {
            MainViewModel mainViewModel = Container.GetExportedValue<MainViewModel>();
            IFileService fileService = Container.GetExportedValue<IFileService>();

            Assert.IsFalse(fileService.OpenedDocuments.Any());
            Assert.IsFalse(mainViewModel.DocumentViews.Any());

            //  Create a new solution document for testing.
            fileService.NewSolutionCommand.Execute(new List<string> {
                Environment.CurrentDirectory, "NewSolution"});
            IDocument document = fileService.OpenedDocuments.Last();

            ISolutionView solutionView = mainViewModel.DocumentViews.OfType<ISolutionView>().Single();
            SolutionViewModel richTextViewModel = ViewHelper.GetViewModel<SolutionViewModel>(solutionView);
            Assert.AreEqual(document, richTextViewModel.Document);
            Assert.AreEqual(1, mainViewModel.DocumentViews.Count);

            // Test ActiveDocument <-> ActiveDocumentView synchronisation
            Assert.IsNotNull(fileService.ActiveDocument);
            Assert.IsNotNull(mainViewModel.ActiveDocumentView);
            Assert.AreEqual(mainViewModel.DocumentViews.First(), mainViewModel.ActiveDocumentView);
            Assert.AreEqual(fileService.OpenedDocuments.First(), fileService.ActiveDocument);

            fileService.ActiveDocument = null;
            Assert.IsNull(fileService.ActiveDocument);
            Assert.IsNull(mainViewModel.ActiveDocumentView);

            //  Close solution document
            fileService.ActiveDocument = fileService.SolutionDoc;
            fileService.CloseDocumentCommand.Execute(fileService.ActiveDocument);
            Assert.IsFalse(fileService.OpenedDocuments.Any());
            Assert.IsFalse(mainViewModel.DocumentViews.Any());
            Assert.IsNull(mainViewModel.ActiveDocumentView);

            //  Show solution document
            fileService.ShowSolutionCommand.Execute(null);
            Assert.IsTrue(fileService.OpenedDocuments.Any());
            Assert.IsTrue(mainViewModel.DocumentViews.Any());
            Assert.IsNotNull(mainViewModel.ActiveDocumentView);

            //  Close Solution
            fileService.CloseSolutionCommand.Execute(null);
            Assert.IsFalse(fileService.OpenedDocuments.Any());
            Assert.IsFalse(mainViewModel.DocumentViews.Any());
            Assert.IsNull(mainViewModel.ActiveDocumentView);
        }
    }
}
