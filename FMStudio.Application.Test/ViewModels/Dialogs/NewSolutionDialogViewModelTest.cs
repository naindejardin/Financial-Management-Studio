using System;
using FMStudio.Applications.Test.Views;
using FMStudio.Applications.ViewModels.Dialogs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FMStudio.Applications.Test.ViewModels
{
    [TestClass]
    public class NewSolutionDialogViewModelTest
    {
        [TestMethod]
        public void NewSolutionDialogViewModelCloseTest()
        {
            MockNewSolutionDialogView view = new MockNewSolutionDialogView();
            NewSolutionDialogViewModel viewModel = new NewSolutionDialogViewModel(view);

            Assert.AreEqual(String.Empty, viewModel.Location);
            Assert.AreEqual("NewSolution", viewModel.SolutionName);

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
                v.ViewModel.Location = Environment.CurrentDirectory;
                v.ViewModel.SolutionName = "TestSolution";

                v.ViewModel.OKCommand.Execute(null);
            };
            dialogResult = viewModel.ShowDialog(owner);
            Assert.IsTrue(showDialogCalled);
            Assert.AreEqual(true, dialogResult);
            Assert.AreEqual(Environment.CurrentDirectory, viewModel.Location);
            Assert.AreEqual("TestSolution", viewModel.SolutionName);
        }
    }
}
