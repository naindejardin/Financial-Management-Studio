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
