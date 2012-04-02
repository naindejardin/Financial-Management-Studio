using System;
using System.Collections.Generic;
using System.IO;
using BillList.Applications.Documents;
using FMStudio.Application.Test.Documents;
using FMStudio.Application.Test.Views;
using FMStudio.Application.ViewModels.Dialogs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FMStudio.Application.Test.ViewModels
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
