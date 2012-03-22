using System;
using FMStudio.Application.Test.Views;
using FMStudio.Application.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FMStudio.Application.Test.ViewModels
{
    [TestClass]
    public class NewSolutionViewModelTest
    {
        [TestMethod]
        public void NewSolutionViewModelCloseTest()
        {
            MockDialogView view = new MockDialogView();
            NewSolutionDialogViewModel viewModel = new NewSolutionDialogViewModel(
                view, "NewSolutioin", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));

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
