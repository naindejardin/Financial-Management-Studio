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

            object owner = new object();
            view.ShowDialogAction = v =>
            {
                v.ViewModel.Location = Environment.CurrentDirectory;
                v.ViewModel.SolutionName = "TestSolution";

            };
            bool? dialogResult = viewModel.ShowDialog(owner);
            Assert.AreEqual(Environment.CurrentDirectory, viewModel.Location);
            Assert.AreEqual("TestSolution", viewModel.SolutionName);
        }
    }
}
