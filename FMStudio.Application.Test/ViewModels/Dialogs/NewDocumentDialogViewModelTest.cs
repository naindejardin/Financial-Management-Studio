using System;
using System.Collections.Generic;
using FMStudio.Applications.Documents;
using FMStudio.Applications.Test.Documents;
using FMStudio.Applications.Test.Views;
using FMStudio.Applications.ViewModels.Dialogs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FMStudio.Applications.Test.ViewModels.Dialogs
{
    [TestClass]
    public class NewDocumentDialogViewModelTest
    {
        [TestMethod]
        public void NewDocumentDialogViewModelCloseTest()
        {
            MockDocumentType documentType = new MockDocumentType("Mock Document Type", ".mock");

            MockNewDocumentDialogView view = new MockNewDocumentDialogView();
            NewDocumentDialogViewModel viewModel = new NewDocumentDialogViewModel(
                view,
                new List<IDocumentType>() { documentType });

            Assert.AreEqual("NewFile", viewModel.FileName);
            Assert.AreEqual(documentType, viewModel.SelectDocumentType);

            object owner = new object();
            bool showDialogCalled = false;
            view.ShowDialogAction = v =>
            {
                showDialogCalled = true;

                //  Cancel
                v.Close();
            };
            bool? dialogResult = viewModel.ShowDialog(owner);
            Assert.IsNull(dialogResult);
            Assert.IsTrue(showDialogCalled);

            showDialogCalled = false;
            view.ShowDialogAction = v =>
            {
                showDialogCalled = true;
                v.ViewModel.FileName = "NewMockFile";
                v.ViewModel.SelectDocumentType = documentType;

                v.ViewModel.OKCommand.Execute(null);
            };
            dialogResult = viewModel.ShowDialog(owner);
            Assert.IsTrue(showDialogCalled);
            Assert.AreEqual(true, dialogResult);
            Assert.AreEqual("NewMockFile", viewModel.FileName);
            Assert.AreEqual(documentType, viewModel.SelectDocumentType);
        }
    }
}
