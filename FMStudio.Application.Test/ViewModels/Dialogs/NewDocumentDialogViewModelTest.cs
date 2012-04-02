using System.Collections.Generic;
using BillList.Applications.Documents;
using FMStudio.Application.Test.Documents;
using FMStudio.Application.Test.Views;
using FMStudio.Application.ViewModels.Dialogs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FMStudio.Application.Test.ViewModels.Dialogs
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

            object owner = new object();
            view.ShowDialogAction = v =>
            {
                v.ViewModel.FileName = "NewMockFile";
                v.ViewModel.SelectDocumentType = documentType;
            };
            bool? dialogResult = viewModel.ShowDialog(owner);
            Assert.AreEqual("NewMockFile", viewModel.FileName);
            Assert.AreEqual(documentType, viewModel.SelectDocumentType);
        }
    }
}
