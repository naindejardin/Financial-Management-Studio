using System;
using System.Collections.Generic;
using System.IO;
using FMStudio.Applications.Test.Views;
using FMStudio.Applications.ViewModels.Dialogs;
using FMStudio.Documents;
using FMStudio.Test.Documents;
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

            object owner = new object();
            view.ShowDialogAction = v =>
            {
                viewModel.YesCommand.Execute(null);
            };
            bool? dialogResult = viewModel.ShowDialog(owner);
            Assert.AreEqual(true, dialogResult);

            view.ShowDialogAction = v =>
            {
                viewModel.NoCommand.Execute(null);
            };
            dialogResult = viewModel.ShowDialog(owner);
            Assert.AreEqual(false, dialogResult);
        }
    }
}
