using System;
using System.Collections.Generic;
using System.IO;
using FMStudio.Applications.Documents;
using FMStudio.Applications.Test.Documents;
using FMStudio.Applications.Test.Views;
using FMStudio.Applications.ViewModels.Dialogs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FMStudio.Applications.Test.ViewModels
{
    [TestClass]
    public class SaveChangesDialogViewModelTest
    {
        [TestMethod]
        public void SaveChangesDialogViewModelCloseTest()
        {
            MockDocumentType documentType = new MockDocumentType("Mock Document", ".mock");
            IEnumerable<IDocument> documents = new IDocument[] 
            {
                documentType.New(Path.Combine(Environment.CurrentDirectory, "Test1")),
                documentType.New(Path.Combine(Environment.CurrentDirectory, "Test2")),
                documentType.New(Path.Combine(Environment.CurrentDirectory, "Test3"))
            };

            MockSaveChangesDialogView view = new MockSaveChangesDialogView();
            SaveChangesDialogViewModel viewModel = new SaveChangesDialogViewModel(view, documents);

            Assert.AreEqual(documents, viewModel.Documents);
        }
    }
}
