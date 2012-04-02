using System;
using FMStudio.Application.Test.Views;
using FMStudio.Application.ViewModels.Dialogs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FMStudio.Application.Test.ViewModels
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
